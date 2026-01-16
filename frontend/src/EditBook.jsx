import React, { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { api } from "./api";
import StatusMessage from "./StatusMessage";

export default function EditBook() {
  const { id } = useParams();
  const navigate = useNavigate();

  const [title, setTitle] = useState("");
  const [authorId, setAuthorId] = useState("");
  const [publisherId, setPublisherId] = useState("");

  const [authors, setAuthors] = useState([]);
  const [publishers, setPublishers] = useState([]);

  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    // Fetch authors and publishers in parallel
    Promise.all([api.get("/api/authors"), api.get("/api/publishers")])
      .then(([authorsData, publishersData]) => {
        setAuthors(authorsData);
        setPublishers(publishersData);
      })
      .catch(() => setError("Failed to load authors or publishers"));

    // Fetch book details
    api
      .get(`/api/books/${id}`)
      .then(data => {
        setTitle(data.title);
        setAuthorId(data.author.id);
        setPublisherId(data.publisher.id);
        setLoading(false);
      })
      .catch(() => setError("Failed to load book details"));
  }, [id]);

  const handleSubmit = (e) => {
    e.preventDefault();
    setError(null);
    setSaving(true);

    if (!title || !authorId || !publisherId) {
      setError("Please fill all fields");
      setSaving(false);
      return;
    }

    const updatedBook = {
      id: id,
      title,
      author: { id: parseInt(authorId) },
      publisher: { id: parseInt(publisherId) },
      isBorrowed: false  // Edit only allowed for not borrowed books
    };

    api
      .put(`/api/books/${id}`, updatedBook)
      .then(() => {
        navigate("/", {
          state: { message: "Book updated successfully!", tone: "success" },
        });
      })
      .catch((err) => {
        if (err.message.includes("Cannot update a borrowed book")) {
          setError("This book is currently borrowed and cannot be edited.");
        } else {
          setError(err.message);
        }
        setSaving(false);
      });
  };

  if (loading) return <StatusMessage>Loading book details...</StatusMessage>;

  return (
    <div className="page">
      <header className="page-header">
        <div>
          <h2>Edit Book</h2>
          <p className="page-subtitle">Update catalog details without losing context.</p>
        </div>
      </header>
      <section className="card">
        {error && <StatusMessage tone="error">{error}</StatusMessage>}

        <form onSubmit={handleSubmit} className="form-grid">
          <label className="field">
            <span>Title</span>
            <input
              type="text"
              value={title}
              onChange={e => setTitle(e.target.value)}
              className="input"
            />
          </label>

          <label className="field">
            <span>Author</span>
            <select value={authorId} onChange={e => setAuthorId(e.target.value)} className="select">
              <option value="">Select author</option>
              {authors.map(author => (
                <option key={author.id} value={author.id}>{author.name}</option>
              ))}
            </select>
          </label>

          <label className="field">
            <span>Publisher</span>
            <select value={publisherId} onChange={e => setPublisherId(e.target.value)} className="select">
              <option value="">Select publisher</option>
              {publishers.map(pub => (
                <option key={pub.id} value={pub.id}>{pub.name}</option>
              ))}
            </select>
          </label>

          <div className="form-actions">
            <button type="submit" disabled={saving} className="btn btn-primary">
              {saving ? "Saving..." : "Save"}
            </button>
          </div>
        </form>
      </section>
    </div>
  );
}
