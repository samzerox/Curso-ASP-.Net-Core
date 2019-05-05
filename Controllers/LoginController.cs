using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using contactos.Models;
using contactos.Services;

namespace contactos.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private IConfiguration _config;
        private IUserService _userService;

        public LoginController(IConfiguration config, IUserService userService)
        {
            _config = config;
            _userService = userService;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login([FromBody]UserDto login)
        {
            IActionResult response = Unauthorized();
            // Metodo responsable de validar las credenciales del usuario y devolver el modelo Usuario
            // Para demostracion (en este punto)  he usado datos de prueba sin persistencia de Datos
            // Si no retorna un objeto nulo, se procede a generar el JWT
            // Usando el metodo GenerateJSONWebToken
            // var user = AuthenticateUser(login); <- Ya no se usara asi por que se agrego el servicio
            var user = _userService.Authenticate(login.username, login.password);

            if (user != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                response = Ok(new {token = tokenString });
            } 

            return response;

        }

        [Authorize]
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserDto userDto)
        {
            User user = new User {};
            user.username = userDto.username;
            user.email = userDto.email;
            user.fechaCreado = userDto.FechaCreado;

            try
            {
                _userService.Create(user, userDto.password);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }
            
        }

        private string GenerateJSONWebToken(User userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.username),
                new Claim(JwtRegisteredClaimNames.Email, userInfo.email),
                new Claim("FechaCreado", userInfo.fechaCreado.ToString("yyyy-MM-dd")),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Se crea el token utilizando la clase JwtSecurityToken
            // Se le pasa algunos datos como el editor ( issuer), audienca
            // tiempo de expiracion y la firma
            // Se crea Claims y son los que remplazaron el null de aqui abajo
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                // null,
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            // Finalmente el metodo JwtSecurityTokenHandler genera el JWT
            // Este metodo espera un objeto de la clase JwtSecurityToken
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private Usuario AuthenticateUser(Usuario login)
        {
            Usuario user = null;

            // Valida las credenciales del usuario
            if (login.username == "samuel")
            {
                // user = new Usuario { username= "Samuel", password= "123456"};
                user = new Usuario{username = login.username, password = login.password, email = login.email, FechaCreado = login.FechaCreado};
            }
            return user;
        }
    }
}