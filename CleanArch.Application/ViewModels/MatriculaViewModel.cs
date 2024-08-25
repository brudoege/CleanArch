using CleanArch.Domain.Enums;

namespace CleanArch.Application.ViewModels
{
    public class MatriculaViewModel
    {
        public int Id { get; set; }
        public int IdAluno { get; set; }
        public string NomeAluno { get; set; }
        public int IdCurso { get; set; }
        public string TituloCurso { get; set; }
        public string StatusMatricula { get; set; }
        public DateTime DataMatricula { get; set; }
    }
}
