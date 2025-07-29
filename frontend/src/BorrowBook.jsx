import React, { useEffect, useState } from "react";
import { API_BASE_URL } from "./config";

export default function BorrowBook() {
  const [books, setBooks] = useState([]);
  const [selectedBookId, setSelectedBookId] = useState("");
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const fetchAvailableBooks = () => {
    setLoading(true);
    fetch(`${API_BASE_URL}/api/books?isBorrowed=false`)
      .then((res) => {
        if (!res.ok) throw new Error("Error fetching books");
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
    fetchAvailableBooks();
  }, []);

  const handleBorrow = () => {
    if (!selectedBookId) {
      alert("Please select a book.");
      return;
    }
    fetch(`${API_BASE_URL}/api/books/${selectedBookId}/borrow`, {
      method: "POST",
    })
      .then((res) => {
        if (!res.ok) throw new Error("Failed to borrow book");
        alert("Book borrowed successfully!");
        fetchAvailableBooks();
      })
      .catch((err) => alert(err.message));
  };

  if (loading) return <div>Loading available books...</div>;
  if (error) return <div>Error: {error}</div>;

  if (books.length === 0) return <div>No available books to borrow.</div>;

  return (
    <div>
      <h2>Choose a book to borrow</h2>
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
      <button onClick={handleBorrow} disabled={!selectedBookId}>
        Borrow
      </button>
    </div>
  );
}