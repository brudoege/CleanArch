using CleanArch.Application.Services;
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
        public async Task<IActionResult> Post([FromBody] MatriculaManipulacaoViewModel matriculaManipulacaoViewModel)
        {
            if (matriculaManipulacaoViewModel == null)
            {
                return StatusCode(400, new { StatusCode = 400, Message = "Dados inválidos." });
            }

            try
            {
                var matriculaId = await _service.Incluir(matriculaManipulacaoViewModel);
                return StatusCode(200, new { StatusCode = 200, Message = "Matricula incluida com sucesso!", MatriculaId = matriculaId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao incluir a matricula: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] MatriculaManipulacaoViewModel matriculaManipulacaoViewModel)
        {
            if (matriculaManipulacaoViewModel == null)
            {
                return StatusCode(400, new { StatusCode = 400, Message = "Dados inválidos." });
            }

            try
            {
                var matriculaExiste = await _service.SelecionarPorId(id);

                if (matriculaExiste == null)
                {
                    return StatusCode(404, new { StatusCode = 404, Message = "Matricula não encontrada." });
                }

                await _service.Alterar(matriculaExiste, matriculaManipulacaoViewModel);
                return StatusCode(200, new { StatusCode = 200, Message = "Matricula atualizada com sucesso!", MatriculaId = id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Message = $"Erro ao atualizar a matricula: {ex.Message}" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var matriculas = await _service.ListarTodos();

                if (matriculas == null || !matriculas.Any())
                {
                    return StatusCode(404, new { StatusCode = 404, Message = "Nenhum matricula encontrada." });
                }

                return StatusCode(200, new { StatusCode = 200, Message = "Matriculas encontradas com sucesso!", Matriculas = matriculas });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Message = $"Erro ao listar as matriculas: {ex.Message}" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var matriculaExiste = await _service.SelecionarPorId(id);

                if (matriculaExiste == null)
                {
                    return StatusCode(404, new { StatusCode = 404, Message = "Matricula não encontrada." });
                }

                await _service.Excluir(matriculaExiste);
                return StatusCode(200, new { StatusCode = 200, Message = "Matricula cancelada com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Message = $"Erro ao cancelar a matricula: {ex.Message}" });
            }
        }
    }
}
