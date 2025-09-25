using Microsoft.EntityFrameworkCore;

namespace FridgeBackend.Models;

public class RecipeContext : DbContext
{
    public RecipeContext(DbContextOptions<RecipeContext> options)
        : base(options)
    {
    }
    
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<Comment> Comments { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Recipe -> Author relationship
        modelBuilder.Entity<Recipe>()
            .HasOne(r => r.Author)
            .WithMany(u => u.CreatedRecipes)
            .HasForeignKey(r => r.AuthorUId);
            
        // Recipe -> Ingredients relationship
        modelBuilder.Entity<Ingredient>()
            .HasOne(i => i.Recipe)
            .WithMany(r => r.Ingredients)
            .HasForeignKey(i => i.RecipeId);
            
        // Recipe -> Comments relationship
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Recipe)
            .WithMany(r => r.Comments)
            .HasForeignKey(c => c.RecipeId);
    }
}