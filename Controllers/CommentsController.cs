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
    public class CommentsController : ControllerBase
    {
        private readonly RecipeContext _context;
        private readonly RecipeService _recipeService;

        public CommentsController(RecipeContext context, RecipeService recipeService)
        {
            _context = context;
            _recipeService = recipeService;

        }
        
        // A DTO (Data Transfer Object) for the request body
        public class CreateCommentRequest
        {                
            public int AuthorUId { get; set; }
            public int RecipeId { get; set; } // Changed from string to int

            public required string Body { get; set; }
        }
        
        // GET: api/Comments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
        {
            return await _context.Comments
                .Include(c => c.Author) // Include author information
                .ToListAsync();
        }

        // GET: api/Comments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetComment(int id)
        {
            var comment = await _context.Comments
                .Include(c => c.Author) // Include author information
                .FirstOrDefaultAsync(c => c.Id == id);

            if (comment == null)
            {
                return NotFound();
            }

            return comment;
        }

        // PUT: api/Comments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment(int id, Comment comment)
        {
            if (id != comment.Id)
            {
                return BadRequest();
            }

            _context.Entry(comment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
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

        // POST: api/Comments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostComment(CreateCommentRequest request) // Fixed method name and parameter name
        {
            var createdComment = await _recipeService.CreateCommentForUserRecipeAsync(
                request.RecipeId,    // Fixed parameter order to match service method
                request.AuthorUId,   // Fixed parameter order to match service method
                request.Body
            );
    
            if (createdComment == null)
            {
                return NotFound($"User with ID {request.AuthorUId} or Recipe with ID {request.RecipeId} not found.");
            }
            
            // Return the comment created by the service, not the original parameter
            return CreatedAtAction(nameof(GetComment), new { id = createdComment.Id }, createdComment);
        }

        // DELETE: api/Comments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}