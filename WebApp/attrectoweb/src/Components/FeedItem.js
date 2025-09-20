import React from "react";
import LikeButton from "./LikeButton.js";

export default function FeedItem({ feed }) {
  if (!feed) return null;

  const {
    $type,
    id,
    imageUrl,
    videoUrl,
    title,
    content,
    authorUserName,
    publishedAt,
    likeCount,
    isOwnFeed,
    isLiked,
  } = feed;

  const date = new Date(publishedAt).toLocaleString();

  return (
    <div className="w-full rounded-2xl shadow-md overflow-hidden bg-white mb-4">
      {imageUrl ? (
        <img
          src={`${process.env.REACT_APP_IMAGE_URL}${imageUrl}`}
          alt={title}
          className="w-full object-contain"
        />
      ) : (
        ""
      )}
      <div className="p-4">
        <h2 className="text-xl font-bold">{title}</h2>
        {/* RSS típus → HTML renderelése */}
        {$type === "rss" ? (
          <div
            className="prose max-w-none text-gray-700"
            dangerouslySetInnerHTML={{ __html: content }}
          />
        ) : (
          <>
            <p className="text-gray-700">{content}</p>
            {videoUrl ? (
              <div>
                <a href={videoUrl}>{videoUrl}</a>
              </div>
            ) : (
              ""
            )}
          </>
        )}
        <div className="flex justify-between items-center mt-3 text-sm text-gray-500">
          <span>By {authorUserName}</span>
          <span>{date}</span>
        </div>
        <div className="flex justify-between items-center mt-3">
          <LikeButton feedId={id} initialLiked={isLiked} initialCount={likeCount} />
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
