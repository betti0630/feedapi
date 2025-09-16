import React, { useEffect, useState } from "react";
import { useAuth } from "../AuthContext";
import axios from "axios";
import FeedList from "../Components/FeedList";

export default function Home() {
  const auth = useAuth();
  const [data, setData] = useState(null);

  useEffect(() => {
    if (auth.token) {
      axios.get(`${process.env.REACT_APP_API_URL}/Feeds?includeExternal=true`, {
        headers: {
          Authorization: `Bearer ${auth.token}`,
        }
      })
      .then((r) => setData(r.data))
      .catch(() => setData("Hiba történt."));
    }
  }, [auth.token]);

  return (
    <div>
      <h2>Home</h2>
      {auth.token ? (
        <>
          <p>Be vagy jelentkezve!</p>
          <button onClick={auth.logout}>Logout</button>
          {data!=null ? <FeedList feeds={data.items}></FeedList> : ""}
          {/* <pre>{JSON.stringify(data, null, 2)}</pre> */}
        </>
      ) : (
        <p>Nem vagy bejelentkezve.</p>
      )}
    </div>
  );
}
