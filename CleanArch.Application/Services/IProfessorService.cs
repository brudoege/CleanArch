using CleanArch.Application.ViewModels;
using CleanArch.Domain.Entitidades;

namespace CleanArch.Application.Services
{
    public interface IProfessorService
    {
        Task<int> Incluir(ProfessorManipulacaoViewModel professorManipulacaoViewModel);
        Task Alterar(Professor professorExiste, ProfessorManipulacaoViewModel professorManipulacaoViewModel);
        Task<Professor?> SelecionarPorId(int idProfessor);
        Task<List<ProfessorViewModel?>?> ListarTodos();
    }
}
