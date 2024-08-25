using CleanArch.Application.ViewModels;
using CleanArch.Domain.Entitidades;

namespace CleanArch.Application.Services
{
    public interface IMatriculaService
    {
        Task<int> Incluir(MatriculaManipulacaoViewModel matriculaManipulacaoViewModel);
        Task Alterar(Matricula matriculaExiste, MatriculaManipulacaoViewModel matriculaManipulacaoViewModel);
        Task Excluir(Matricula matriculaExiste);
        Task<Matricula?> SelecionarPorId(int idMatricula);
        Task<List<MatriculaViewModel?>?> ListarTodos();
    }
}
