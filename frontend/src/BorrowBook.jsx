import React, { useEffect, useState } from "react";
import { api } from "./api";
import StatusMessage from "./StatusMessage";

export default function BorrowBook() {
  const [books, setBooks] = useState([]);
  const [selectedBookId, setSelectedBookId] = useState("");
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [message, setMessage] = useState(null);
  const [messageTone, setMessageTone] = useState("info");

  const fetchAvailableBooks = () => {
    setLoading(true);
    api
      .get("/api/books?isBorrowed=false")
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
      setMessageTone("error");
      setMessage("Please select a book.");
      return;
    }
    setMessage(null);
    api
      .post(`/api/books/${selectedBookId}/borrow`)
      .then(() => {
        setMessageTone("success");
        setMessage("Book borrowed successfully!");
        fetchAvailableBooks();
      })
      .catch((err) => {
        setMessageTone("error");
        setMessage(err.message);
      });
  };

  if (loading) return <StatusMessage>Loading available books...</StatusMessage>;
  if (error) return <StatusMessage tone="error">Error: {error}</StatusMessage>;

  const hasBooks = books.length > 0;

  return (
    <div className="page">
      <header className="page-header">
        <div>
          <h2>Borrow a Book</h2>
          <p className="page-subtitle">Select a title and confirm availability.</p>
        </div>
      </header>
      {message && <StatusMessage tone={messageTone}>{message}</StatusMessage>}
      {hasBooks ? (
        <section className="card form-grid">
          <label className="field">
            <span>Available books</span>
            <select
              value={selectedBookId}
              onChange={(e) => setSelectedBookId(e.target.value)}
              className="select"
            >
              <option value="">-- Select a book --</option>
              {books.map((book) => (
                <option key={book.id} value={book.id}>
                  {book.title} - Author: {book.author.name}
                </option>
              ))}
            </select>
          </label>
          <div className="form-actions">
            <button
              onClick={handleBorrow}
              disabled={!selectedBookId}
              className="btn btn-primary"
            >
              Borrow
            </button>
          </div>
        </section>
      ) : (
        <StatusMessage>No available books to borrow.</StatusMessage>
      )}
    </div>
  );
}
