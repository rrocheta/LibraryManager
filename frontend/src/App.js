import React, { useState } from "react";
import BooksList from "./BooksList";
import CreateBook from "./CreateBook";

function App() {
  const [showCreate, setShowCreate] = useState(false);

  return (
    <div className="App">
      <h1>Library Manager</h1>
      <button onClick={() => setShowCreate(!showCreate)}>
        {showCreate ? "Back to List" : "Add New Book"}
      </button>
      {showCreate ? <CreateBook onCreated={() => setShowCreate(false)} /> : <BooksList />}
    </div>
  );
}

export default App;
