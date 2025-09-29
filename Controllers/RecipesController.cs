using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FridgeBackend.Models;
using FridgeBackend.Services;

namespace FridgeBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        // Keep both fields
        private readonly RecipeContext _context;
        private readonly RecipeService _recipeService;

        // ✅ Use a SINGLE constructor to inject ALL dependencies
        public RecipesController(RecipeContext context, RecipeService recipeService)
        {
            _context = context;
            _recipeService = recipeService;
        }
        
        // A DTO (Data Transfer Object) for the request body
        public class CreateRecipeRequest
        {
            public int UserId { get; set; }
            public required string Name { get; set; }
            public string Description { get; set; } = "";
        }

        // GET: api/Recipes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipes()
        {
            // Note: In a full refactor, this logic would also move to the RecipeService
            return await _context.Recipes
                .Include(r => r.Comments)
                .ThenInclude(c => c.Author)
                .ToListAsync();
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Recipe>> GetRecipe(int id)
        {
            var recipe = await _context.Recipes
                .Include(r => r.Comments)
                .ThenInclude(c => c.Author)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (recipe == null)
            {
                return NotFound();
            }

            return recipe;
        }

        // PUT: api/Recipes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecipe(int id, Recipe recipe)
        {
            if (id != recipe.Id)
            {
                return BadRequest();
            }

            _context.Entry(recipe).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateRecipe(CreateRecipeRequest request)
        {
            var createdRecipe = await _recipeService.CreateRecipeForUserAsync(
                request.UserId, 
                request.Name, 
                request.Description
            );

            if (createdRecipe == null)
            {
                return NotFound($"User with ID {request.UserId} not found.");
            }
            
            return CreatedAtAction(nameof(GetRecipe), new { id = createdRecipe.Id }, createdRecipe);
        }
        
        // DELETE: api/Recipes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RecipeExists(int id)
        {
            return _context.Recipes.Any(e => e.Id == id);
        }
    }
}