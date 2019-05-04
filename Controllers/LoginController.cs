using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using contactos.Models;

namespace contactos.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private IConfiguration _config;

        public LoginController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login([FromBody]Usuario login)
        {
            IActionResult response = Unauthorized();
            // Metodo responsable de validar las credenciales del usuario y devolver el modelo Usuario
            // Para demostracion (en este punto)  he usado datos de prueba sin persistencia de Datos
            // Si no retorna un objeto nulo, se procede a generar el JWT
            // Usando el metodo GenerateJSONWebToken
            var user = AuthenticateUser(login);

            if (user != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                response = Ok(new {token = tokenString });
            } 

            return response;

        }

        private string GenerateJSONWebToken(Usuario userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Se crea el token utilizando la clase JwtSecurityToken
            // Se le pasa algunos datos como el editor ( issuer), audienca
            // tiempo de expiracion y la firma
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                null,
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
                user = new Usuario { username= "Samuel", password= "123456"};
            }
            return user;
        }
    }
}