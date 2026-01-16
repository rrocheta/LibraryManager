import React from "react";
import { BrowserRouter as Router, Routes, Route, NavLink } from "react-router-dom";
import BorrowBook from "./BorrowBook";
import ReturnBook from "./ReturnBook";
import CreateBook from "./CreateBook";
import EditBook from "./EditBook";
import BooksPage from "./BooksPage";
import NotFound from "./NotFound";


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
            <NavLink to="/" end>
              Catalog
            </NavLink>
            <NavLink to="/create">Add Book</NavLink>
            <NavLink to="/borrow">Borrow</NavLink>
            <NavLink to="/return">Return</NavLink>
          </nav>
        </header>
        <main className="content">
          <Routes>
            <Route path="/" element={<BooksPage />} />
            <Route path="/borrow" element={<BorrowBook />} />
            <Route path="/return" element={<ReturnBook />} />
            <Route path="/create" element={<CreateBook />} />
            <Route path="/edit/:id" element={<EditBook />} />
            <Route path="*" element={<NotFound />} />
          </Routes>
        </main>
      </div>
    </Router>
  );
}

export default App;
