using System.Text.Json.Serialization;

namespace Forum.Models
{
    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        [JsonIgnore]
        public ICollection<Comment> Comments { get; set; }

        public DateOnly CreationDate { get; set; } = new();
        public TimeOnly CreationTime { get; set; } = new();

    }
}
