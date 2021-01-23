using curso.api.Business.Entities;
using curso.api.Business.Repositories;
using curso.api.Models.Cursos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace curso.api.Controllers
{

  
    [Route("api/v1/cursos")]
    [ApiController]
    public class CursoController : ControllerBase
     {
        private readonly ICursoRepository _cursoRepository;

        public CursoController(ICursoRepository cursoRepository)
        {
            _cursoRepository = cursoRepository;
        }

        [SwaggerResponse(statusCode: 201, description: "Sucesso ao Cadastrar um curso")]
        [SwaggerResponse(statusCode: 401, description: "Não Autorizado")]
        [HttpPost]
        [Route("")]
        [Authorize]
        public async Task<IActionResult> Post(CursoViewModelInput cursoViewModelInput)
        {

            Curso curso = new Curso();
            curso.Nome = cursoViewModelInput.Nome;
            curso.Desricao = cursoViewModelInput.Descriao;       

            var codigoUsuario = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            curso.Codigo = codigoUsuario;
            _cursoRepository.adicionar(curso);
            _cursoRepository.Commit();
            return Created("", cursoViewModelInput);
        }

        [SwaggerResponse(statusCode: 200, description: "Sucesso ao obter os cursos")]
        [SwaggerResponse(statusCode: 401, description: "Não Autorizado")]
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get()
        {

            var codigoUsuario = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            var cursos = _cursoRepository.ObterPorUsuario(codigoUsuario)
                .Select(s => new CursoViewModelOutput()
                {
                    Nome = s.Nome,
                    Descricao = s.Desricao,
                    Login = s.Usuario.Login
                });


        

            return Ok(cursos);
        }
    }
}
