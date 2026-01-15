import React, { useState, useEffect } from "react";
import { API_BASE_URL } from "./config";

export default function CreateBook() {
  const [title, setTitle] = useState("");
  const [authorId, setAuthorId] = useState("");
  const [publisherId, setPublisherId] = useState("");

  const [authors, setAuthors] = useState([]);
  const [publishers, setPublishers] = useState([]);

  const [loadingAuthors, setLoadingAuthors] = useState(true);
  const [loadingPublishers, setLoadingPublishers] = useState(true);

  const [error, setError] = useState(null);
  const [successMessage, setSuccessMessage] = useState(null);

  // Fetch Authors
  useEffect(() => {
    fetch(`${API_BASE_URL}/api/authors`)
      .then(res => res.json())
      .then(data => {
        setAuthors(data);
        setLoadingAuthors(false);
      })
      .catch(err => {
        setError("Failed to load authors");
        setLoadingAuthors(false);
      });
  }, []);

  // Fetch publishers
  useEffect(() => {
    fetch(`${API_BASE_URL}/api/publishers`)
      .then(res => res.json())
      .then(data => {
        setPublishers(data);
        setLoadingPublishers(false);
      })
      .catch(err => {
        setError("Failed to load publishers");
        setLoadingPublishers(false);
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

    fetch(`${API_BASE_URL}/api/books`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(newBook)
    })
    .then(res => {
      if (!res.ok) {
        throw new Error("Failed to create book");
      }
      return res.json();
    })
    .then(data => {
      setSuccessMessage("Book created successfully!");
      // reset form
      setTitle("");
      setAuthorId("");
      setPublisherId("");
    })
    .catch(err => setError(err.message));
  };

  if (loadingAuthors || loadingPublishers) return <div>Loading...</div>;

  return (
    <div className="page">
      <header className="page-header">
        <div>
          <h2>Create New Book</h2>
          <p className="page-subtitle">Add a title to the catalog with its author and publisher.</p>
        </div>
      </header>

      <section className="card">
        {error && <div className="state error">{error}</div>}
        {successMessage && <div className="state success">{successMessage}</div>}

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
