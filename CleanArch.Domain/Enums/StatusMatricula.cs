using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CleanArch.Domain.Enums
{
    public enum StatusMatricula
    {
        [Description("Matrícula Ativa")]
        Ativa,

        [Description("Matrícula Concluída")]
        Concluida,

        [Description("Matrícula Cancelada")]
        Cancelada
    }
}
