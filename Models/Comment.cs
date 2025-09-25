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

    public DateTime Date { get; set; }

    // FK back to Recipe
    public int RecipeId { get; set; }
    
    [JsonIgnore] // Prevents circular reference Recipe -> Comments -> Recipe -> Comments...
    public Recipe? Recipe { get; set; } // Made nullable - EF will handle the relationship
}