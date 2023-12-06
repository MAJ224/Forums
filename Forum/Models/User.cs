using Forum.Models;
using Newtonsoft.Json;

public class User
{
    public int UserId { get; set; }
    public string Username { get; set; }

    [JsonIgnore]
    public ICollection<Forum.Models.Thread>? Threads { get; set; }
    [JsonIgnore]
    public ICollection<Comment>? Comments { get; set; }
}