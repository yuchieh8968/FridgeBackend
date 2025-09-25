using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace FridgeBackend.Models;

public class Recipe
{
    [Key]
    public int Id { get; set; }

    [MaxLength(30)]
    public required string Name { get; set; }

    [MaxLength(1000)]
    public string Description { get; set; } = "";

    public int AuthorUId { get; set; }   // FK column

    [ForeignKey(nameof(AuthorUId))]
    [JsonIgnore]
    public User? Author { get; set; }

    public ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    [NotMapped] public ICollection<Ingredient> Allergies { get; set; } = new List<Ingredient>();
    [NotMapped] public ICollection<User> LikedBy { get; set; } = new List<User>();
}