using BookstoreMS.Models.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        {
            var command = new SqlCommand(@"
                SELECT u.UserId, u.Username, r.RoleName
                FROM AppUser u
                JOIN AppRole r ON u.RoleId = r.RoleId
                WHERE u.Username = @Username AND u.Password = @Password", connection);

            command.Parameters.AddWithValue("@Username", request.Username);
            command.Parameters.AddWithValue("@Password", request.Password);

            connection.Open();
            var reader = command.ExecuteReader();

            if (reader.Read())
            {
                var response = new LoginResponse
                {
                    UserId = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    Role = reader.GetString(2)
                };

                return Ok(response);
            }

            return Unauthorized("Invalid username or password");
        }
    }
}