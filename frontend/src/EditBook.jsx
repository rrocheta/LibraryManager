import React, { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";

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
    Promise.all([
      fetch("http://localhost:8080/api/authors").then(res => res.json()),
      fetch("http://localhost:8080/api/publishers").then(res => res.json())
    ])
      .then(([authorsData, publishersData]) => {
        setAuthors(authorsData);
        setPublishers(publishersData);
      })
      .catch(() => setError("Failed to load authors or publishers"));

    // Fetch book details
    fetch(`http://localhost:8080/api/books/${id}`)
      .then(res => {
        if (!res.ok) throw new Error("Failed to load book");
        return res.json();
      })
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

    fetch(`http://localhost:8080/api/books/${id}`, {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(updatedBook)
    })
      .then(res => {
        if (!res.ok) throw new Error("Failed to update book");
        alert("Book updated successfully!");
        navigate("/");
      })
      .catch(err => {
        setError(err.message);
        setSaving(false);
      });
  };

  if (loading) return <div>Loading book details...</div>;

  return (
    <div>
      <h2>Edit Book</h2>
      {error && <div style={{ color: "red" }}>{error}</div>}

      <form onSubmit={handleSubmit}>
        <div>
          <label>Title:</label><br />
          <input
            type="text"
            value={title}
            onChange={e => setTitle(e.target.value)}
          />
        </div>

        <div>
          <label>Author:</label><br />
          <select value={authorId} onChange={e => setAuthorId(e.target.value)}>
            <option value="">Select author</option>
            {authors.map(author => (
              <option key={author.id} value={author.id}>{author.name}</option>
            ))}
          </select>
        </div>

        <div>
          <label>Publisher:</label><br />
          <select value={publisherId} onChange={e => setPublisherId(e.target.value)}>
            <option value="">Select publisher</option>
            {publishers.map(pub => (
              <option key={pub.id} value={pub.id}>{pub.name}</option>
            ))}
          </select>
        </div>

        <button type="submit" disabled={saving}>Save</button>
      </form>
    </div>
  );
}
