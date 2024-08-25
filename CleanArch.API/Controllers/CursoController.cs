using CleanArch.Application.Services;
using CleanArch.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CleanArch.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CursoController : ControllerBase
    {
        private readonly ICursoService _service;

        public CursoController(ICursoService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CursoManipulacaoViewModel cursoManipulacaoViewModel)
        {
            if (cursoManipulacaoViewModel == null)
            {
                return StatusCode(400, new { StatusCode = 400, Message = "Dados inválidos." });
            }

            try
            {
                var cursoId = await _service.Incluir(cursoManipulacaoViewModel);
                return StatusCode(200, new { StatusCode = 200, Message = "Curso incluido com sucesso!", CursoId = cursoId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao incluir o curso: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] CursoManipulacaoViewModel cursoManipulacaoViewModel)
        {
            if (cursoManipulacaoViewModel == null)
            {
                return StatusCode(400, new { StatusCode = 400, Message = "Dados inválidos." });
            }

            try
            {
                var cursoExiste = await _service.SelecionarPorId(id);

                if (cursoExiste == null)
                {
                    return StatusCode(404, new { StatusCode = 404, Message = "Curso não encontrado." });
                }

                await _service.Alterar(cursoExiste, cursoManipulacaoViewModel);
                return StatusCode(200, new { StatusCode = 200, Message = "Curso atualizado com sucesso!", CursoId = id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Message = $"Erro ao atualizar o curso: {ex.Message}" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var cursos = await _service.ListarTodos();

                if (cursos == null || !cursos.Any())
                {
                    return StatusCode(404, new { StatusCode = 404, Message = "Nenhum curso encontrado." });
                }

                return StatusCode(200, new { StatusCode = 200, Message = "Cursos encontrados com sucesso!", Cursos = cursos });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Message = $"Erro ao listar os cursos: {ex.Message}" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var cursoExiste = await _service.SelecionarPorId(id);

                if (cursoExiste == null)
                {
                    return StatusCode(404, new { StatusCode = 404, Message = "Curso não encontrado." });
                }

                await _service.Excluir(cursoExiste);
                return StatusCode(200, new { StatusCode = 200, Message = "Curso inativado com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Message = $"Erro ao inativar o curso: {ex.Message}" });
            }
        }
    }
}

