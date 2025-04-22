import { useState } from "react";
import { Login } from "./components/Login";
import { ContactList } from "./components/ContactList";
import { ContactDetails } from "./components/ContactDetails";

function App() {
  const [currentView, setCurrentView] = useState("login");

  const handleLogout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("userId");
    localStorage.removeItem("refreshToken");
    setCurrentView("login");
  };

  const token = localStorage.getItem("token");

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
          {currentView === "contacts" && <ContactList />}
          {currentView === "contactDetails" && (
            <ContactDetails setCurrentView={setCurrentView} />
          )}
        </div>
      ) : (
        <Login setCurrentView={setCurrentView} />
      )}
    </main>
  );
}

export default App;
