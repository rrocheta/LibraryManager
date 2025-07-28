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
      <nav>
        | <Link to="/">List and Manage Books</Link> |{" "}
        <Link to="/create">Create Book</Link> |{" "}
        <Link to="/borrow">Borrow Book</Link> |{" "}
        <Link to="/return">Return Book</Link> |{" "}
      </nav>
      <Routes>
        <Route path="/" element={<BooksPage />} />
        <Route path="/borrow" element={<BorrowBook />} />
        <Route path="/return" element={<ReturnBook />} />
        <Route path="/create" element={<CreateBook />} />
        <Route path="/edit/:id" element={<EditBook />} />
      </Routes>
    </Router>
  );
}

export default App;
