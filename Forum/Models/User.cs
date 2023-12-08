using Forum.Models;
using System.Text.Json.Serialization;

public class User
{
    // User Data
    [JsonIgnore]
    public required int UserId { get; set; }
    public required string Username { get; set; }

    // Security Data
    public bool IsActive { get; set; }
    public string? SecretLey { get; set; }

    // Relationship attributes
    [JsonIgnore]
    public ICollection<Post> Posts { get; set; } = new List<Post>();
    [JsonIgnore]
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();

}