using Forum.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Forum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscussionController : ControllerBase
    {
        private dbContext dbcontext;

        public DiscussionController(dbContext context)
        {
            dbcontext = context;
        }

        // GET: api/<PostController>
        [HttpGet]
        [ProducesResponseType(typeof(List<Discussion>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDiscussions()
        {
            // Check if DB is null
            if (dbcontext.Discussions.Count() == 0)
            {
                return NotFound("Database doesn't include any records.");
            }

            return Ok(await dbcontext.Discussions.ToListAsync());
        }

        // GET api/<PostController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Discussion), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDiscussion(int id)
        {
            // Check if DB is null
            if (dbcontext.Discussions.Count() == 0)
            {
                return NotFound("Database doesn't include any records.");
            }

            Discussion? discussion = await dbcontext.Discussions.FindAsync(id);

            // Check if the user is not found
            if (discussion == null)
            {
                return NotFound("User with specified ID not found.");
            }

            return Ok(discussion);
        }

        // POST api/<PostController>
        [HttpPost]
        [ProducesResponseType(typeof(Discussion), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> PostDiscussion([FromBody] Discussion NewDiscussion)
        {
            // Check the input
            if (NewDiscussion == null || !ModelState.IsValid)
            {
                // Return a 400 Bad Request with validation error details
                return await GetErrors();
            }

            // Check if Post is already defined
            var existingPost = await dbcontext.Discussions.SingleOrDefaultAsync(d => d.DiscussionId == NewDiscussion.DiscussionId);
            if (existingPost != null)
            {
                return Conflict("Post with the same ID already exists.");
            }

            // add new cart to database
            dbcontext.Discussions.Add(NewDiscussion);
            await dbcontext.SaveChangesAsync();

            return CreatedAtAction(nameof(PostDiscussion), new { id = NewDiscussion.DiscussionId }, NewDiscussion);
        }

        // DELETE api/<PostController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Discussion), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(int id)
        {
            // Check if DB is not null
            if (dbcontext.Discussions.Count() == 0)
            {
                return NotFound("Database doesn't include any records.");
            }

            // Check if ID exists
            Discussion? DiscussionToRemove = await dbcontext.Discussions.FindAsync(id);
            if (DiscussionToRemove == null)
            {
                return NotFound("Discussion  with specified ID not found.");
            }

            // Remove the Discussion from database
            dbcontext.Discussions.Remove(DiscussionToRemove);
            await dbcontext.SaveChangesAsync();

            return Ok(DiscussionToRemove);
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
