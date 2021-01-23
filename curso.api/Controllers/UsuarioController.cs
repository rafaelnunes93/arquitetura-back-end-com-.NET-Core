using curso.api.Business.Entities;
using curso.api.Business.Repositories;
using curso.api.Configurations;
using curso.api.Filters;
using curso.api.InfraEstruture.Data;
using curso.api.Models;
using curso.api.Models.Usuarios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace curso.api.Controllers
{
    [Route("api/v1/usuario")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {

        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IAutheiticationService _autheitication;


        public UsuarioController(IUsuarioRepository usuarioRepository,
            IAutheiticationService autheitication
            )
        {
            _usuarioRepository = usuarioRepository;
            _autheitication = autheitication;
        }

        [SwaggerResponse(statusCode:200,description:"Sucesso ao autenticar",Type = typeof(LoginViewModelInput))]
        [SwaggerResponse(statusCode:400,description:"Campos Obrigatorios", Type = typeof(ValidaCampoViewModelOutput))]
        [SwaggerResponse(statusCode:500,description:"Erro Interno", Type = typeof(ErroGenericoViewModel))]
        [HttpPost]
        [Route("logar")]
        [ValidacaoModelStateCustomizado]
        public IActionResult Logar(LoginViewModelInput loginViewModelInput )
        {
            var usuario = _usuarioRepository.ObterUsuario(loginViewModelInput.Login);

           if(usuario == null)
            {
                return BadRequest("Houve um erro ao tentar acessar");
            }

            var usuarioViewModelOutput = new UsuarioViewModelOutput()
            {
                Codigo = usuario.Codigo,
                Login = usuario.Login,
                Email = usuario.Email
            };

            var token = _autheitication.GerarToken(usuarioViewModelOutput);

            return Ok(new { 
                Token = token,
                Usuario = usuarioViewModelOutput
            });
        }

        [HttpPost]
        [Route("registrar")]
        [ValidacaoModelStateCustomizado]
        public IActionResult Registrar(RegistroViewModelInput loginViewModelInput)
        {
                       

            //var migracoesPendentes = contexto.Database.GetPendingMigrations();

         

            var usuario = new Usuario();    

            usuario.Login = loginViewModelInput.Login;
            usuario.Senha = loginViewModelInput.Senha;
            usuario.Email = loginViewModelInput.Email;
           
            _usuarioRepository.Adicionar(usuario);
            _usuarioRepository.Commit();

            return Created("", loginViewModelInput);
        }
    }
}
