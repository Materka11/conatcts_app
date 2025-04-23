import { Login } from "./components/Login";
import { ContactList } from "./components/ContactList";
import { ContactDetails } from "./components/ContactDetails";
import { Navigate, Route, Routes, useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";

function App() {
  const [token, setToken] = useState(localStorage.getItem("token"));
  const navigate = useNavigate();

  useEffect(() => {
    setToken(localStorage.getItem("token"));
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [localStorage.getItem("token")]);

  const handleLogout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("userId");
    localStorage.removeItem("refreshToken");
    navigate("/login");
  };

  return (
    <main className="min-h-screen bg-gray-100">
      <nav className="bg-blue-600 p-4 text-white flex justify-between">
        <h1 className="text-xl font-bold">Contacts App</h1>
        <div>
          <a href="/contacts" className="mr-4 cursor-pointer">
            Contacts
          </a>
          {token ? (
            <button onClick={handleLogout} className="cursor-pointer">
              Logout
            </button>
          ) : (
            <a href="/login" className="cursor-pointer">
              Login
            </a>
          )}
        </div>
      </nav>
      {token ? (
        <Routes>
          <Route path="/contacts" element={<ContactList token={token} />} />
          <Route path="/contacts/:id" element={<ContactDetails />} />
          <Route path="/" element={<Navigate to="/contacts" />} />
        </Routes>
      ) : (
        <Routes>
          <Route path="/login" element={<Login />} />
          <Route path="/contacts" element={<ContactList token={token} />} />
          <Route path="*" element={<Navigate to="/login" />} />
        </Routes>
      )}
    </main>
  );
}

export default App;
