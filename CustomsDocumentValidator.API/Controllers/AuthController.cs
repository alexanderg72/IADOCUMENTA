using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace CustomsDocumentValidator.API.Controllers;

public class LoginRequest
{
    public string NombreUsuario { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class RegisterRequest
{
    public string NombreUsuario { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
}

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly string _connectionString;

    public AuthController(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection") ?? "";
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var query = "SELECT Rol, Activo FROM Usuarios WHERE NombreUsuario = @user AND Password = @pass";
        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@user", request.NombreUsuario);
        command.Parameters.AddWithValue("@pass", request.Password);

        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            bool activo = reader.GetBoolean(reader.GetOrdinal("Activo"));
            if (!activo)
                return Unauthorized(new { success = false, mensaje = "El usuario está desactivado" });

            string rol = reader.GetString(reader.GetOrdinal("Rol"));

            return Ok(new { success = true, rol = rol, mensaje = "Login exitoso" });
        }

        return Unauthorized(new { success = false, mensaje = "Usuario o contraseña incorrectos" });
    }

    [HttpGet("usuarios")]
    public IActionResult GetUsuarios()
    {
        var usuarios = new List<object>();
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var query = "SELECT Id, NombreUsuario, Rol, Activo FROM Usuarios";
        using var command = new SqlCommand(query, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            usuarios.Add(new
            {
                Id = reader.GetInt32(0),
                NombreUsuario = reader.GetString(1),
                Rol = reader.GetString(2),
                Activo = reader.GetBoolean(3)
            });
        }
        return Ok(usuarios);
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterRequest request)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var query = "INSERT INTO Usuarios (NombreUsuario, Password, Rol, Activo) VALUES (@user, @pass, @rol, 1)";
        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@user", request.NombreUsuario);
        command.Parameters.AddWithValue("@pass", request.Password);
        command.Parameters.AddWithValue("@rol", request.Rol);

        try
        {
            command.ExecuteNonQuery();
            return Ok(new { success = true });
        }
        catch
        {
            // Si llega aquí, es porque el UNIQUE en SQL falló (el usuario ya existe)
            return BadRequest(new { success = false, mensaje = "El usuario ya existe." });
        }
    }
}