using CleanArch.Application.ViewModels;
using CleanArch.Domain.Entitidades;
using CleanArch.Domain.Repositories;

namespace CleanArch.Application.Services
{
    public class ProfessorService : IProfessorService
    {
        private readonly IProfessorRepository _professorRepository;

        public ProfessorService(IProfessorRepository professorRepository)
        {
            _professorRepository = professorRepository;
        }
        public async Task<int> Incluir(ProfessorManipulacaoViewModel professorManipulacaoViewModel)
        {
            if (professorManipulacaoViewModel == null)
            {
                throw new ArgumentNullException(nameof(professorManipulacaoViewModel));
            }

            return await IncluirProfessorAsync(professorManipulacaoViewModel);
        }

        public async Task Alterar(Professor professorExiste, ProfessorManipulacaoViewModel professorManipulacaoViewModel)
        {
            if (professorManipulacaoViewModel == null)
            {
                throw new ArgumentNullException(nameof(professorManipulacaoViewModel));
            }
            if (professorExiste == null)
            {
                throw new ArgumentNullException(nameof(professorExiste));
            }

            await AlterarProfessorAsync(professorExiste, professorManipulacaoViewModel);
        }

        public async Task<Professor?> SelecionarPorId(int idProfessor)
        {
            if (idProfessor == null)
            {
                throw new ArgumentNullException(nameof(idProfessor));
            }

            return await _professorRepository.SelecionarAsync(idProfessor);
        }

        public async Task<List<ProfessorViewModel?>?> ListarTodos()
        {
            var professors = await _professorRepository.SelecionarTudoAsync();

            var professorViewModels = professors.Select(a => new ProfessorViewModel
            {
                Id = a.Id,
                Nome = a.Nome,
                Email = a.Email,
            }).ToList();

            return professorViewModels;
        }

        private async Task<int> IncluirProfessorAsync(ProfessorManipulacaoViewModel professorManipulacaoViewModel)
        {
            var professor = new Professor
            {
                Nome = professorManipulacaoViewModel.Nome,
                Email = professorManipulacaoViewModel.Email,
            };

            await _professorRepository.IncluirAsync(professor);
            return professor.Id;
        }

        private async Task AlterarProfessorAsync(Professor professor, ProfessorManipulacaoViewModel professorManipulacaoViewModel)
        {
            professor.Nome = professorManipulacaoViewModel.Nome;
            professor.Email = professorManipulacaoViewModel.Email;

            await _professorRepository.AlterarAsync(professor);
        }
    }
}
