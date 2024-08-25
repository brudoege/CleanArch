using CleanArch.Domain.Entitidades;

namespace CleanArch.Domain.Repositories
{
    public interface IMatriculaRepository : IBaseRepository<Matricula>
    {
        Task<List<Matricula>> SelecionarTudoComAlunoECursoAsync();
    }
}
