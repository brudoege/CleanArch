using CleanArch.Application.Services;
using CleanArch.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CleanArch.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessorController : ControllerBase
    {
        private readonly IProfessorService _service;

        public ProfessorController(IProfessorService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProfessorManipulacaoViewModel professorManipulacaoViewModel)
        {
            if (professorManipulacaoViewModel == null)
            {
                return StatusCode(400, new { StatusCode = 400, Message = "Dados inválidos." });
            }

            try
            {
                var professorId = await _service.Incluir(professorManipulacaoViewModel);
                return StatusCode(200, new { StatusCode = 200, Message = "Professor incluido com sucesso!", ProfessorId = professorId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao incluir o professor: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ProfessorManipulacaoViewModel professorManipulacaoViewModel)
        {
            if (professorManipulacaoViewModel == null)
            {
                return StatusCode(400, new { StatusCode = 400, Message = "Dados inválidos." });
            }

            try
            {
                var professorExiste = await _service.SelecionarPorId(id);

                if (professorExiste == null)
                {
                    return StatusCode(404, new { StatusCode = 404, Message = "Professor não encontrado." });
                }

                await _service.Alterar(professorExiste, professorManipulacaoViewModel);
                return StatusCode(200, new { StatusCode = 200, Message = "Professor atualizado com sucesso!", ProfessorId = id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Message = $"Erro ao atualizar o professor: {ex.Message}" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var professors = await _service.ListarTodos();

                if (professors == null || !professors.Any())
                {
                    return StatusCode(404, new { StatusCode = 404, Message = "Nenhum professor encontrado." });
                }

                return StatusCode(200, new { StatusCode = 200, Message = "Professors encontrados com sucesso!", Professors = professors });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Message = $"Erro ao listar os professors: {ex.Message}" });
            }
        }
    }
}