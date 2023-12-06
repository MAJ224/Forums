using Forum.Models;
using System.Text.Json.Serialization;

public class User
{
    public int UserId { get; set; }
    public string Username { get; set; }
    [JsonIgnore]
    public ICollection<Post> Posts { get; set; } = new List<Post>();
    [JsonIgnore]
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();

}