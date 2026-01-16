import React, { useEffect, useState } from "react";
import { Link, useLocation, useNavigate } from "react-router-dom";
import { api } from "./api";
import StatusMessage from "./StatusMessage";

export default function BooksPage() {
  const [books, setBooks] = useState([]);
  const [authors, setAuthors] = useState([]);
  const [filterTitle, setFilterTitle] = useState("");
  const [selectedAuthorId, setSelectedAuthorId] = useState("");
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [pageMessage, setPageMessage] = useState(null);
  const [pageTone, setPageTone] = useState("info");

  // Trigger simples para refazer o fetch quando precisa (ex: depois de apagar)
  const [reloadKey, setReloadKey] = useState(0);

  const navigate = useNavigate();
  const location = useLocation();

  const fetchAuthors = () =>
    api
      .get("/api/authors")
      .then((data) => setAuthors(data))
      .catch(() => {
        // opcional: não bloquear a página toda se falhar authors
      });

  const deleteBook = (id) => {
    if (!window.confirm("Are you sure you want to delete this book?")) return;

    setPageMessage(null);
    api
      .del(`/api/books/${id}`)
      .then(() => {
        setPageTone("success");
        setPageMessage("Book deleted successfully.");
        // força refresh da lista
        setReloadKey((k) => k + 1);
      })
      .catch((err) => {
        if (err.message.includes("Cannot delete a borrowed book")) {
          setPageMessage("This book is currently borrowed and cannot be deleted.");
        } else {
          setPageMessage(err.message);
        }
        setPageTone("error");
      });
  };

  useEffect(() => {
    fetchAuthors();
  }, []);

  useEffect(() => {
    const fetchBooks = () => {
      setLoading(true);
      setError(null);

      let url = "/api/books";
      if (selectedAuthorId) {
        const params = new URLSearchParams({ authorId: selectedAuthorId });
        url += `?${params.toString()}`;
      }

      api
        .get(url)
        .then((data) => {
          setBooks(data);
          setLoading(false);
        })
        .catch((err) => {
          setError(err.message);
          setLoading(false);
        });
    };

    fetchBooks();
  }, [selectedAuthorId, reloadKey]);

  useEffect(() => {
    if (location.state?.message) {
      setPageMessage(location.state.message);
      setPageTone(location.state.tone || "success");
      navigate(location.pathname, { replace: true, state: null });
    }
  }, [location, navigate]);

  const filteredBooks = books.filter((book) =>
    book.title.toLowerCase().includes(filterTitle.toLowerCase())
  );

  return (
    <div className="page">
      <header className="page-header">
        <div>
          <h2>Catalog</h2>
          <p className="page-subtitle">
            Manage the bookstore inventory and availability.
          </p>
        </div>
        <div className="header-actions">
          <Link className="btn btn-primary" to="/create">
            Add New
          </Link>
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

      {pageMessage && <StatusMessage tone={pageTone}>{pageMessage}</StatusMessage>}

      {loading ? (
        <StatusMessage>Loading books...</StatusMessage>
      ) : error ? (
        <StatusMessage tone="error">Error: {error}</StatusMessage>
      ) : filteredBooks.length === 0 ? (
        <StatusMessage>No books found.</StatusMessage>
      ) : (
        <section className="card">
          <div className="card-header">
            <h3>All books</h3>
            <span className="badge">{filteredBooks.length} items</span>
          </div>
          <ul className="book-list">
            {filteredBooks.map((book) => {
              const isBorrowed = book.isBorrowed;
              return (
                <li key={book.id} className="book-item">
                  <div className="book-meta">
                    <div className="book-title">{book.title}</div>
                    <div className="book-subtitle">
                      Author: {book.author?.name} · Publisher:{" "}
                      {book.publisher?.name}
                    </div>
                  </div>
                  <div className="book-actions">
                    <span
                      className={
                        isBorrowed ? "pill pill-borrowed" : "pill pill-available"
                      }
                    >
                      {isBorrowed ? "Borrowed" : "Available"}
                    </span>
                    <button
                      onClick={() => navigate(`/edit/${book.id}`)}
                      className="btn btn-outline"
                      disabled={isBorrowed}
                    >
                      Edit
                    </button>
                    <button
                      onClick={() => deleteBook(book.id)}
                      className="btn btn-danger"
                      disabled={isBorrowed}
                    >
                      Delete
                    </button>
                  </div>
                </li>
              );
            })}
          </ul>
        </section>
      )}
    </div>
  );
}
