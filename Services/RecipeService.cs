namespace FridgeBackend.Services;

using FridgeBackend.Models;

public class RecipeService
{
    private readonly RecipeContext _context;

    // Inject the DbContext using the constructor
    public RecipeService(RecipeContext context)
    {
        _context = context;
    }

    // THIS IS WHERE YOUR "SYNC CODE" / LOGIC GOES
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
            Author = user // Let EF handle the foreign key
        };

        // 3. Add to the context and save
        _context.Recipes.Add(newRecipe);
        await _context.SaveChangesAsync();

        // 4. Return the created recipe
        return newRecipe;
    }
}