import React, { useEffect, useState } from "react";
import { api } from "./api";
import StatusMessage from "./StatusMessage";

export default function ReturnBook() {
  const [books, setBooks] = useState([]);
  const [selectedBookId, setSelectedBookId] = useState("");
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [message, setMessage] = useState(null);
  const [messageTone, setMessageTone] = useState("info");

  const fetchBorrowedBooks = () => {
    setLoading(true);
    api
      .get("/api/books?isBorrowed=true")
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
      setMessageTone("error");
      setMessage("Please select a book to return.");
      return;
    }
    setMessage(null);
    api
      .post(`/api/books/${selectedBookId}/return`)
      .then(() => {
        setMessageTone("success");
        setMessage("Book returned successfully!");
        fetchBorrowedBooks();
      })
      .catch((err) => {
        setMessageTone("error");
        setMessage(err.message);
      });
  };

  if (loading) return <StatusMessage>Loading borrowed books...</StatusMessage>;
  if (error) return <StatusMessage tone="error">Error: {error}</StatusMessage>;

  const hasBooks = books.length > 0;

  return (
    <div className="page">
      <header className="page-header">
        <div>
          <h2>Return a Book</h2>
          <p className="page-subtitle">Confirm the borrowed title to check it back in.</p>
        </div>
      </header>
      {message && <StatusMessage tone={messageTone}>{message}</StatusMessage>}
      {hasBooks ? (
        <section className="card form-grid">
          <label className="field">
            <span>Borrowed books</span>
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
              onClick={handleReturn}
              disabled={!selectedBookId}
              className="btn btn-primary"
            >
              Return
            </button>
          </div>
        </section>
      ) : (
        <StatusMessage>No borrowed books to return at the moment.</StatusMessage>
      )}
    </div>
  );
}
