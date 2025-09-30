using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace FridgeBackend.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    [MaxLength(30)]
    public required string Name { get; set; }

    [MaxLength(30)]
    public required string Email { get; set; }
    
    [InverseProperty(nameof(Recipe.Author))]
    public ICollection<Recipe> CreatedRecipes { get; set; } = new List<Recipe>();

    [NotMapped]
    [JsonIgnore] // Also ignore these since they're not mapped
    public ICollection<Recipe> SavedRecipes { get; set; } = new List<Recipe>();

    [NotMapped]
    [JsonIgnore]
    public ICollection<Recipe> LikedRecipes { get; set; } = new List<Recipe>();

    [NotMapped]
    [JsonIgnore]
    public ICollection<Ingredient> ExistingIngredients { get; set; } = new List<Ingredient>();
}