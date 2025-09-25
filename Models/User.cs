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
    
    public ICollection<Recipe> CreatedRecipes { get; set; } = new List<Recipe>();

    [NotMapped]
    public ICollection<Recipe> SavedRecipes { get; set; } = new List<Recipe>();

    [NotMapped]
    public ICollection<Recipe> LikedRecipes { get; set; } = new List<Recipe>();

    [NotMapped]
    public ICollection<Ingredient> ExistingIngredients { get; set; } = new List<Ingredient>();
}