
using CleanArch.Application.ViewModels;
using CleanArch.Domain.Entitidades;

namespace CleanArch.Application.Services
{
    public interface IAlunoService
    {
        Task<int> Incluir(AlunoManipulacaoViewModel alunoManipulacaoViewModel);
        Task Alterar(Aluno alunoExiste, AlunoManipulacaoViewModel alunoManipulacaoViewModel);
        Task Excluir(Aluno alunoExiste);
        Task<Aluno?> SelecionarPorId(int idAluno);
        Task<List<AlunoViewModel?>?> ListarTodos();
    }
}
