using Forum.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Forum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private dbContext dbcontext;

        public CommentController(dbContext context)
        {
            dbcontext = context;
        }

        // GET: api/<CommentController>
        [HttpGet]
        [ProducesResponseType(typeof(List<Comment>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetComments()
        {
            // Check if DB is null
            if (dbcontext.Comments.Count() == 0)
            {
                return NotFound("Database doesn't include any records.");
            }

            return Ok(await dbcontext.Comments.ToListAsync());
        }

        // GET api/<CommentController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Comment), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetComment(int id)
        {
            // Check if DB is null
            if (dbcontext.Comments.Count() == 0)
            {
                return NotFound("Database doesn't include any records.");
            }

            Comment? comment = await dbcontext.Comments.FindAsync(id);

            // Check if the user is not found
            if (comment == null)
            {
                return NotFound("User with specified ID not found.");
            }

            return Ok(comment);
        }

        // POST api/<CommentController>
        [HttpPost]
        [ProducesResponseType(typeof(Comment), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> PostComment([FromBody] Comment NewComment)
        {
            // Check the input
            if (NewComment == null || !ModelState.IsValid)
            {
                // Return a 400 Bad Request with validation error details
                return await GetErrors();
            }

            // Check if Comment is already defined
            var existingComment = await dbcontext.Comments.SingleOrDefaultAsync(comment => comment.CommentId == NewComment.CommentId);
            if (existingComment != null)
            {
                return Conflict("Post with the same ID already exists.");
            }

            // add new cart to database
            dbcontext.Comments.Add(NewComment);
            await dbcontext.SaveChangesAsync();

            return CreatedAtAction(nameof(PostComment), new { id = NewComment.CommentId }, NewComment);
        }
    

        // PUT api/<CommentController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> PutComment(int id, [FromBody] Comment NewComment)
        {

            // Check if DB is not null
            if (dbcontext.Comments.Count() == 0)
            {
                return NotFound("Database doesn't include any records.");
            }

            // Check the input
            if (NewComment == null || !ModelState.IsValid)
            {
                // Return a 400 Bad Request with validation error details
                return await GetErrors();
            }

            // Check if ID exists
            Comment? comment = await dbcontext.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound("User with specified ID not found.");
            }

            // match NewComment.CommentID with specified ID 
            NewComment.CommentId = id;

            // Remove old comment
            dbcontext.Comments.Remove(comment);
            await dbcontext.SaveChangesAsync();

            // Add new comment
            dbcontext.Comments.Add(NewComment);
            await dbcontext.SaveChangesAsync();

            return Ok(NewComment);
        }

        // DELETE api/<CommentController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Comment), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteComment(int id)
        {
            // Check if DB is not null
            if (dbcontext.Comments.Count() == 0)
            {
                return NotFound("Database doesn't include any records.");
            }

            // Check if ID exists
            Comment? CommentToRemove = await dbcontext.Comments.FindAsync(id);
            if (CommentToRemove == null)
            {
                return NotFound("Comment with specified ID not found.");
            }

            // Remove the Comment from database
            dbcontext.Comments.Remove(CommentToRemove);
            await dbcontext.SaveChangesAsync();

            return Ok(CommentToRemove);
        }

        private async Task<BadRequestObjectResult> GetErrors()
        {
            var validationErrors = ModelState
                    .Where(e => e.Value.Errors.Any())
                    .ToDictionary(
                        key => key.Key,
                        value => value.Value.Errors.Select(error => error.ErrorMessage).ToArray()
                    );

            return BadRequest(validationErrors);
        }
    }
}
