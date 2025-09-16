import React from "react";

export default function FeedItem({ feed }) {
  if (!feed) return null;

  const {
    $type,
    imageUrl,
    videoUrl,
    title,
    content,
    authorUserName,
    publishedAt,
    likeCount,
    isOwnFeed,
  } = feed;

  const date = new Date(publishedAt).toLocaleString();

  return (
    <div className="max-w-md rounded-2xl shadow-md overflow-hidden bg-white mb-4">
      {imageUrl ? <img src={imageUrl} alt={title} className="w-full h-64 object-cover" /> : ""}
      {videoUrl ? <a href={videoUrl}>VIDEO</a> : ""}
      <div className="p-4">
        <h2 className="text-xl font-bold">{title}</h2>
          {/* RSS típus → HTML renderelése */}
        {$type === "rss" ? (
          <div
            className="prose max-w-none text-gray-700"
            dangerouslySetInnerHTML={{ __html: content }}
          />
        ) : (
          <p className="text-gray-700">{content}</p>
        )}
        <div className="flex justify-between items-center mt-3 text-sm text-gray-500">
          <span>By {authorUserName}</span>
          <span>{date}</span>
        </div>
        <div className="flex justify-between items-center mt-3">
          <span className="text-gray-600">❤️ {likeCount}</span>
          {isOwnFeed && (
            <button className="px-3 py-1 text-sm bg-red-500 text-white rounded-xl hover:bg-red-600">
              Delete
            </button>
          )}
        </div>
      </div>
    </div>
  );
}
