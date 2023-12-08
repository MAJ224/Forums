using System.Text.Json.Serialization;

namespace Forum.Models
{
    public class Post
    {
        // Data
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        // Comments
        public ICollection<Comment> Comments { get; set; }

        // Creation Origin
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        public DateOnly CreationDate { get; set; } = new();
        public TimeOnly CreationTime { get; set; } = new();

    }
}
