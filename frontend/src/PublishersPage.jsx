import React, { useEffect, useState } from "react";
import { api } from "./api";
import StatusMessage from "./StatusMessage";

export default function PublishersPage() {
  const [publishers, setPublishers] = useState([]);
  const [name, setName] = useState("");
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [successMessage, setSuccessMessage] = useState(null);

  const loadPublishers = () => {
    setLoading(true);
    setError(null);
    api
      .get("/api/publishers")
      .then((data) => {
        setPublishers(data);
        setLoading(false);
      })
      .catch((err) => {
        if (err.message.includes("No publishers found")) {
          setPublishers([]);
          setLoading(false);
          return;
        }
        setError(err.message);
        setLoading(false);
      });
  };

  useEffect(() => {
    loadPublishers();
  }, []);

  const handleSubmit = (event) => {
    event.preventDefault();
    setError(null);
    setSuccessMessage(null);

    const trimmed = name.trim();
    if (!trimmed) {
      setError("Please enter a publisher name.");
      return;
    }

    api
      .post("/api/publishers", { name: trimmed })
      .then((created) => {
        setPublishers((prev) =>
          [...prev, created].sort((a, b) => a.name.localeCompare(b.name))
        );
        setName("");
        setSuccessMessage("Publisher added successfully.");
      })
      .catch((err) => setError(err.message));
  };

  const deletePublisher = (publisherId) => {
    setError(null);
    setSuccessMessage(null);
    const publisher = publishers.find((item) => item.id === publisherId);
    if (!publisher) return;
    if (!window.confirm(`Delete "${publisher.name}"?`)) return;

    api
      .del(`/api/publishers/${publisherId}`)
      .then(() => {
        setPublishers((prev) => prev.filter((item) => item.id !== publisherId));
        setSuccessMessage("Publisher deleted successfully.");
      })
      .catch((err) => setError(err.message));
  };

  if (loading) return <StatusMessage>Loading publishers...</StatusMessage>;

  return (
    <div className="page">
      <header className="page-header">
        <div>
          <h2>Publishers</h2>
          <p className="page-subtitle">Manage the publishing houses in the catalog.</p>
        </div>
      </header>

      <section className="card">
        {error && <StatusMessage tone="error">{error}</StatusMessage>}
        {successMessage && <StatusMessage tone="success">{successMessage}</StatusMessage>}

        <form onSubmit={handleSubmit} className="form-grid">
          <label className="field">
            <span>Publisher name</span>
            <input
              type="text"
              value={name}
              onChange={(event) => setName(event.target.value)}
              className="input"
              placeholder="e.g., Riverstone Press"
            />
          </label>

          <div className="form-actions">
            <button type="submit" className="btn btn-primary">
              Add Publisher
            </button>
          </div>
        </form>
      </section>

      {publishers.length === 0 ? (
        <StatusMessage>No publishers found yet.</StatusMessage>
      ) : (
        <section className="card">
          <div className="card-header">
            <h3>All publishers</h3>
            <span className="badge">{publishers.length} items</span>
          </div>
          <ul className="book-list">
            {publishers.map((publisher) => (
              <li key={publisher.id} className="book-item">
                <div className="book-meta">
                  <div className="book-title">{publisher.name}</div>
                  <div className="book-subtitle">ID: {publisher.id}</div>
                </div>
                <div className="book-actions">
                  <button
                    onClick={() => deletePublisher(publisher.id)}
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
