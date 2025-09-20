import React, { useState } from "react";
import FeedItem from "./FeedItem";

export default function FeedList({ feeds }) {
  const [items, setItems] = useState(feeds || []);

  const handleDelete = (id) => {
    setItems((prev) => prev.filter((f) => f.id !== id));
  };

  if (!items || items.length === 0) {
    return <p className="text-gray-500">No feeds available.</p>;
  }

  return (
    <div className="flex flex-col items-center space-y-6">
      {items.map((feed) => (
        <div className="w-full max-w-5xl" key={feed.id}>
          <FeedItem key={feed.id} feed={feed} onDelete={handleDelete} />
        </div>
      ))}
    </div>
  );
}
