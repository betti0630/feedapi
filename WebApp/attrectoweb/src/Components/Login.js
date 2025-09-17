import React, { useState } from "react";
import { useAuth } from "../AuthContext";
import { useNavigate } from "react-router-dom";

export default function Login() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState(null);
  const auth = useAuth();
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await auth.login(username, password);
      navigate("/");
    } catch {
      setError("Wrong login!");
    }
  };

  return (
<div className="flex items-center justify-center min-h-screen bg-gray-100">
  <form
    onSubmit={handleSubmit}
    className="bg-white shadow-md rounded-2xl p-8 w-full max-w-sm space-y-4"
  >

    <input
      value={username}
      onChange={(e) => setUsername(e.target.value)}
      placeholder="Username"
      className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-400"
    />

    <input
      type="password"
      value={password}
      onChange={(e) => setPassword(e.target.value)}
      placeholder="Password"
      className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-400"
    />

    <button
      type="submit"
      className="w-full bg-red-500 text-white py-2 rounded-lg hover:bg-red-600 transition"
    >
      Login
    </button>

    {error && (
      <p className="bg-red-100 text-red-600 text-sm p-2 rounded-lg text-center">
        {error}
      </p>
    )}
  </form>
</div>

  );
}
