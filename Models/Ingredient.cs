using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace FridgeBackend.Models;

public class Ingredient
{
    [Key]
    public int Id { get; set; }

    [MaxLength(30)]
    public required string Name { get; set; }
    
    public int Quantity { get; set; }

    // FK back to Recipe
    public int RecipeId { get; set; }
    
    [JsonIgnore] // Prevents circular reference Recipe -> Ingredients -> Recipe -> Ingredients...
    public Recipe? Recipe { get; set; } // Made nullable - EF will handle the relationship
}