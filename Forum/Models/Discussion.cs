﻿using System.Text.Json.Serialization;

namespace Forum.Models
{
    public class Discussion
    {
        // Discussion Data
        public int DiscussionId { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }

        // Comments
        public ICollection<Comment>? Comments { get; set; }

        // Creation Origin
        public required int UserId { get; set; }
        public required DateTime CreationDate { get; set; } = new();

        // Relationship attributes
        [JsonIgnore]
        public User? User { get; set; }

    }
}
