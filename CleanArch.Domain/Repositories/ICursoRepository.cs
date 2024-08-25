using CleanArch.Domain.Entitidades;

namespace CleanArch.Domain.Repositories
{
    public interface ICursoRepository : IBaseRepository<Curso>
    {
        Task<Curso> SelecionarComMatriculasAsync(int id);

        Task<List<Curso>> SelecionarTudoComProfessorAsync();
    }
}
