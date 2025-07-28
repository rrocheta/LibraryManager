import React, { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";

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
    let url = "http://localhost:8080/api/books";

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
    fetch("http://localhost:8080/api/authors")
      .then((res) => res.json())
      .then((data) => setAuthors(data));
  };

    const deleteBook = (id) => {
    if (!window.confirm("Are you sure you want to delete this book?")) return;

    fetch(`http://localhost:8080/api/books/${id}`, {
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
    <div>
      <h2>Books</h2>

      {/* Filtros */}
      <div style={{ marginBottom: "1rem" }}>
        <label>
          Filter by Title:
          <input
            type="text"
            value={filterTitle}
            onChange={(e) => setFilterTitle(e.target.value)}
            className="ml-2"
            placeholder="Search title..."
          />
        </label>

        <label style={{ marginLeft: "1rem" }}>
          Filter by Author:
          <select
            value={selectedAuthorId}
            onChange={(e) => setSelectedAuthorId(e.target.value)}
            className="ml-2"
          >
            <option value="">All</option>
            {authors.map((author) => (
              <option key={author.id} value={author.id}>
                {author.name}
              </option>
            ))}
          </select>
        </label>

        {(filterTitle || selectedAuthorId) && (
          <button
            onClick={() => {
              setFilterTitle("");
              setSelectedAuthorId("");
            }}
            className="ml-4 bg-gray-200 px-2 py-1 rounded"
          >
            Clear Filters
          </button>
        )}
      </div>

      {/* Lista de livros */}
      {loading ? (
        <p>Loading books...</p>
      ) : error ? (
        <p>Error: {error}</p>
      ) : filteredBooks.length === 0 ? (
        <p>No books found.</p>
      ) : (
        <>
          <h3 style={{ marginBottom: "1rem" }}>All available books</h3>
          <ul>
            {filteredBooks.map((book) => (
              <li key={book.id}>
                <strong>{book.title}</strong> - Author: {book.author?.name} - Publisher:{" "}
                {book.publisher?.name}{" "}
                <span style={{ marginLeft: "1rem" }}>
                  <button
                    onClick={() => navigate(`/edit/${book.id}`)}
                    className="mr-2 bg-blue-500 text-white px-2 py-1 rounded"
                    style={{ marginRight: "0.5rem" }}
                  >
                    Edit
                  </button>
                  <button
                    onClick={() => deleteBook(book.id)}
                    className="bg-red-500 text-white px-2 py-1 rounded"
                  >
                    Delete
                  </button>
                </span>
              </li>
            ))}
          </ul>
        </>
      )}
    </div>
  );
}
