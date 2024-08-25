using CleanArch.Application.ViewModels;
using CleanArch.Domain.Entitidades;
using CleanArch.Domain.Repositories;

namespace CleanArch.Application.Services
{
    public class AlunoService : IAlunoService
    {
        private readonly IAlunoRepository _alunoRepository;

        public AlunoService(IAlunoRepository alunoRepository)
        {
            _alunoRepository = alunoRepository;
        }

        public async Task<int> Incluir(AlunoManipulacaoViewModel alunoManipulacaoViewModel)
        {
            if (alunoManipulacaoViewModel == null)
            {
                throw new ArgumentNullException(nameof(alunoManipulacaoViewModel));
            }

            return await IncluirAlunoAsync(alunoManipulacaoViewModel);
        }

        public async Task Alterar(Aluno alunoExiste, AlunoManipulacaoViewModel alunoManipulacaoViewModel)
        {
            if (alunoManipulacaoViewModel == null)
            {
                throw new ArgumentNullException(nameof(alunoManipulacaoViewModel));
            }
            if (alunoExiste == null)
            {
                throw new ArgumentNullException(nameof(alunoExiste));
            }

            if (!alunoExiste.Ativo)
            {
                throw new ArgumentException("O aluno informado está inativo.");
            }

            await AlterarAlunoAsync(alunoExiste, alunoManipulacaoViewModel);
        }

        public async Task Excluir(Aluno alunoExiste)
        {
            if (alunoExiste == null)
            {
                throw new ArgumentNullException(nameof(alunoExiste));
            }

            if (!alunoExiste.Ativo)
            {
                throw new ArgumentException("O aluno informado já está inativo.");
            }

            var alunoManipulacaoViewModel = new AlunoManipulacaoViewModel
            {
                Nome = alunoExiste.Nome,
                Email = alunoExiste.Email,
                Endereco = alunoExiste.Endereco,
            };

            alunoExiste.Ativo = false;
            await AlterarAlunoAsync(alunoExiste, alunoManipulacaoViewModel);
        }

        public async Task<Aluno?> SelecionarPorId(int idAluno)
        {
            if (idAluno == null)
            {
                throw new ArgumentNullException(nameof(idAluno));
            }

            return await _alunoRepository.SelecionarAsync(idAluno);
        }

        public async Task<List<AlunoViewModel?>?> ListarTodos()
        {
            var alunos = await _alunoRepository.SelecionarTudoAsync();

            var alunoViewModels = alunos.Select(a => new AlunoViewModel
            {
                Id = a.Id,
                Nome = a.Nome,
                Email = a.Email,
                Endereco = a.Endereco,
                Ativo = a.Ativo
            }).ToList();

            return alunoViewModels;
        }

        private async Task<int> IncluirAlunoAsync(AlunoManipulacaoViewModel alunoManipulacaoViewModel)
        {
            var aluno = new Aluno
            {
                Nome = alunoManipulacaoViewModel.Nome,
                Endereco = alunoManipulacaoViewModel.Endereco,
                Email = alunoManipulacaoViewModel.Email,
                Ativo = true
            };

            await _alunoRepository.IncluirAsync(aluno);
            return aluno.Id;
        }

        private async Task AlterarAlunoAsync(Aluno aluno, AlunoManipulacaoViewModel alunoManipulacaoViewModel)
        {
            aluno.Nome = alunoManipulacaoViewModel.Nome;
            aluno.Endereco = alunoManipulacaoViewModel.Endereco;
            aluno.Email = alunoManipulacaoViewModel.Email;

            await _alunoRepository.AlterarAsync(aluno);
        }
    }
}
