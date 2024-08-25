using CleanArch.Application.Services;
using CleanArch.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CleanArch.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlunoController : ControllerBase
    {
        private readonly IAlunoService _service;

        public AlunoController(IAlunoService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AlunoManipulacaoViewModel alunoManipulacaoViewModel)
        {
            if (alunoManipulacaoViewModel == null)
            {
                return StatusCode(400, new { StatusCode = 400, Message = "Dados inválidos." });
            }

            try
            {
                var alunoId = await _service.Incluir(alunoManipulacaoViewModel);
                return StatusCode(200, new { StatusCode = 200, Message = "Aluno incluido com sucesso!", AlunoId = alunoId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao incluir o aluno: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] AlunoManipulacaoViewModel alunoManipulacaoViewModel)
        {
            if (alunoManipulacaoViewModel == null)
            {
                return StatusCode(400, new { StatusCode = 400, Message = "Dados inválidos." });
            }

            try
            {
                var alunoExiste = await _service.SelecionarPorId(id);

                if (alunoExiste == null)
                {
                    return StatusCode(404, new { StatusCode = 404, Message = "Aluno não encontrado." });
                }

                await _service.Alterar(alunoExiste, alunoManipulacaoViewModel);
                return StatusCode(200, new { StatusCode = 200, Message = "Aluno atualizado com sucesso!", AlunoId = id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Message = $"Erro ao atualizar o aluno: {ex.Message}" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var alunos = await _service.ListarTodos();

                if (alunos == null || !alunos.Any())
                {
                    return StatusCode(404, new { StatusCode = 404, Message = "Nenhum aluno encontrado." });
                }

                return StatusCode(200, new { StatusCode = 200, Message = "Alunos encontrados com sucesso!", Alunos = alunos });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Message = $"Erro ao listar os alunos: {ex.Message}" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var alunoExiste = await _service.SelecionarPorId(id);

                if (alunoExiste == null)
                {
                    return StatusCode(404, new { StatusCode = 404, Message = "Aluno não encontrado." });
                }

                await _service.Excluir(alunoExiste);
                return StatusCode(200, new { StatusCode = 200, Message = "Aluno inativado com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Message = $"Erro ao inativar o aluno: {ex.Message}" });
            }
        }
    }
}
