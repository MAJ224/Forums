﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
        public async Task<IActionResult> Get()
        {
            // Check if DB is null
            if (dbcontext.Users.Count() == 0)
            {
                return NotFound("Database doesn't include any records.");
            }

            return Ok(await dbcontext.Users.ToArrayAsync());
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(int id)
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
        public async Task<IActionResult> Post([FromBody] User u)
        {
            // Check the input
            if (u == null || !ModelState.IsValid)
            {
                // Return a 400 Bad Request with validation error details
                return await GetErrors();
            }

            // Check if user is already defined
            var existingUser = await dbcontext.Users.SingleOrDefaultAsync(u => u.UserId == u.UserId);
            if (existingUser != null)
            {
                return Conflict("User with the same ID already exists.");
            }

            // add new cart to database
            dbcontext.Users.Add(u);
            await dbcontext.SaveChangesAsync();

            return CreatedAtAction(nameof(u.Username), new { id = u.UserId }, u); ;
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Put(int id, [FromBody] User NewUser)
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
        public async Task<IActionResult> Delete(int id)
        {
            // Check if DB is not null
            if (dbcontext.Users.Count() == 0)
            {
                return NotFound("Database doesn't include any records.");
            }

            // Check if ID exists
            User? user = await dbcontext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound("User with specified ID not found.");
            }

            dbcontext.Users.Remove(user);

            await dbcontext.SaveChangesAsync();
            return Ok(user);
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
