import React, { useState, useEffect } from "react";

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
    fetch("http://localhost:8080/api/Authors")
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
    fetch("http://localhost:8080/api/Publishers")
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

    fetch("http://localhost:8080/api/books", {
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
    <div>
      <h2>Create New Book</h2>

      {error && <div style={{color: "red"}}>{error}</div>}
      {successMessage && <div style={{color: "green"}}>{successMessage}</div>}

      <form onSubmit={handleSubmit}>
        <div>
          <label>Title:</label><br/>
          <input
            type="text"
            value={title}
            onChange={e => setTitle(e.target.value)}
          />
        </div>

        <div>
          <label>Author:</label><br/>
          <select
            value={authorId}
            onChange={e => setAuthorId(e.target.value)}
          >
            <option value="">Select author</option>
            {authors.map(author => (
              <option key={author.id} value={author.id}>{author.name}</option>
            ))}
          </select>
        </div>

        <div>
          <label>Publisher:</label><br/>
          <select
            value={publisherId}
            onChange={e => setPublisherId(e.target.value)}
          >
            <option value="">Select publisher</option>
            {publishers.map(pub => (
              <option key={pub.id} value={pub.id}>{pub.name}</option>
            ))}
          </select>
        </div>

        <button type="submit">Create Book</button>
      </form>
    </div>
  );
}
