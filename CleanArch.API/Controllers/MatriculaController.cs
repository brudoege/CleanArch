﻿using CleanArch.Application.Services;
using CleanArch.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CleanArch.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatriculaController : ControllerBase
    {
        private readonly IMatriculaService _service;

        public MatriculaController(IMatriculaService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MatriculaViewModel matriculaViewModel)
        {
            if (matriculaViewModel == null)
            {
                return BadRequest("Dados inválidos.");
            }

            try
            {
                var matriculaId = await _service.Salvar(matriculaViewModel);
                return Ok(new { Message = "Matrícula salva com sucesso!", MatriculaId = matriculaId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao salvar a matrícula: {ex.Message}");
            }
        }
    }
}
