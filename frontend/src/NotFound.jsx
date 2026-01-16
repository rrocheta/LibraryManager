import React from "react";
import { Link } from "react-router-dom";

export default function NotFound() {
  return (
    <div className="page">
      <header className="page-header">
        <div>
          <h2>Page not found</h2>
          <p className="page-subtitle">That route does not exist in the library.</p>
        </div>
      </header>
      <section className="card">
        <p>Use the catalog to keep browsing the collection.</p>
        <div className="form-actions form-actions-spaced">
          <Link to="/" className="btn btn-primary">
            Back to catalog
          </Link>
        </div>
      </section>
    </div>
  );
}
