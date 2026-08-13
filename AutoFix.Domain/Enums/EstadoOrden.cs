using System.ComponentModel.DataAnnotations;

namespace AutoFix.Domain.Enums
{
    public enum EstadoOrden
    {
        [Display(Name = "Pendiente")]
        Pendiente = 1,

        [Display(Name = "En Proceso")]
        EnProceso = 2,

        [Display(Name = "Finalizada")]
        Finalizada = 3,

        [Display(Name = "Cancelada")]
        Cancelada = 4
    }
}
