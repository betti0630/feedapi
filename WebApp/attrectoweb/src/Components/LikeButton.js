import { useState } from "react";
import { useAuth } from "../AuthContext";
import { HeartIcon as HeartOutline } from "@heroicons/react/24/outline";
import { HeartIcon as HeartSolid } from "@heroicons/react/24/solid";

export default function LikeButton({
  feedId,
  initialLiked = false,
  initialCount = 0,
}) {
  const auth = useAuth();
  const [isLiked, setIsLiked] = useState(initialLiked);
  const [likeCount, setLikeCount] = useState(initialCount);
  const [loading, setLoading] = useState(false);

  const toggleLike = async () => {
    if (loading) return;
    setLoading(true);

    try {
      if (isLiked) {
        // Unlike → DELETE
        const res = await fetch(
          `${process.env.REACT_APP_API_URL}/feeds/${feedId}/like`,
          { method: "DELETE" ,
            headers: {
              Authorization: `Bearer ${auth.token}`,
            },
          }
        );
        if (res.ok) {
          setIsLiked(false);
          setLikeCount((c) => c - 1);
        }
      } else {
        // Like → POST
        const res = await fetch(
          `${process.env.REACT_APP_API_URL}/feeds/${feedId}/like`,
          { method: "POST" ,
            headers: {
              Authorization: `Bearer ${auth.token}`,
            },
          }
        );
        if (res.ok) {
          setIsLiked(true);
          setLikeCount((c) => c + 1);
        }
      }
    } catch (err) {
      console.error("Like API error:", err);
    } finally {
      setLoading(false);
    }
  };

  return (
    <button
      onClick={toggleLike}
      className="flex items-center space-x-1 text-gray-600 focus:outline-none"
    >
      {isLiked ? (
        <HeartSolid className="w-6 h-6 text-red-500" />
      ) : (
        <HeartOutline className="w-6 h-6 text-gray-400" />
      )}
      <span>{likeCount}</span>
    </button>
  );
}
