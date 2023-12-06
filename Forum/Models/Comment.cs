using Newtonsoft.Json;

namespace Forum.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string Content { get; set; }

        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }

        public int ThreadId { get; set; }
        [JsonIgnore]
        public Thread Thread { get; set; }
    }
}
