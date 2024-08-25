using CleanArch.Application.ViewModels;
using CleanArch.Domain.Entitidades;

namespace CleanArch.Application.Services
{
    public interface ICursoService
    {
        Task<int> Incluir(CursoManipulacaoViewModel cursoManipulacaoViewModel);
        Task Alterar(Curso cursoExiste, CursoManipulacaoViewModel cursoManipulacaoViewModel);
        Task Excluir(Curso cursoExiste);
        Task<Curso?> SelecionarPorId(int idCurso);
        Task<List<CursoViewModel?>?> ListarTodos();
    }
}
