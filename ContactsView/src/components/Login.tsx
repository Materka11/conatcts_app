import { useState } from "react";
import { LoginService, RegisterService } from "../services/authService";

interface IProps {
  setCurrentView: (view: string) => void;
}

export const Login = ({ setCurrentView }: IProps) => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");

  const handleLogin = async () => {
    try {
      const response = await LoginService(email, password);
      if (response) {
        setCurrentView("contacts");
      }
    } catch (err) {
      console.error(err);
      setError("Invalid credentials");
    }
  };

  const handleRegister = async () => {
    try {
      const response = await RegisterService(email, password);
      if (response) {
        setError("Registeration successful. You can now log in.");
      }
    } catch (err) {
      console.error(err);
      setError("Registration failed. Please try again.");
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
