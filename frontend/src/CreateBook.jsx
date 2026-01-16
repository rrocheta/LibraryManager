import React, { useState, useEffect } from "react";
import { api } from "./api";
import StatusMessage from "./StatusMessage";

export default function CreateBook() {
  const [title, setTitle] = useState("");
  const [authorId, setAuthorId] = useState("");
  const [publisherId, setPublisherId] = useState("");

  const [authors, setAuthors] = useState([]);
  const [publishers, setPublishers] = useState([]);

  const [loading, setLoading] = useState(true);

  const [error, setError] = useState(null);
  const [successMessage, setSuccessMessage] = useState(null);

  useEffect(() => {
    setLoading(true);
    setError(null);
    Promise.all([api.get("/api/authors"), api.get("/api/publishers")])
      .then(([authorsData, publishersData]) => {
        setAuthors(authorsData);
        setPublishers(publishersData);
        setLoading(false);
      })
      .catch(() => {
        setError("Failed to load authors or publishers");
        setLoading(false);
      });
  }, []);

  // Send form
  const handleSubmit = (e) => {
    e.preventDefault();
    setError(null);
    setSuccessMessage(null);

    if (!title || !authorId || !publisherId) {
      setError("Please fill all fields");
      return;
    }

    const newBook = {
      title,
      authorId: parseInt(authorId),
      publisherId: parseInt(publisherId),
    };

    api
      .post("/api/books", newBook)
      .then(() => {
        setSuccessMessage("Book created successfully!");
        // reset form
        setTitle("");
        setAuthorId("");
        setPublisherId("");
      })
      .catch((err) => setError(err.message));
  };

  if (loading) return <StatusMessage>Loading authors and publishers...</StatusMessage>;

  return (
    <div className="page">
      <header className="page-header">
        <div>
          <h2>Create New Book</h2>
          <p className="page-subtitle">Add a title to the catalog with its author and publisher.</p>
        </div>
      </header>

      <section className="card">
        {error && <StatusMessage tone="error">{error}</StatusMessage>}
        {successMessage && <StatusMessage tone="success">{successMessage}</StatusMessage>}

        <form onSubmit={handleSubmit} className="form-grid">
          <label className="field">
            <span>Title</span>
            <input
              type="text"
              value={title}
              onChange={e => setTitle(e.target.value)}
              className="input"
              placeholder="e.g., The Night Circus"
            />
          </label>

          <label className="field">
            <span>Author</span>
            <select
              value={authorId}
              onChange={e => setAuthorId(e.target.value)}
              className="select"
            >
              <option value="">Select author</option>
              {authors.map(author => (
                <option key={author.id} value={author.id}>{author.name}</option>
              ))}
            </select>
          </label>

          <label className="field">
            <span>Publisher</span>
            <select
              value={publisherId}
              onChange={e => setPublisherId(e.target.value)}
              className="select"
            >
              <option value="">Select publisher</option>
              {publishers.map(pub => (
                <option key={pub.id} value={pub.id}>{pub.name}</option>
              ))}
            </select>
          </label>

          <div className="form-actions">
            <button type="submit" className="btn btn-primary">Create Book</button>
          </div>
        </form>
      </section>
    </div>
  );
}
