using System.Text.Json.Serialization;

namespace Forum.Models
{
    public class Comment
    {
        // Comment Data
        public required string Content { get; set; }
        public int CommentId { get; set; }

        // Creation Origin
        public required int UserId { get; set; }
        public required int DiscussionId { get; set; }

        // Relationship attributes
        [JsonIgnore]
        public User? User { get; set; }
        [JsonIgnore]
        public Discussion? discussion { get; set; }

    }
}
