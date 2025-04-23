import { ChangeEvent, useState } from "react";
import { LoginService, RegisterService } from "../services/authService";
import { useNavigate } from "react-router-dom";

const emailRegex = /^[^@\s]+@[^@\s]+\.[^@\s]+$/;
const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$/;

export const Login = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [fieldErrors, setFieldErrors] = useState({ email: "", password: "" });
  const [submitError, setSubmitError] = useState("");
  const navigate = useNavigate();

  const validateEmail = (value: string) => {
    if (!value) return "Email is required.";
    if (!emailRegex.test(value)) return "Invalid email format.";
    return "";
  };

  const validatePassword = (value: string) => {
    if (!value) return "Password is required.";
    if (!passwordRegex.test(value))
      return "Password must be at least 8 characters, include uppercase, lowercase, number, and special character.";
    return "";
  };

  const handleEmailChange = (e: ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value;
    setEmail(value);
    setFieldErrors((prev) => ({ ...prev, email: validateEmail(value) }));
  };

  const handlePasswordChange = (e: ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value;
    setPassword(value);
    setFieldErrors((prev) => ({ ...prev, password: validatePassword(value) }));
  };

  const isFormValid = () => {
    return !validateEmail(email) && !validatePassword(password);
  };

  const handleLogin = async () => {
    const emailError = validateEmail(email);
    const passwordError = validatePassword(password);
    setFieldErrors({ email: emailError, password: passwordError });

    if (emailError || passwordError) return;

    try {
      const response = await LoginService(email, password);
      if (response) {
        navigate("/contacts");
      }
    } catch (err) {
      console.error(err);
      setSubmitError("Invalid credentials");
    }
  };

  const handleRegister = async () => {
    const emailError = validateEmail(email);
    const passwordError = validatePassword(password);
    setFieldErrors({ email: emailError, password: passwordError });

    if (emailError || passwordError) return;

    try {
      const response = await RegisterService(email, password);
      if (response) {
        setSubmitError("Registration successful. You can now log in.");
      }
    } catch (err) {
      console.error(err);
      setSubmitError("Registration failed. Please try again.");
    }
  };

  return (
    <div className="max-w-md mx-auto mt-20 p-6 bg-white rounded-lg shadow-xl">
      <h2 className="text-2xl font-bold mb-4">Login / Register</h2>
      {submitError && <p className="text-red-500 mb-4">{submitError}</p>}
      <div className="mb-4">
        <input
          type="email"
          value={email}
          onChange={handleEmailChange}
          placeholder="Email"
          className="w-full p-2 border rounded"
        />
        {fieldErrors.email && (
          <p className="text-red-500 text-sm">{fieldErrors.email}</p>
        )}
      </div>
      <div className="mb-4">
        <input
          type="password"
          value={password}
          onChange={handlePasswordChange}
          placeholder="Hasło"
          className="w-full p-2 border rounded"
        />
        {fieldErrors.password && (
          <p className="text-red-500 text-sm">{fieldErrors.password}</p>
        )}
      </div>
      <div className="flex justify-between">
        <button
          onClick={handleLogin}
          disabled={!isFormValid()}
          className={`px-4 py-2 rounded ${
            isFormValid()
              ? "bg-blue-600 hover:bg-blue-700 text-white"
              : "bg-gray-300 text-gray-600 cursor-not-allowed"
          }`}
        >
          Login
        </button>
        <button
          onClick={handleRegister}
          disabled={!isFormValid()}
          className={`px-4 py-2 rounded ${
            isFormValid()
              ? "bg-green-600 hover:bg-green-700 text-white"
              : "bg-gray-300 text-gray-600 cursor-not-allowed"
          }`}
        >
          Register
        </button>
      </div>
    </div>
  );
};
