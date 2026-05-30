using BuildingBlocks.Contracts;
using MassTransit;
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
        private readonly IPublishEndpoint _publishEndpoint;

        public UsersController(IUserRepository userRepository, IPublishEndpoint publishEndpoint)
        {
            _userRepository = userRepository;
            _publishEndpoint = publishEndpoint;
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

            await _publishEndpoint.Publish(
                new UserCreatedEvent
                {
                    Id = user.Id.Value,
                    Email = user.Email.Value,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                },
                cancellationToken
            );

            return CreatedAtAction(
                nameof(GetUser),
                new { id = user.Id.Value },
                MapToResponse(user)
            );
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UserResponse>> GetUser(
            Guid id,
            CancellationToken cancellationToken
        )
        {
            var userId = UserId.From(id);

            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

            if (user is null)
                return NotFound();

            return Ok(MapToResponse(user));
        }

        private static UserResponse MapToResponse(User user) =>
            new()
            {
                Id = user.Id.Value,
                Email = user.Email.Value,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };
    }
}
