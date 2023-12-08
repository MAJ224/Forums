using Forum.Models;
using System.Text.Json.Serialization;

public class User
{
    // User Data
    public int UserId { get; set; }
    public string Username { get; set; }

    // Security Data
    public bool IsActive { get; set; }
    public string? SecretLey { get; set; }

    // Relation attributes
    [JsonIgnore]
    public ICollection<Post> Posts { get; set; } = new List<Post>();
    [JsonIgnore]
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();

}