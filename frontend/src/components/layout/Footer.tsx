const GITHUB_URL = "https://github.com/pwiseley/vehicle-maintenance-tracker";
const DOCS_URL = "/swagger";

export function Footer() {
  return (
    <footer className="app-footer">
      <div className="wrap">
        <div className="fbar">
          <span>© 2026 All rights reserved</span>
          <div className="flinks">
            <a href={DOCS_URL} target="_blank" rel="noreferrer">
              Documentation
            </a>
            <a href={GITHUB_URL} target="_blank" rel="noreferrer">
              GitHub
            </a>
          </div>
        </div>
      </div>
    </footer>
  );
}
