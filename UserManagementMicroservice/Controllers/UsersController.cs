using Microsoft.AspNetCore.Mvc;
using UserManagementMicroservice.Models;
using UserManagementMicroservice.Services;

namespace UserManagementMicroservice.Controllers
{
    [ApiController]
    [Route("api/users")] //API route
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        //GET /api/users
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAllUsers()
        {
            return Ok(_userService.GetAllUsers());
        }


        //GET /api/users/{username}
        [HttpGet("{username}")]
        public ActionResult<User> GetUser(string username)
        {
            var user = _userService.GetUserByUsername(username);
            if (user == null)
                return NotFound();
            return Ok(user);
        }


        //PUT /api/users/{username}
        [HttpPut("{username}")]
        public IActionResult UpdateUser(string username, [FromBody] User updatedUser)
        {
            var user = _userService.GetUserByUsername(username);
            if (user == null)
                return NotFound();  // Return 404 if the user is not found

            bool usernameUpdated = false;

            if (!string.IsNullOrEmpty(updatedUser.Username) && updatedUser.Username != username)
            {
                usernameUpdated = true;
            }


            // Update the user information (excluding the username)
            _userService.UpdateUser(updatedUser, username);

            // If the username update was attempted, return a message to the client
            if (usernameUpdated)
            {
                return Ok(new
                {
                    Message = "Remember that the 'username' field will not be updated since it is the identifier of each user (primary key), but the other fields will be updated.",
                    UpdatedUser = _userService.GetUserByUsername(username)
                });
            }

            // If no username update was attempted, return a 204 No Content response
            return NoContent();
        }

    }
}
