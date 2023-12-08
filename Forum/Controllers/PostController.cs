using Forum.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Forum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private dbContext dbcontext;

        public PostController(dbContext context)
        {
            dbcontext = context;
        }

        // GET: api/<PostController>
        [HttpGet]
        [ProducesResponseType(typeof(List<Post>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            // Check if DB is null
            if (dbcontext.Posts.Count() == 0)
            {
                return NotFound("Database doesn't include any records.");
            }

            return Ok(await dbcontext.Posts.ToListAsync());
        }

        // GET api/<PostController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Post), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(int id)
        {
            // Check if DB is null
            if (dbcontext.Posts.Count() == 0)
            {
                return NotFound("Database doesn't include any records.");
            }

            Post? post = await dbcontext.Posts.FindAsync(id);

            // Check if the user is not found
            if (post == null)
            {
                return NotFound("User with specified ID not found.");
            }

            return Ok(post);
        }

        // POST api/<PostController>
        [HttpPost]
        [ProducesResponseType(typeof(Post), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Post([FromBody] Post NewPost)
        {
            // Check the input
            if (NewPost == null || !ModelState.IsValid)
            {
                // Return a 400 Bad Request with validation error details
                return await GetErrors();
            }

            // Check if Post is already defined
            var existingPost = await dbcontext.Posts.SingleOrDefaultAsync(post => post.PostId == NewPost.PostId);
            if (existingPost != null)
            {
                return Conflict("Post with the same ID already exists.");
            }

            // add new cart to database
            dbcontext.Posts.Add(NewPost);
            await dbcontext.SaveChangesAsync();

            return CreatedAtAction(nameof(Post), new { id = NewPost.PostId }, NewPost);
        }

        // DELETE api/<PostController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Post), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(int id)
        {
            // Check if DB is not null
            if (dbcontext.Posts.Count() == 0)
            {
                return NotFound("Database doesn't include any records.");
            }

            // Check if ID exists
            Post? PostToRemove = await dbcontext.Posts.FindAsync(id);
            if (PostToRemove == null)
            {
                return NotFound("Post with specified ID not found.");
            }

            // Remove the post from database
            dbcontext.Posts.Remove(PostToRemove);
            await dbcontext.SaveChangesAsync();

            return Ok(PostToRemove);
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
