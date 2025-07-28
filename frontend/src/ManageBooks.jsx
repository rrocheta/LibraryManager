import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";

export default function ManageBooks() {
  const [books, setBooks] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const fetchAvailableBooks = () => {
    setLoading(true);
    fetch("http://localhost:8080/api/books?isBorrowed=false")
      .then(res => {
        if (!res.ok) throw new Error("Failed to fetch books");
        return res.json();
      })
      .then(data => {
        setBooks(data);
        setLoading(false);
      })
      .catch(err => {
        setError(err.message);
        setLoading(false);
      });
  };

  useEffect(() => {
    fetchAvailableBooks();
  }, []);

  const deleteBook = (id) => {
    if (!window.confirm("Are you sure you want to delete this book?")) return;

    fetch(`http://localhost:8080/api/books/${id}`, {
      method: "DELETE"
    })
      .then(res => {
        if (!res.ok) throw new Error("Failed to delete book");
        // Refresh list
        fetchAvailableBooks();
      })
      .catch(err => alert(err.message));
  };

  if (loading) return <div>Loading books...</div>;
  if (error) return <div>Error: {error}</div>;
  if (books.length === 0) return <div>No available books to manage.</div>;

  return (
    <div>
      <h2>Manage Books</h2>
      <ul>
        {books.map(book => (
          <li key={book.id}>
            <strong>{book.title}</strong> — Author: {book.author.name} — Publisher: {book.publisher.name}{" "}
            <Link to={`/edit/${book.id}`}>Edit</Link>{" | "}
            <button onClick={() => deleteBook(book.id)}>Delete</button>
          </li>
        ))}
      </ul>
    </div>
  );
}
