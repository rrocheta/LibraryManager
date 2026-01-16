import React, { useEffect, useState } from "react";
import { Link, useLocation, useNavigate } from "react-router-dom";
import { api } from "./api";
import StatusMessage from "./StatusMessage";

export default function BooksPage() {
  const [books, setBooks] = useState([]);
  const [authors, setAuthors] = useState([]);
  const [filterTitle, setFilterTitle] = useState("");
  const [selectedAuthorId, setSelectedAuthorId] = useState("");
  const [selectedStatus, setSelectedStatus] = useState("");
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [pageMessage, setPageMessage] = useState(null);
  const [pageTone, setPageTone] = useState("info");
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [totalCount, setTotalCount] = useState(0);
  const pageSize = 10;

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
    setPage(1);
  }, [filterTitle, selectedAuthorId, selectedStatus]);

  useEffect(() => {
    const fetchBooks = () => {
      setLoading(true);
      setError(null);

      const params = new URLSearchParams({
        page: String(page),
        pageSize: String(pageSize),
      });

      if (selectedAuthorId) {
        params.set("authorId", selectedAuthorId);
      }

      if (filterTitle.trim()) {
        params.set("title", filterTitle.trim());
      }

      if (selectedStatus) {
        params.set(
          "isBorrowed",
          selectedStatus === "borrowed" ? "true" : "false"
        );
      }

      const url = `/api/books/paged?${params.toString()}`;
      api
        .get(url)
        .then((data) => {
          setBooks(data.items || []);
          setTotalCount(data.totalCount || 0);
          setTotalPages(data.totalPages || 1);
          setLoading(false);
        })
        .catch((err) => {
          setError(err.message);
          setLoading(false);
        });
    };

    fetchBooks();
  }, [selectedAuthorId, filterTitle, selectedStatus, page, reloadKey]);

  useEffect(() => {
    if (location.state?.message) {
      setPageMessage(location.state.message);
      setPageTone(location.state.tone || "success");
      navigate(location.pathname, { replace: true, state: null });
    }
  }, [location, navigate]);

  useEffect(() => {
    if (totalPages > 0 && page > totalPages) {
      setPage(totalPages);
    }
  }, [page, totalPages]);

  const getPageNumbers = () => {
    const maxButtons = 5;
    const safeTotalPages = Math.max(totalPages, 1);
    const current = Math.min(page, safeTotalPages);
    const half = Math.floor(maxButtons / 2);

    let start = Math.max(1, current - half);
    let end = Math.min(safeTotalPages, start + maxButtons - 1);

    if (end - start + 1 < maxButtons) {
      start = Math.max(1, end - maxButtons + 1);
    }

    const pages = [];
    for (let i = start; i <= end; i += 1) {
      pages.push(i);
    }
    return pages;
  };

  const startIndex = totalCount === 0 ? 0 : (page - 1) * pageSize + 1;
  const endIndex = Math.min(page * pageSize, totalCount);

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

          <label className="field">
            <span>Filter by status</span>
            <select
              value={selectedStatus}
              onChange={(e) => setSelectedStatus(e.target.value)}
              className="select"
            >
              <option value="">All statuses</option>
              <option value="available">Available</option>
              <option value="borrowed">Borrowed</option>
            </select>
          </label>

          <div className="field filters-actions">
            {(filterTitle || selectedAuthorId || selectedStatus) && (
              <button
                onClick={() => {
                  setFilterTitle("");
                  setSelectedAuthorId("");
                  setSelectedStatus("");
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
      ) : books.length === 0 ? (
        <StatusMessage>No books found.</StatusMessage>
      ) : (
        <section className="card">
          <div className="card-header">
            <h3>All books</h3>
            <span className="badge">{totalCount} items</span>
          </div>
          <ul className="book-list">
            {books.map((book) => {
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
          {totalPages > 1 && (
            <div className="pagination">
              <div className="pagination-info">
                Showing {startIndex}-{endIndex} of {totalCount}
              </div>
              <div className="pagination-controls">
                <button
                  className="btn btn-outline"
                  onClick={() => setPage((current) => Math.max(1, current - 1))}
                  disabled={page === 1}
                >
                  Previous
                </button>
                <div className="pagination-pages">
                  {getPageNumbers().map((pageNumber) => (
                    <button
                      key={pageNumber}
                      type="button"
                      className={`btn btn-outline pagination-page${
                        pageNumber === page ? " is-active" : ""
                      }`}
                      onClick={() => setPage(pageNumber)}
                      aria-current={pageNumber === page ? "page" : undefined}
                    >
                      {pageNumber}
                    </button>
                  ))}
                </div>
                <button
                  className="btn btn-outline"
                  onClick={() =>
                    setPage((current) => Math.min(totalPages, current + 1))
                  }
                  disabled={page === totalPages}
                >
                  Next
                </button>
              </div>
            </div>
          )}
        </section>
      )}
    </div>
  );
}
