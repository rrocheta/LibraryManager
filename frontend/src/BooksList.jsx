import React, { useEffect, useState } from "react";

export default function BooksList() {
  const [books, setBooks] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    fetch("http://localhost:8080/api/books")
      .then((res) => {
        if (!res.ok) {
          throw new Error("Erro ao buscar livros");
        }
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
  }, []);

  if (loading) return <div>Carregando livros...</div>;
  if (error) return <div>Erro: {error}</div>;

  if (books.length === 0) return <div>Nenhum livro encontrado.</div>;

  return (
    <div>
      <h2>Lista de Livros</h2>
      <ul>
        {books.map((book) => (
          <li key={book.id}>
            <strong>{book.title}</strong> — Autor: {book.authorName} — Editora: {book.publisherName}
          </li>
        ))}
      </ul>
    </div>
  );
}
