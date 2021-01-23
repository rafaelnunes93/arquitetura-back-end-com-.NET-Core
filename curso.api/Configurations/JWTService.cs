using curso.api.Models.Usuarios;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace curso.api.Configurations
{
    public class JWTService : IAutheiticationService
    {
        private readonly IConfiguration _configuration;

        public JWTService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GerarToken(UsuarioViewModelOutput usuarioViewModelOutput)
        {
            var secret = Encoding.ASCII.GetBytes(_configuration.GetSection("MinhaChaveSuperSecreta").Value);
            var symmetricSecurityKey = new SymmetricSecurityKey(secret);
            var securitytokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, usuarioViewModelOutput.Codigo.ToString()),
                    new Claim(ClaimTypes.Name, usuarioViewModelOutput.Login.ToString()),
                    new Claim(ClaimTypes.Email, usuarioViewModelOutput.Email.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var jwtSecuritytokenHandler = new JwtSecurityTokenHandler();
            var tokenGenerated = jwtSecuritytokenHandler.CreateToken(securitytokenDescriptor);
            var token = jwtSecuritytokenHandler.WriteToken(tokenGenerated);

            return token;
        }
    }
}
