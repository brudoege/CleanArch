using CleanArch.Domain.Entitidades;
using CleanArch.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CleanArch.Infrastructure.Repositorios
{
    public class CursoRepository : BaseRepository<Curso>, ICursoRepository  // BaseRepository implementa o Generics
    {
        public CursoRepository(Contexto contexto) : base(contexto)
        {
        }

        public async Task<Curso> SelecionarComMatriculasAsync(int id)
        {
            return await _contexto.Set<Curso>()
                .Include(c => c.Matriculas) // Carrega as matrículas associadas
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Curso>> SelecionarTudoComProfessorAsync()
        {
            return await _contexto.Set<Curso>()
                .Include(c => c.Professor)
                .ToListAsync();
        }
    }
}