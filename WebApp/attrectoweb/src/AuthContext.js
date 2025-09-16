import React, { createContext, useContext, useState } from "react";
import axios from "axios";

const API_URL = process.env.REACT_APP_API_URL;

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [token, setToken] = useState(localStorage.getItem("token"));

  const login = async (username, password) => {
    const response = await axios.post(`${API_URL}/auth/login`, {
      username,
      password,
    });

    const jwt = response.data.access_token;
    localStorage.setItem("token", jwt);
    setToken(jwt);
  };

  const logout = () => {
    localStorage.removeItem("token");
    setToken(null);
  };

  return (
    <AuthContext.Provider value={{ token, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => useContext(AuthContext);
