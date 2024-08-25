using CleanArch.Application.ViewModels;
using CleanArch.Domain.Entitidades;
using CleanArch.Domain.Repositories;

namespace CleanArch.Application.Services
{
    public class CursoService : ICursoService
    {
        private readonly ICursoRepository _cursoRepository;
        private readonly IProfessorRepository _professorRepository;

        public CursoService(ICursoRepository cursoRepository, IProfessorRepository professorRepository)
        {
            _cursoRepository = cursoRepository;
            _professorRepository = professorRepository;
        }

        public async Task<int> Incluir(CursoManipulacaoViewModel cursoManipulacaoViewModel)
        {
            if (cursoManipulacaoViewModel == null)
            {
                throw new ArgumentNullException(nameof(cursoManipulacaoViewModel));
            }

            var professorExiste = await _professorRepository.SelecionarAsync(cursoManipulacaoViewModel.IdProfessor);
            if (professorExiste == null)
            {
                throw new ArgumentException("O professor informado não existe.");
            }

            return await IncluirCursoAsync(cursoManipulacaoViewModel);
        }

        public async Task Alterar(Curso cursoExiste, CursoManipulacaoViewModel cursoManipulacaoViewModel)
        {
            if (cursoManipulacaoViewModel == null)
            {
                throw new ArgumentNullException(nameof(cursoManipulacaoViewModel));
            }
            if (cursoExiste == null)
            {
                throw new ArgumentNullException(nameof(cursoExiste));
            }

            if (!cursoExiste.Ativo)
            {
                throw new ArgumentException("O curso informado está inativo.");
            }

            var professorExiste = await _professorRepository.SelecionarAsync(cursoManipulacaoViewModel.IdProfessor);
            if (professorExiste == null)
            {
                throw new ArgumentException("O professor informado não existe.");
            }

            await AlterarCursoAsync(cursoExiste, cursoManipulacaoViewModel);
        }

        public async Task Excluir(Curso cursoExiste)
        {
            if (cursoExiste == null)
            {
                throw new ArgumentNullException(nameof(cursoExiste));
            }

            if (!cursoExiste.Ativo)
            {
                throw new ArgumentException("O curso informado já está inativo.");
            }

            var cursoManipulacaoViewModel = new CursoManipulacaoViewModel
            {
                Titulo = cursoExiste.Titulo,
                Descricao = cursoExiste.Descricao,
                DataInicio = cursoExiste.DataInicio,
                IdProfessor = cursoExiste.IdProfessor
            };

            cursoExiste.Ativo = false;
            await AlterarCursoAsync(cursoExiste, cursoManipulacaoViewModel);
        }

        public async Task<Curso?> SelecionarPorId(int idCurso)
        {
            if (idCurso == null)
            {
                throw new ArgumentNullException(nameof(idCurso));
            }

            return await _cursoRepository.SelecionarAsync(idCurso);
        }

        public async Task<List<CursoViewModel?>?> ListarTodos()
        {
            var cursos = await _cursoRepository.SelecionarTudoComProfessorAsync();

            var cursoViewModels = cursos.Select(a => new CursoViewModel
            {
                Id = a.Id,
                Titulo = a.Titulo,
                Descricao = a.Descricao,
                DataInicio = a.DataInicio,
                IdProfessor = a.IdProfessor,
                ProfessorNome = a.Professor?.Nome,
                Ativo = a.Ativo
            }).ToList();

            return cursoViewModels;
        }

        private async Task<int> IncluirCursoAsync(CursoManipulacaoViewModel cursoManipulacaoViewModel)
        {
            var curso = new Curso
            {
                Titulo = cursoManipulacaoViewModel.Titulo,
                Descricao = cursoManipulacaoViewModel.Descricao,
                DataInicio = cursoManipulacaoViewModel.DataInicio,
                IdProfessor = cursoManipulacaoViewModel.IdProfessor,
                Ativo = true
            };

            await _cursoRepository.IncluirAsync(curso);
            return curso.Id;
        }

        private async Task AlterarCursoAsync(Curso curso, CursoManipulacaoViewModel cursoManipulacaoViewModel)
        {
            curso.Titulo = cursoManipulacaoViewModel.Titulo;
            curso.Descricao = cursoManipulacaoViewModel.Descricao;
            curso.DataInicio = cursoManipulacaoViewModel.DataInicio;
            curso.IdProfessor = cursoManipulacaoViewModel.IdProfessor;

            await _cursoRepository.AlterarAsync(curso);
        }
    }
}
