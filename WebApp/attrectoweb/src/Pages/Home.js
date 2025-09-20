import React, { useEffect, useState } from "react";
import { useAuth } from "../AuthContext";
import FeedList from "../Components/FeedList";
import Login from "../Components/Login";

export default function Home() {
  const auth = useAuth();
  const [data, setData] = useState(null);

  useEffect(() => {
    if (auth.token) {
      fetch(`${process.env.REACT_APP_API_URL}/Feeds?includeExternal=true`, {
        headers: {
          Authorization: `Bearer ${auth.token}`,
        },
      })
        .then((r) => {
          if (!r.ok) {
            throw new Error("API error");
          }
          return r.json();
        })
        .then((data) => setData(data))
        .catch(() => setData("An error occured."));
    }
  }, [auth.token]);

  return (
    <div className="relative min-h-screen bg-gray-100">
      {auth.token ? (
        <>
          <button
            onClick={auth.logout}
            className="absolute top-4 right-4 bg-red-500 text-white px-4 py-2 rounded-lg hover:bg-red-600 transition"
          >
            Logout
          </button>
          <div className="flex justify-center pt-16">
            {data != null ? (
              <FeedList feeds={data.items} />
            ) : (
              <p className="text-gray-500">No feeds available</p>
            )}
          </div>
        </>
      ) : (
        <div className="flex items-center justify-center min-h-screen">
          <div className="w-full max-w-sm">
            <Login />
          </div>
        </div>
      )}
    </div>
  );
}
