import React from "react";
import { BrowserRouter as Router, Routes, Route, Link } from "react-router-dom";
import BooksList from "./BooksList";
import BorrowBook from "./BorrowBook";
import ReturnBook from "./ReturnBook";
import CreateBook from "./CreateBook"; // ✅ IMPORTADO

function App() {
  return (
    <Router>
      <nav>
        <Link to="/">Books</Link> | 
        <Link to="/borrow">Borrow Book</Link> | 
        <Link to="/return">Return Book</Link> | 
        <Link to="/create">Create Book</Link> {/* ✅ NOVO LINK */}
      </nav>
      <Routes>
        <Route path="/" element={<BooksList />} />
        <Route path="/borrow" element={<BorrowBook />} />
        <Route path="/return" element={<ReturnBook />} />
        <Route path="/create" element={<CreateBook />} /> {/* ✅ NOVA ROTA */}
      </Routes>
    </Router>
  );
}

export default App;
