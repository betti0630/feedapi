import React from "react";
import FeedItem from "./FeedItem";

export default function FeedList({ feeds }) {
  if (!feeds || feeds.length === 0) {
    return <p className="text-gray-500">No feeds available.</p>;
  }

  return (
    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
      {feeds.map(feed => (
        <FeedItem key={feed.id} feed={feed} />
      ))}
    </div>
  );
}
