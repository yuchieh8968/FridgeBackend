using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace FridgeBackend.Models;

public class Comment
{
    [Key]
    public int Id { get; set; }

    [MaxLength(300)]
    public required string Body { get; set; }

    public int AuthorUId { get; set; } // Foreign key

    [ForeignKey(nameof(AuthorUId))]
    public User? Author { get; set; } // Navigation property to the User

    // Remove [JsonIgnore] from RecipeId - we need it for deserialization
    public int RecipeId { get; set; }

    [JsonIgnore] // Only ignore the navigation property to prevent circular references
    public Recipe? Recipe { get; set; } // Made nullable - EF will handle the relationship
}