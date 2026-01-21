import React, { useEffect, useState } from "react";
import { BrowserRouter as Router, Routes, Route, NavLink } from "react-router-dom";
import BorrowBook from "./BorrowBook";
import ReturnBook from "./ReturnBook";
import CreateBook from "./CreateBook";
import EditBook from "./EditBook";
import BooksPage from "./BooksPage";
import AuthorsPage from "./AuthorsPage";
import PublishersPage from "./PublishersPage";
import NotFound from "./NotFound";


function App() {
  const [theme, setTheme] = useState(() => {
    if (typeof window === "undefined") return "light";
    const storedTheme = window.localStorage.getItem("theme");
    if (storedTheme === "dark" || storedTheme === "light") {
      return storedTheme;
    }
    const prefersDark =
      window.matchMedia &&
      window.matchMedia("(prefers-color-scheme: dark)").matches;
    return prefersDark ? "dark" : "light";
  });

  useEffect(() => {
    document.body.classList.toggle("theme-dark", theme === "dark");
    window.localStorage.setItem("theme", theme);
  }, [theme]);

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
          <div className="topbar-actions">
            <nav className="nav">
              <NavLink to="/" end>
                Catalog
              </NavLink>
              <NavLink to="/create">Add Book</NavLink>
              <NavLink to="/borrow">Borrow</NavLink>
              <NavLink to="/return">Return</NavLink>
              <NavLink to="/authors">Authors</NavLink>
              <NavLink to="/publishers">Publishers</NavLink>
            </nav>
            <button
              type="button"
              className={`theme-toggle${theme === "dark" ? " is-dark" : ""}`}
              onClick={() =>
                setTheme((current) => (current === "dark" ? "light" : "dark"))
              }
              aria-pressed={theme === "dark"}
              aria-label="Toggle dark mode"
              title={theme === "dark" ? "Switch to light mode" : "Switch to dark mode"}
            >
              <span className="theme-toggle-track" aria-hidden="true">
                <span className="theme-toggle-icon sun">
                  <svg viewBox="0 0 24 24" aria-hidden="true" focusable="false">
                    <circle cx="12" cy="12" r="4.2" />
                    <g strokeWidth="2" strokeLinecap="round">
                      <line x1="12" y1="2.5" x2="12" y2="5" />
                      <line x1="12" y1="19" x2="12" y2="21.5" />
                      <line x1="2.5" y1="12" x2="5" y2="12" />
                      <line x1="19" y1="12" x2="21.5" y2="12" />
                      <line x1="4.8" y1="4.8" x2="6.6" y2="6.6" />
                      <line x1="17.4" y1="17.4" x2="19.2" y2="19.2" />
                      <line x1="17.4" y1="6.6" x2="19.2" y2="4.8" />
                      <line x1="4.8" y1="19.2" x2="6.6" y2="17.4" />
                    </g>
                  </svg>
                </span>
                <span className="theme-toggle-icon moon">
                  <svg viewBox="0 0 24 24" aria-hidden="true" focusable="false">
                    <path d="M16.9 15.4a7.2 7.2 0 0 1-8.3-8.3 7.5 7.5 0 1 0 8.3 8.3z" />
                  </svg>
                </span>
                <span className="theme-toggle-thumb" />
              </span>
              <span className="sr-only">
                {theme === "dark" ? "Switch to light mode" : "Switch to dark mode"}
              </span>
            </button>
          </div>
        </header>
        <main className="content">
          <Routes>
            <Route path="/" element={<BooksPage />} />
            <Route path="/borrow" element={<BorrowBook />} />
            <Route path="/return" element={<ReturnBook />} />
            <Route path="/create" element={<CreateBook />} />
            <Route path="/edit/:id" element={<EditBook />} />
            <Route path="/authors" element={<AuthorsPage />} />
            <Route path="/publishers" element={<PublishersPage />} />
            <Route path="*" element={<NotFound />} />
          </Routes>
        </main>
      </div>
    </Router>
  );
}

export default App;
