import React, { useEffect, useState } from "react";
import { api } from "./api";
import StatusMessage from "./StatusMessage";

export default function AuthorsPage() {
  const [authors, setAuthors] = useState([]);
  const [name, setName] = useState("");
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [successMessage, setSuccessMessage] = useState(null);

  const loadAuthors = () => {
    setLoading(true);
    setError(null);
    api
      .get("/api/authors")
      .then((data) => {
        setAuthors(data);
        setLoading(false);
      })
      .catch((err) => {
        if (err.message.includes("No authors found")) {
          setAuthors([]);
          setLoading(false);
          return;
        }
        setError(err.message);
        setLoading(false);
      });
  };

  useEffect(() => {
    loadAuthors();
  }, []);

  const handleSubmit = (event) => {
    event.preventDefault();
    setError(null);
    setSuccessMessage(null);

    const trimmed = name.trim();
    if (!trimmed) {
      setError("Please enter an author name.");
      return;
    }

    api
      .post("/api/authors", { name: trimmed })
      .then((created) => {
        setAuthors((prev) =>
          [...prev, created].sort((a, b) => a.name.localeCompare(b.name))
        );
        setName("");
        setSuccessMessage("Author added successfully.");
      })
      .catch((err) => setError(err.message));
  };

  const deleteAuthor = (authorId) => {
    setError(null);
    setSuccessMessage(null);
    const author = authors.find((item) => item.id === authorId);
    if (!author) return;
    if (!window.confirm(`Delete "${author.name}"?`)) return;

    api
      .del(`/api/authors/${authorId}`)
      .then(() => {
        setAuthors((prev) => prev.filter((item) => item.id !== authorId));
        setSuccessMessage("Author deleted successfully.");
      })
      .catch((err) => setError(err.message));
  };

  if (loading) return <StatusMessage>Loading authors...</StatusMessage>;

  return (
    <div className="page">
      <header className="page-header">
        <div>
          <h2>Authors</h2>
          <p className="page-subtitle">Manage the authors featured in the catalog.</p>
        </div>
      </header>

      <section className="card">
        {error && <StatusMessage tone="error">{error}</StatusMessage>}
        {successMessage && <StatusMessage tone="success">{successMessage}</StatusMessage>}

        <form onSubmit={handleSubmit} className="form-grid">
          <label className="field">
            <span>Author name</span>
            <input
              type="text"
              value={name}
              onChange={(event) => setName(event.target.value)}
              className="input"
              placeholder="e.g., Isabel Dalhousie"
            />
          </label>

          <div className="form-actions">
            <button type="submit" className="btn btn-primary">
              Add Author
            </button>
          </div>
        </form>
      </section>

      {authors.length === 0 ? (
        <StatusMessage>No authors found yet.</StatusMessage>
      ) : (
        <section className="card">
          <div className="card-header">
            <h3>All authors</h3>
            <span className="badge">{authors.length} items</span>
          </div>
          <ul className="book-list">
            {authors.map((author) => (
              <li key={author.id} className="book-item">
                <div className="book-meta">
                  <div className="book-title">{author.name}</div>
                  <div className="book-subtitle">ID: {author.id}</div>
                </div>
                <div className="book-actions">
                  <button
                    onClick={() => deleteAuthor(author.id)}
                    className="btn btn-danger"
                  >
                    Delete
                  </button>
                </div>
              </li>
            ))}
          </ul>
        </section>
      )}
    </div>
  );
}
