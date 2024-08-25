using CleanArch.Domain.Enums;

namespace CleanArch.Domain.Entitidades
{
    public class Matricula : IEntity
    {
        public int Id { get; set; }
        public int IdAluno { get; set; }
        public Aluno Aluno { get; set; }
        public int IdCurso { get; set; }
        public Curso Curso { get; set; }
        public StatusMatricula StatusMatricula { get; set; }
        public DateTime DataMatricula { get; set; }
    }
}
