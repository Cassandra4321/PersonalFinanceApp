using Microsoft.AspNetCore.Mvc;
using UserService.Core.Interfaces;
using UserService.Core.Models;
using UserService.Core.Services;

namespace UserService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserDomainService _userService;

        public UsersController(UserDomainService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public ActionResult<AppUser> CreateUser([FromBody] AppUser user)
        {
            try
            {
                var createdUser = _userService.CreateUser(user.Email, user.Name);
                return CreatedAtAction(nameof(CreateUser), createdUser);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
