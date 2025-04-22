import { useState } from "react";
import axios from "axios";

interface IProps {
  setToken: (token: string) => void;
  setUserId: (userId: string) => void;
  setRefreshToken: (refreshToken: string) => void;
  setCurrentView: (view: string) => void;
}

export const Login = ({
  setToken,
  setUserId,
  setRefreshToken,
  setCurrentView,
}: IProps) => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");

  const handleLogin = async () => {
    try {
      const response = await axios.post("/api/auth/login", { email, password });
      if (response.data) {
        localStorage.setItem("token", response.data.accessToken);
        localStorage.setItem("refreshToken", response.data.refreshToken);
        localStorage.setItem("userId", response.data.userId);
        setToken(response.data.accessToken);
        setRefreshToken(response.data.refreshToken);
        setUserId(response.data.userId);
        setCurrentView("contacts");
      }
    } catch (err) {
      console.error(err);
      setError("Invalid credentials");
    }
  };

  const handleRegister = async () => {
    try {
      const response = await axios.post("/api/auth/register", {
        email,
        password,
      });
      if (response.data) {
        setError("Rejestracja zakończona sukcesem. Zaloguj się.");
      }
    } catch (err) {
      console.error(err);
      setError("Rejestracja nie powiodła się. Spróbuj ponownie.");
    }
  };

  return (
    <div className="max-w-md mx-auto mt-20 p-6 bg-white rounded-lg shadow-xl">
      <h2 className="text-2xl font-bold mb-4">Login / Register</h2>
      {error && <p className="text-red-500 mb-4">{error}</p>}
      <div className="mb-4">
        <input
          type="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          placeholder="Email"
          className="w-full p-2 border rounded"
        />
      </div>
      <div className="mb-4">
        <input
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          placeholder="Hasło"
          className="w-full p-2 border rounded"
        />
      </div>
      <div className="flex justify-between">
        <button
          onClick={handleLogin}
          className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
        >
          Login
        </button>
        <button
          onClick={handleRegister}
          className="bg-green-600 text-white px-4 py-2 rounded hover:bg-green-700"
        >
          Register
        </button>
      </div>
    </div>
  );
};
