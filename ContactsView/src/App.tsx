import { useState } from "react";
import { Login } from "./components/Login";

function App() {
  const [token, setToken] = useState(localStorage.getItem("token") || null);
  const [userId, setUserId] = useState(localStorage.getItem("userId") || null);
  const [refreshToken, setRefreshToken] = useState(
    localStorage.getItem("refreshToken") || null
  );
  const [currentView, setCurrentView] = useState("login");

  const handleLogout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("userId");
    localStorage.removeItem("refreshToken");
    setToken(null);
    setUserId(null);
    setRefreshToken(null);
    setCurrentView("login");
  };

  return (
    <main className="min-h-screen bg-gray-100">
      {token ? (
        <div>
          <nav className="bg-blue-600 p-4 text-white flex justify-between">
            <h1 className="text-xl font-bold">Contacts App</h1>
            <div>
              <button
                onClick={() => setCurrentView("contacts")}
                className="mr-4"
              >
                Contacts
              </button>
              <button onClick={handleLogout}>Logout</button>
            </div>
          </nav>
          {/* {currentView === "contacts" && (
            <ContactList
              token={token}
              userId={userId}
              refreshToken={refreshToken}
              setToken={setToken}
            />
          )}
          {currentView === "contactDetails" && (
            <ContactDetails token={token} setCurrentView={setCurrentView} />
          )} */}
        </div>
      ) : (
        <Login
          setToken={setToken}
          setUserId={setUserId}
          setRefreshToken={setRefreshToken}
          setCurrentView={setCurrentView}
        />
      )}
    </main>
  );
}

export default App;
