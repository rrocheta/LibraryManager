import React from "react";
import { BrowserRouter as Router, Routes, Route, Link } from "react-router-dom";
import BooksList from "./BooksList";
import BorrowBook from "./BorrowBook";
import ReturnBook from "./ReturnBook";
import CreateBook from "./CreateBook";
import ManageBooks from "./ManageBooks";
import EditBook from "./EditBook";

function App() {
  return (
    <Router>
      <nav>
        <Link to="/">Books</Link> |{" "}
        <Link to="/borrow">Borrow Book</Link> |{" "}
        <Link to="/return">Return Book</Link> |{" "}
        <Link to="/create">Create</Link> |{" "}
        <Link to="/manage">Manage</Link>
      </nav>
      <Routes>
        <Route path="/" element={<BooksList />} />
        <Route path="/borrow" element={<BorrowBook />} />
        <Route path="/return" element={<ReturnBook />} />
        <Route path="/create" element={<CreateBook />} />
        <Route path="/manage" element={<ManageBooks />} />
        <Route path="/edit/:id" element={<EditBook />} />
      </Routes>
    </Router>
  );
}

export default App;
