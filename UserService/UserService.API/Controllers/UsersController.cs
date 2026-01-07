using Microsoft.AspNetCore.Mvc;
using UserService.Contracts.Users;
using UserService.Core.Abstractions;
using UserService.Core.Users;
using DomainUser = UserService.Core.Users.User;

namespace UserService.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public sealed class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<UserResponse>> CreateUser(
            CreateUserRequest request,
            CancellationToken cancellationToken
        )
        {
            if (string.IsNullOrWhiteSpace(request.Email))
                return BadRequest("Email is required.");

            if (string.IsNullOrWhiteSpace(request.FirstName))
                return BadRequest("FirstName is required.");

            if (string.IsNullOrWhiteSpace(request.LastName))
                return BadRequest("LastName is required.");

            var email = Email.From(request.Email);

            if (await _userRepository.EmailExistsAsync(email, cancellationToken))
                return Conflict("A user with this email already exists.");

            var user = DomainUser.Create(email, request.FirstName, request.LastName);

            await _userRepository.AddAsync(user, cancellationToken);

            var response = new UserResponse
            {
                Id = user.Id.Value,
                Email = user.Email.Value,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };

            return CreatedAtAction(nameof(CreateUser), response);
        }
    }
}
