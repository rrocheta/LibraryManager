import React, { useEffect, useState } from "react";
import { API_BASE_URL } from "./config";

export default function ReturnBook() {
  const [books, setBooks] = useState([]);
  const [selectedBookId, setSelectedBookId] = useState("");
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const fetchBorrowedBooks = () => {
    setLoading(true);
    fetch(`${API_BASE_URL}/api/books?isBorrowed=true`)
      .then((res) => {
        if (!res.ok) throw new Error("Failed to fetch books");
        return res.json();
      })
      .then((data) => {
        setBooks(data);
        setLoading(false);
        setSelectedBookId("");
      })
      .catch((err) => {
        setError(err.message);
        setLoading(false);
      });
  };

  useEffect(() => {
    fetchBorrowedBooks();
  }, []);

  const handleReturn = () => {
    if (!selectedBookId) {
      alert("Please select a book to return.");
      return;
    }
    fetch(`${API_BASE_URL}/api/books/${selectedBookId}/return`, {
      method: "POST",
    })
      .then((res) => {
        if (!res.ok) throw new Error("Failed to return book");
        alert("Book returned successfully!");
        fetchBorrowedBooks();
      })
      .catch((err) => alert(err.message));
  };

  if (loading) return <div>Loading borrowed books...</div>;
  if (error) return <div>Error: {error}</div>;

  if (books.length === 0) return <div>No borrowed books to return at the moment.</div>;

  return (
    <div>
      <h2>Choose a book to return</h2>
      <select
        value={selectedBookId}
        onChange={(e) => setSelectedBookId(e.target.value)}
      >
        <option value="">-- Select a book --</option>
        {books.map((book) => (
          <option key={book.id} value={book.id}>
            {book.title} — Author: {book.author.name}
          </option>
        ))}
      </select>
      <button onClick={handleReturn} disabled={!selectedBookId}>
        Return
      </button>
    </div>
  );
}