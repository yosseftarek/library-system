open System
open System.Windows.Forms
open System.Drawing

// Book Record Type
type Book = {
    Title: string
    Author: string
    Genre: string
    IsBorrowed: bool
    BorrowDate: DateTime option
}

// Initial Library Data
let mutable library = [
    { Title = "Book 1"; Author = "Author 1"; Genre = "Fiction"; IsBorrowed = false; BorrowDate = None }
    { Title = "Book 2"; Author = "Author 2"; Genre = "Science"; IsBorrowed = true; BorrowDate = Some(DateTime.Now.AddDays(-5.0)) }
]

// Helper Functions
let addBook title author genre =
    library <- { Title = title; Author = author; Genre = genre; IsBorrowed = false; BorrowDate = None } :: library

let searchBooks title =
    library |> List.filter (fun book -> book.Title.IndexOf(title, StringComparison.OrdinalIgnoreCase) >= 0)

let getAvailableBooks () =
    library |> List.filter (fun book -> not book.IsBorrowed)

let getBorrowedBooks () =
    library |> List.filter (fun book -> book.IsBorrowed)

let getAllBooks () =
    library 
    |> List.map (fun book ->
        if book.IsBorrowed then
            $"{book.Title} by {book.Author} (Borrowed on {book.BorrowDate.Value.ToShortDateString()})"
        else
            $"{book.Title} by {book.Author} (Available)"
    )
    |> String.concat "\n"

    let borrowBook title =
    let mutable borrowed = false
    library <- 
        library |> List.map (fun book ->
            if book.Title.Equals(title, StringComparison.OrdinalIgnoreCase) && not book.IsBorrowed then
                borrowed <- true
                { book with IsBorrowed = true; BorrowDate = Some(DateTime.Now) }
            else book
        )
    if borrowed then
        MessageBox.Show($"You have successfully borrowed '{title}'!") |> ignore
    else
        MessageBox.Show("Book is already borrowed or not found!") |> ignore

let returnBook title =
    let mutable returned = false
    library <- 
        library |> List.map (fun book ->
            if book.Title.Equals(title, StringComparison.OrdinalIgnoreCase) && book.IsBorrowed then
                returned <- true
                { book with IsBorrowed = false; BorrowDate = None }
            else book
        )
    if returned then
        MessageBox.Show($"You have successfully returned '{title}'!") |> ignore
    else
        MessageBox.Show("Book is already available or not found!") |> ignore

// Form Setup
let form = new Form(Text = "Library Management System", Width = 800, Height = 600, StartPosition = FormStartPosition.CenterScreen)

// Placeholder Logic for TextBox
let setupPlaceholder (textBox: TextBox) (placeholder: string) =
    textBox.Text <- placeholder
    textBox.ForeColor <- Color.Gray
    textBox.GotFocus.Add(fun _ -> 
        if textBox.Text = placeholder then
            textBox.Text <- ""
            textBox.ForeColor <- Color.Black
    )
    textBox.LostFocus.Add(fun _ -> 
        if String.IsNullOrWhiteSpace(textBox.Text) then
            textBox.Text <- placeholder
            textBox.ForeColor <- Color.Gray
    )

// UI Elements
let addBookTitleBox = new TextBox(Top = 50, Left = 150, Width = 200)
setupPlaceholder addBookTitleBox "Enter Book Title"

let addBookAuthorBox = new TextBox(Top = 90, Left = 150, Width = 200)
setupPlaceholder addBookAuthorBox "Enter Author"

let addBookGenreBox = new TextBox(Top = 130, Left = 150, Width = 200)
setupPlaceholder addBookGenreBox "Enter Genre"

let addButton = new Button(Text = "Add Book", Top = 170, Left = 150, Width = 100)
addButton.Click.Add(fun _ -> 
    if addBookTitleBox.Text <> "Enter Book Title" && addBookAuthorBox.Text <> "Enter Author" && addBookGenreBox.Text <> "Enter Genre" then
        addBook addBookTitleBox.Text addBookAuthorBox.Text addBookGenreBox.Text
        MessageBox.Show("Book added successfully!") |> ignore
)

let searchBox = new TextBox(Top = 220, Left = 150, Width = 200)
setupPlaceholder searchBox "Search Title"

let searchButton = new Button(Text = "Search Book", Top = 260, Left = 150, Width = 100)
searchButton.Click.Add(fun _ -> 
    let results = searchBooks searchBox.Text
    if results.Length > 0 then
        let resultText = results |> List.map (fun b -> $"{b.Title} by {b.Author}") |> String.concat "\n"
        MessageBox.Show($"Found Books:\n{resultText}") |> ignore
    else
        MessageBox.Show("No books found!") |> ignore
)

let availableBooksButton = new Button(Text = "Show Available Books", Top = 310, Left = 150, Width = 150)
availableBooksButton.Click.Add(fun _ -> 
    let results = getAvailableBooks ()
    let resultText = results |> List.map (fun b -> $"{b.Title} by {b.Author} (Available)") |> String.concat "\n"
    MessageBox.Show($"Available Books:\n{resultText}") |> ignore
)

let borrowedBooksButton = new Button(Text = "Show Borrowed Books", Top = 360, Left = 150, Width = 150)
borrowedBooksButton.Click.Add(fun _ -> 
    let results = getBorrowedBooks ()
    let resultText = results |> List.map (fun b -> $"{b.Title} (Borrowed on {b.BorrowDate.Value.ToShortDateString()})") |> String.concat "\n"
    MessageBox.Show($"Borrowed Books:\n{resultText}") |> ignore
)

let allBooksButton = new Button(Text = "Show All Books", Top = 410, Left = 150, Width = 150)
allBooksButton.Click.Add(fun _ -> 
    let results = getAllBooks ()
    MessageBox.Show($"All Books:\n{results}") |> ignore
)

let borrowTitleBox = new TextBox(Top = 450, Left = 150, Width = 200)
setupPlaceholder borrowTitleBox "Enter Book Title to Borrow"

let borrowButton = new Button(Text = "Borrow Book", Top = 450, Left = 400, Width = 100)
borrowButton.Click.Add(fun _ -> 
    if borrowTitleBox.Text <> "Enter Book Title to Borrow" then
        borrowBook borrowTitleBox.Text
)

let returnTitleBox = new TextBox(Top = 500, Left = 150, Width = 200)
setupPlaceholder returnTitleBox "Enter Book Title to Return"

let returnButton = new Button(Text = "Return Book", Top = 500, Left = 400, Width = 100)
returnButton.Click.Add(fun _ -> 
    if returnTitleBox.Text <> "Enter Book Title to Return" then
        returnBook returnTitleBox.Text
)

form.Controls.Add(addBookTitleBox)
form.Controls.Add(addBookAuthorBox)
form.Controls.Add(addBookGenreBox)
form.Controls.Add(addButton)
form.Controls.Add(searchBox)
form.Controls.Add(searchButton)
form.Controls.Add(availableBooksButton)
form.Controls.Add(borrowedBooksButton)
form.Controls.Add(allBooksButton)
form.Controls.Add(borrowTitleBox)
form.Controls.Add(borrowButton)
form.Controls.Add(returnTitleBox)
form.Controls.Add(returnButton)

// Run the Application
[<STAThread>]
Application.EnableVisualStyles()
Application.Run(form)