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
            if (username != updatedUser.Username)
                return BadRequest();
            var user = _userService.GetUserByUsername(username);
            if (user == null)
                return NotFound();
            _userService.UpdateUser(updatedUser);
            return NoContent();
        }
    }
}
