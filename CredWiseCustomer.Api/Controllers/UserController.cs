using CredWiseCustomer.Application.DTOs;
using CredWiseCustomer.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using Microsoft.Extensions.Options;
using Authentication.Service.Interfaces;

namespace CredWiseCustomer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IValidator<CreateUserDto> _createUserValidator;
        private readonly IValidator<UpdateUserDto> _updateUserValidator;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IAuthenticationService _authService;

        public UserController(
            IUserService userService,
            IValidator<CreateUserDto> createUserValidator,
            IValidator<UpdateUserDto> updateUserValidator,
            IPasswordHasher passwordHasher,
            IAuthenticationService authService)
        {
            _userService = userService;
            _createUserValidator = createUserValidator;
            _updateUserValidator = updateUserValidator;
            _passwordHasher = passwordHasher;
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("decode")]
        public IActionResult DecodeToken([FromBody] string token)
        {
            try
            {
                // Remove "Bearer " prefix if present
                if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    token = token.Substring("Bearer ".Length).Trim();
                }

                var handler = new JwtSecurityTokenHandler();

                if (!handler.CanReadToken(token))
                {
                    return BadRequest("Token is not in a valid JWT format");
                }

                var jwtToken = handler.ReadJwtToken(token);

                return Ok(new
                {
                    Header = jwtToken.Header,
                    Payload = jwtToken.Payload,
                    Claims = jwtToken.Claims.Select(c => new { c.Type, c.Value }),
                    RoleClaim = jwtToken.Claims.FirstOrDefault(c =>
                        c.Type == "role" || c.Type == ClaimTypes.Role)?.Value
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Token decoding failed: {ex.Message}");
            }
        }
       
        [Authorize(Policy = "Admin")]
        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            // Validate the input
            var validationResult = await _createUserValidator.ValidateAsync(createUserDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }
            // Set CreatedBy to 'Admin'
            createUserDto.CreatedBy = "Admin";
            var user = await _userService.CreateUserAsync(createUserDto);
            return CreatedAtAction(nameof(GetUserById), new { id = user.UserId }, user);
        }

        [HttpGet("{id:int}")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound($"User with ID {id} not found.");
            }
            return Ok(user);
        }

        [HttpGet("email/{email}")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<UserDto>> GetUserByEmail(string email)
        {
            var user = await _userService.GetByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"User with email {email} not found.");
            }
            return Ok(user);
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpPut("{id:int}")]
        [Authorize(Policy = "AdminOrCustomer")]
        public async Task<ActionResult<UserDto>> UpdateUser(int id, [FromBody] UpdateUserDto updateUserDto)
        {
            // Validate the input
            var validationResult = await _updateUserValidator.ValidateAsync(updateUserDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }

            var updatedUser = await _userService.UpdateUserAsync(id, updateUserDto);
            if (updatedUser == null)
            {
                return NotFound($"User with ID {id} not found.");
            }
            return Ok(updatedUser);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult> SoftDeleteUser(int id)
        {
            var result = await _userService.SoftDeleteUserAsync(id);
            if (!result)
                return NotFound($"User with ID {id} not found.");

            return NoContent();
        }

        [HttpPost("{id:int}/restore")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult> RestoreUser(int id)
        {
            var result = await _userService.RestoreUserAsync(id);
            if (!result)
                return NotFound($"User with ID {id} not found.");

            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDto dto)
        {
            // Set CreatedBy to 'Customer'
            dto.CreatedBy = "Customer";
            var validationResult = await _createUserValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }
            var user = await _userService.CreateUserAsync(dto);
            return CreatedAtAction(nameof(GetUserById), new { id = user.UserId }, user);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
        {
            // Fetch user by email
            UserDto user;
            try
            {
                user = await _userService.GetByEmailAsync(dto.Email);
            }
            catch
            {
                return Unauthorized("Invalid credentials.");
            }

            // Fetch the actual user entity for password hash
            var userEntity = await _userService.GetUserEntityByEmailAsync(dto.Email);
            if (userEntity == null)
                return Unauthorized("Invalid credentials.");

            // Verify password
            if (!_passwordHasher.VerifyPassword(dto.Password, userEntity.Password))
                return Unauthorized("Invalid credentials.");

            // Generate JWT using third-party service
            var claims = new Dictionary<string, object>
            {
                { "unique_name", user.Email },
                { "nameid", user.UserId.ToString() },
                { "role", user.Role ?? "Customer" }
            };

            var token = await _authService.GenerateTokenAsync(claims);
            
            // Debug logging
            Console.WriteLine($"Generated token for user {user.Email} with role {user.Role}");
            Console.WriteLine($"Token: {token}");
            
            // Verify the token was generated correctly
            var validationResult = await _authService.ValidateTokenAsync(token);
            if (!validationResult.IsValid)
            {
                Console.WriteLine($"Token validation failed: {validationResult.Error}");
                return StatusCode(500, "Failed to generate valid token");
            }
            
            Console.WriteLine($"Token validation successful. Claims: {string.Join(", ", validationResult.Claims.Select(c => $"{c.Key}: {c.Value}"))}");
            
            return Ok(new { token, user });
        }

        [Authorize(Policy = "Admin")]
        [HttpGet("admin-only")]
        public IActionResult AdminOnly()
        {
            return Ok("This endpoint is for Admins only.");
        }

        [Authorize(Policy = "AdminOrCustomer")]
        [HttpGet("admin-or-customer")]
        public IActionResult AdminOrCustomer()
        {
            return Ok("This endpoint is for Admin or Customer.");
        }
    }
}