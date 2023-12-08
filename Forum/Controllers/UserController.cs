using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Forum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private dbContext dbcontext;

        public UserController(dbContext context)
        {
            dbcontext = context;
        }

        // GET: api/<UserController>
        [HttpGet]
        [ProducesResponseType(typeof(List<User>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUsers()
        {
            // Check if DB is null
            if (dbcontext.Users.Count() == 0)
            {
                return NotFound("Database doesn't include any records.");
            }

            return Ok(await dbcontext.Users.ToListAsync());
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUser(int id)
        {
            // Check if DB is null
            if (dbcontext.Users.Count() == 0)
            {
                return NotFound("Database doesn't include any records.");
            }

            User? user = await dbcontext.Users.FindAsync(id);

            // Check if the user is not found
            if (user == null)
            {
                return NotFound("User with specified ID not found.");
            }

            return Ok(user);
        }

        // POST api/<UserController>
        [HttpPost]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> PostUser([FromBody] User NewUser)
        {
            // Check the input
            if (NewUser == null || !ModelState.IsValid)
            {
                // Return a 400 Bad Request with validation error details
                return await GetErrors();
            }

            // Check if user is already defined
            var existingUser = await dbcontext.Users.SingleOrDefaultAsync(user => user.UserId == NewUser.UserId);
            if (existingUser != null)
            {
                return Conflict("User with the same ID already exists.");
            }

            // add new user to database
            dbcontext.Users.Add(NewUser);
            await dbcontext.SaveChangesAsync();

            return CreatedAtAction(nameof(PostUser), new { id = NewUser.UserId }, NewUser);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> PutUser(int id, [FromBody] User NewUser)
        {

            // Check if DB is not null
            if (dbcontext.Users.Count() == 0)
            {
                return NotFound("Database doesn't include any records.");
            }

            // Check the input
            if (NewUser == null || !ModelState.IsValid)
            {
                // Return a 400 Bad Request with validation error details
                return await GetErrors();
            }

            // Check if ID exists
            User? user = await dbcontext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound("User with specified ID not found.");
            }

            // match NewUser.UserID with specified ID 
            NewUser.UserId = id;

            // Remove old user
            dbcontext.Users.Remove(user);
            await dbcontext.SaveChangesAsync();

            // Add new User
            dbcontext.Users.Add(NewUser);
            await dbcontext.SaveChangesAsync();

            return Ok(NewUser);
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            // Check if DB is not null
            if (dbcontext.Users.Count() == 0)
            {
                return NotFound("Database doesn't include any records.");
            }

            // Check if ID exists
            User? UserToRemove = await dbcontext.Users.FindAsync(id);
            if (UserToRemove == null)
            {
                return NotFound("User with specified ID not found.");
            }

            // Remove the user from database
            dbcontext.Users.Remove(UserToRemove);
            await dbcontext.SaveChangesAsync();

            return Ok(UserToRemove);
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
