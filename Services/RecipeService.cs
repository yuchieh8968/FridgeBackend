using Microsoft.EntityFrameworkCore;

namespace FridgeBackend.Services;

using FridgeBackend.Models;

public class RecipeService
{
    private readonly RecipeContext _context;

    public RecipeService(RecipeContext context)
    {
        _context = context;
    }
    
    public async Task<Comment?> CreateCommentForUserRecipeAsync(int recipeId, int userId, string commentBody)
    {
        var recipe = await _context.Recipes
            .Include(r => r.Comments)
            .FirstOrDefaultAsync(r => r.Id == recipeId);
        if (recipe == null)
        {
            return null;
        }

        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            return null;
        }

        var newComment = new Comment
        {
            Body = commentBody,
            RecipeId = recipeId,
            AuthorUId = userId,
            Recipe = recipe
        };

        // Persist new comment
        _context.Comments.Add(newComment);

        // Keep in-memory navigation in sync (so recipe.Comments contains it immediately)
        recipe.Comments.Add(newComment);

        await _context.SaveChangesAsync();

        return newComment;
    }

    public async Task<Recipe?> CreateRecipeForUserAsync(int userId, string recipeName, string description)
    {
        // 1. Find the user
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            // User not found, so we can't create the recipe
            return null;
        }

        // 2. Create the recipe and link the objects
        var newRecipe = new Recipe
        {
            Name = recipeName,
            Description = description,
            AuthorUId = userId,
            Author = user // Let EF handle the foreign key
        };
        
        // 3. Add to the context and save
        _context.Recipes.Add(newRecipe);
        user.CreatedRecipes.Add(newRecipe);
        await _context.SaveChangesAsync();
        
        // 4. Return the created recipe
        return newRecipe;
    }
    
}