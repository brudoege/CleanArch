using CleanArch.Application.ViewModels;
using CleanArch.Domain.Entitidades;
using CleanArch.Domain.Enums;
using CleanArch.Domain.Repositories;

namespace CleanArch.Application.Services
{
    public class MatriculaService : IMatriculaService
    {
        private readonly IMatriculaRepository _matriculaRepository;
        private readonly IAlunoRepository _alunoRepository;
        private readonly ICursoRepository _cursoRepository;

        public MatriculaService(IMatriculaRepository matriculaRepository, IAlunoRepository alunoRepository, ICursoRepository cursoRepository)
        {
            _matriculaRepository = matriculaRepository;
            _alunoRepository = alunoRepository;
            _cursoRepository = cursoRepository;
        }

        public async Task<int> Incluir(MatriculaManipulacaoViewModel matriculaManipulacaoViewModel)
        {
            if (matriculaManipulacaoViewModel == null)
            {
                throw new ArgumentNullException(nameof(matriculaManipulacaoViewModel));
            }

            await ValidarAlunoAsync(matriculaManipulacaoViewModel);

            await ValidarCursoAsync(matriculaManipulacaoViewModel, null);

            return await IncluirMatriculaAsync(matriculaManipulacaoViewModel);
        }

        private async Task ValidarCursoAsync(MatriculaManipulacaoViewModel matriculaManipulacaoViewModel, int? id)
        {
            var cursoExiste = await _cursoRepository.SelecionarComMatriculasAsync(matriculaManipulacaoViewModel.IdCurso);
            if (cursoExiste == null)
            {
                throw new ArgumentException("O curso informado não existe.");
            }
            if (!cursoExiste.Ativo)
            {
                throw new ArgumentException("O curso informado não está ativo.");
            }
            if (cursoExiste.DataInicio.Date <= DateTime.UtcNow.Date)
            {
                throw new ArgumentException("O curso informado já foi iniciado, matrícula não permitida.");
            }

            if (cursoExiste.Matriculas is not null)
            {
                if (id is null)
                {
                    if (cursoExiste.Matriculas.Count(m => m.StatusMatricula == StatusMatricula.Ativa) >= 3)
                    {
                        throw new InvalidOperationException("O matricula informado já atingiu o limite máximo de 30 matrículas ativas.");
                    }

                    if (cursoExiste.Matriculas.Exists(m => m.IdAluno == matriculaManipulacaoViewModel.IdAluno && m.StatusMatricula == StatusMatricula.Ativa))
                    {
                        throw new InvalidOperationException("O aluno já está matriculado no curso com uma matrícula ativa.");
                    }
                }
                else
                {
                    if (cursoExiste.Matriculas.Count(m => m.StatusMatricula == StatusMatricula.Ativa && m.Id != id) >= 30)
                    {
                        throw new InvalidOperationException("O matricula informado já atingiu o limite máximo de 30 matrículas ativas.");
                    }

                    if (cursoExiste.Matriculas.Exists(m => m.IdAluno == matriculaManipulacaoViewModel.IdAluno && m.StatusMatricula == StatusMatricula.Ativa && m.Id != id))
                    {
                        throw new InvalidOperationException("O aluno já está matriculado no curso com uma matrícula ativa.");
                    }
                }
            }
        }

        private async Task ValidarAlunoAsync(MatriculaManipulacaoViewModel matriculaManipulacaoViewModel)
        {
            var alunoExiste = await _alunoRepository.SelecionarAsync(matriculaManipulacaoViewModel.IdAluno);
            if (alunoExiste == null)
            {
                throw new ArgumentException("O aluno informado não existe.");
            }
            if (!alunoExiste.Ativo)
            {
                throw new ArgumentException("O aluno informado não está ativo.");
            }
        }

        public async Task Alterar(Matricula matriculaExiste, MatriculaManipulacaoViewModel matriculaManipulacaoViewModel)
        {
            if (matriculaManipulacaoViewModel == null)
            {
                throw new ArgumentNullException(nameof(matriculaManipulacaoViewModel));
            }
            if (matriculaExiste == null)
            {
                throw new ArgumentNullException(nameof(matriculaExiste));
            }

            if (matriculaExiste.StatusMatricula == StatusMatricula.Cancelada)
            {
                throw new ArgumentException("A matricula informada está cancelada.");
            }

            await ValidarAlunoAsync(matriculaManipulacaoViewModel);

            await ValidarCursoAsync(matriculaManipulacaoViewModel, matriculaExiste.Id);

            await AlterarMatriculaAsync(matriculaExiste, matriculaManipulacaoViewModel);
        }

        public async Task Excluir(Matricula matriculaExiste)
        {
            if (matriculaExiste == null)
            {
                throw new ArgumentNullException(nameof(matriculaExiste));
            }

            if (matriculaExiste.StatusMatricula == StatusMatricula.Cancelada)
            {
                throw new ArgumentException("A matricula informada já está cancelada.");
            }

            var matriculaManipulacaoViewModel = new MatriculaManipulacaoViewModel
            {
                IdAluno = matriculaExiste.IdAluno,
                IdCurso = matriculaExiste.IdCurso,
            };

            matriculaExiste.StatusMatricula = StatusMatricula.Cancelada;
            await AlterarMatriculaAsync(matriculaExiste, matriculaManipulacaoViewModel);
        }

        public async Task<Matricula?> SelecionarPorId(int idMatricula)
        {
            if (idMatricula == null)
            {
                throw new ArgumentNullException(nameof(idMatricula));
            }

            return await _matriculaRepository.SelecionarAsync(idMatricula);
        }

        public async Task<List<MatriculaViewModel?>?> ListarTodos()
        {
            var matriculas = await _matriculaRepository.SelecionarTudoComAlunoECursoAsync();

            var matriculaViewModels = matriculas.Select(a => new MatriculaViewModel
            {
                Id = a.Id,
                IdAluno = a.IdAluno,
                NomeAluno = a.Aluno.Nome,
                IdCurso = a.IdCurso,
                TituloCurso = a.Curso.Titulo,
                StatusMatricula = a.StatusMatricula.GetEnumDescription(),
                DataMatricula = a.DataMatricula
            }).ToList();

            return matriculaViewModels;
        }

        private async Task<int> IncluirMatriculaAsync(MatriculaManipulacaoViewModel matriculaManipulacaoViewModel)
        {
            var matricula = new Matricula
            {
                IdAluno = matriculaManipulacaoViewModel.IdAluno,
                IdCurso = matriculaManipulacaoViewModel.IdCurso,
                StatusMatricula = StatusMatricula.Ativa,
                DataMatricula = DateTime.UtcNow
            };

            await _matriculaRepository.IncluirAsync(matricula);
            return matricula.Id;
        }

        private async Task AlterarMatriculaAsync(Matricula matricula, MatriculaManipulacaoViewModel matriculaManipulacaoViewModel)
        {
            matricula.IdAluno = matriculaManipulacaoViewModel.IdAluno;
            matricula.IdCurso = matriculaManipulacaoViewModel.IdCurso;

            await _matriculaRepository.AlterarAsync(matricula);
        }
    }
}
