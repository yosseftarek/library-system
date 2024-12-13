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