using System.Text.Json.Serialization;

namespace Forum.Models
{
    public class Comment
    {
        public int CommentId { get; set; }

        public int PostId { get; set; }
        [JsonIgnore]
        public Post Post { get; set; }

        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }

        public string Content { get; set; }

    }
}
