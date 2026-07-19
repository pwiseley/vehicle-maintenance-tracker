import { Outlet } from "react-router-dom";
import { Header } from "./Header";
import { Footer } from "./Footer";

export function Layout() {
  return (
    <>
      <Header />
      <main className="app-main">
        <div className="wrap">
          <Outlet />
        </div>
      </main>
      <Footer />
    </>
  );
}
