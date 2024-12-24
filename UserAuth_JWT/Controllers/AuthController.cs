using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserAuth_JWT.Data;
using UserAuth_JWT.DTOs;
using UserAuth_JWT.Models;
using UserAuth_JWT.Services;

namespace UserAuth_JWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;

        public AuthController(AppDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(AuthRequest request)
        {
            if(await _context.Users.AnyAsync(u => u.Username == request.Username))
            {
                return BadRequest("User already exists!");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            Console.WriteLine(hashedPassword);

            var user = new User
            {
                Username = request.Username,
                Password = hashedPassword
            };

            


            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok("User registered successfully!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthRequest request)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == request.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return Unauthorized("Invalid username or password!");
            }

            var token = _tokenService.GenerateToken(user);

            return Ok(new { Token = token });

        }
    }
}
