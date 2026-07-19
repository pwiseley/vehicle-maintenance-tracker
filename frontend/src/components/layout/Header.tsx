import { useState } from "react";
import { NavLink } from "react-router-dom";

const GITHUB_URL = "https://github.com/pwiseley/vehicle-maintenance-tracker";
const DOCS_URL = "/swagger";

export function Header() {
  const [mobOpen, setMobOpen] = useState(false);

  const close = () => setMobOpen(false);

  return (
    <header className="app-header">
      <div className="wrap">
        <div className="hbar">
          <div className="brand">
            <div className="logo">
              <i className="bi bi-tools" />
            </div>
            <span className="brand-name">
              <span className="full">Vehicle Maintenance Tracker</span>
              <span className="short">VMT</span>
            </span>
          </div>

          <nav className="desk">
            <NavLink to="/" end>
              Dashboard
            </NavLink>
            <NavLink to="/vehicles">Vehicles</NavLink>
            <a href={DOCS_URL} target="_blank" rel="noreferrer">
              Docs
            </a>
            <a href={GITHUB_URL} target="_blank" rel="noreferrer" aria-label="GitHub">
              <i className="bi bi-github" />
            </a>
          </nav>

          <button className="burger" onClick={() => setMobOpen((v) => !v)} aria-label="Menu">
            <i className="bi bi-list" />
          </button>
        </div>

        {mobOpen && (
          <nav className="mob">
            <NavLink to="/" end onClick={close}>
              <i className="bi bi-speedometer2" />
              Dashboard
            </NavLink>
            <NavLink to="/vehicles" onClick={close}>
              <i className="bi bi-truck" />
              Vehicles
            </NavLink>
            <a href={DOCS_URL} target="_blank" rel="noreferrer" onClick={close}>
              <i className="bi bi-file-earmark-text" />
              Docs
            </a>
            <a href={GITHUB_URL} target="_blank" rel="noreferrer" onClick={close}>
              <i className="bi bi-github" />
              GitHub
            </a>
          </nav>
        )}
      </div>
    </header>
  );
}
