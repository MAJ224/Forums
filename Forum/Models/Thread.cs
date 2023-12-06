using Newtonsoft.Json;

namespace Forum.Models
{
    public class Thread
    {
        public int ThreadId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public DateOnly CreationDate { get; set; } = new();
        public TimeOnly CreationTime { get; set; } = new();


    }
}
