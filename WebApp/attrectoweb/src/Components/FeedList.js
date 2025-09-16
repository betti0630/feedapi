import React from "react";
import FeedItem from "./FeedItem";

export default function FeedList({ feeds }) {
  if (!feeds || feeds.length === 0) {
    return <p className="text-gray-500">No feeds available.</p>;
  }

  return (
    <div className="flex flex-col items-center space-y-6">
      {feeds.map(feed => (
        <div className="w-full max-w-5xl" key={feed.id}>
          <FeedItem key={feed.id} feed={feed} />
        </div>
      ))}
    </div>
  );
}
