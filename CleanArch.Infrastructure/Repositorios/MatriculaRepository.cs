using CleanArch.Domain.Entitidades;
using CleanArch.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CleanArch.Infrastructure.Repositorios
{
    public class MatriculaRepository : BaseRepository<Matricula>, IMatriculaRepository
    {
        public MatriculaRepository(Contexto contexto) : base(contexto)
        {                
        }

        public async Task<List<Matricula>> SelecionarTudoComAlunoECursoAsync()
        {
            return await _contexto.Set<Matricula>()
                .Include(m => m.Aluno)
                .Include(m => m.Curso)
                .ToListAsync();
        }
    }
}