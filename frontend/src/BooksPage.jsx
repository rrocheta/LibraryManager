import React, { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { API_BASE_URL } from './config';

export default function BooksPage() {
  const [books, setBooks] = useState([]);
  const [authors, setAuthors] = useState([]);
  const [filterTitle, setFilterTitle] = useState("");
  const [selectedAuthorId, setSelectedAuthorId] = useState("");
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const navigate = useNavigate();

  const fetchBooks = () => {
    setLoading(true);
    let url = `${API_BASE_URL}/api/books`;

    if (selectedAuthorId) {
      url += `?authorId=${selectedAuthorId}`;
    }

    fetch(url)
      .then((res) => {
        if (!res.ok) throw new Error("Failed to fetch books");
        return res.json();
      })
      .then((data) => {
        setBooks(data);
        setLoading(false);
      })
      .catch((err) => {
        setError(err.message);
        setLoading(false);
      });
  };

  const fetchAuthors = () => {
    fetch(`${API_BASE_URL}/api/authors`)
      .then((res) => res.json())
      .then((data) => setAuthors(data));
  };

  const deleteBook = (id) => {
    if (!window.confirm("Are you sure you want to delete this book?")) return;

    fetch(`${API_BASE_URL}/api/books/${id}`, {
      method: "DELETE",
    })
      .then(async (res) => {
        if (!res.ok) {
          // tenta ler a mensagem de erro do backend
          const errorText = await res.text();
          throw new Error(errorText || "Failed to delete book");
        }
        fetchBooks();
      })
      .catch((err) => {
        if (err.message.includes("Cannot delete a borrowed book")) {
          alert("This book is currently borrowed and cannot be deleted.");
        } else {
          alert(err.message);
        }
      });
  };

  useEffect(() => {
    fetchAuthors();
  }, []);

  useEffect(() => {
    fetchBooks();
  }, [selectedAuthorId]);

  const filteredBooks = books.filter((book) =>
    book.title.toLowerCase().includes(filterTitle.toLowerCase())
  );

  return (
    <div className="page">
      <header className="page-header">
        <div>
          <h2>Catalog</h2>
          <p className="page-subtitle">Manage the bookstore inventory and availability.</p>
        </div>
        <div className="header-actions">
          <Link className="btn btn-primary" to="/create">Add New</Link>
        </div>
      </header>

      <section className="card">
        <div className="filters">
          <label className="field">
            <span>Filter by title</span>
            <input
              type="text"
              value={filterTitle}
              onChange={(e) => setFilterTitle(e.target.value)}
              className="input"
              placeholder="Search title..."
            />
          </label>

          <label className="field">
            <span>Filter by author</span>
            <select
              value={selectedAuthorId}
              onChange={(e) => setSelectedAuthorId(e.target.value)}
              className="select"
            >
              <option value="">All authors</option>
              {authors.map((author) => (
                <option key={author.id} value={author.id}>
                  {author.name}
                </option>
              ))}
            </select>
          </label>

          <div className="field filters-actions">
            {(filterTitle || selectedAuthorId) && (
              <button
                onClick={() => {
                  setFilterTitle("");
                  setSelectedAuthorId("");
                }}
                className="btn btn-ghost"
              >
                Clear filters
              </button>
            )}
          </div>
        </div>
      </section>

      {loading ? (
        <div className="state">Loading books...</div>
      ) : error ? (
        <div className="state error">Error: {error}</div>
      ) : filteredBooks.length === 0 ? (
        <div className="state">No books found.</div>
      ) : (
        <section className="card">
          <div className="card-header">
            <h3>All available books</h3>
            <span className="badge">{filteredBooks.length} items</span>
          </div>
          <ul className="book-list">
            {filteredBooks.map((book) => (
              <li key={book.id} className="book-item">
                <div className="book-meta">
                  <div className="book-title">{book.title}</div>
                  <div className="book-subtitle">
                    Author: {book.author?.name} · Publisher: {book.publisher?.name}
                  </div>
                </div>
                <div className="book-actions">
                  <span className={book.isBorrowed ? "pill pill-borrowed" : "pill pill-available"}>
                    {book.isBorrowed ? "Borrowed" : "Available"}
                  </span>
                  <button
                    onClick={() => navigate(`/edit/${book.id}`)}
                    className="btn btn-outline"
                  >
                    Edit
                  </button>
                  <button
                    onClick={() => deleteBook(book.id)}
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
