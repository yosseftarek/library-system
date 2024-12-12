﻿open System
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