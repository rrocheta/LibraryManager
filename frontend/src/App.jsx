import React from "react";
import { BrowserRouter as Router, Routes, Route, Link } from "react-router-dom";
import BorrowBook from "./BorrowBook";
import ReturnBook from "./ReturnBook";
import CreateBook from "./CreateBook";
import EditBook from "./EditBook";
import BooksPage from "./BooksPage";


function App() {
  return (
    <Router>
      <div className="app-shell">
        <header className="topbar">
          <div className="brand">
            <span className="brand-mark">LM</span>
            <div>
              <div className="brand-title">Library Manager</div>
              <div className="brand-subtitle">Online bookstore demo</div>
            </div>
          </div>
          <nav className="nav">
            <Link to="/">Catalog</Link>
            <Link to="/create">Add Book</Link>
            <Link to="/borrow">Borrow</Link>
            <Link to="/return">Return</Link>
          </nav>
        </header>
        <main className="content">
          <Routes>
            <Route path="/" element={<BooksPage />} />
            <Route path="/borrow" element={<BorrowBook />} />
            <Route path="/return" element={<ReturnBook />} />
            <Route path="/create" element={<CreateBook />} />
            <Route path="/edit/:id" element={<EditBook />} />
          </Routes>
        </main>
      </div>
    </Router>
  );
}

export default App;
