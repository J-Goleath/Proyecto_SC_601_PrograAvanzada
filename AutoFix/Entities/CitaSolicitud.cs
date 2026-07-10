using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoFix.Entities
{
    [Table("CitasSolicitud")]
    public class CitaSolicitud
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria")]
        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "La hora es obligatoria")]
        [Display(Name = "Hora")]
        [DataType(DataType.Time)]
        public TimeSpan Hora { get; set; }

        [Required(ErrorMessage = "La descripción del problema es obligatoria")]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres")]
        [Display(Name = "Descripción de Fallos")]
        public string DescripcionFallos { get; set; }

        [Display(Name = "Procesada")]
        public bool Procesada { get; set; } = false;

        [Display(Name = "Fecha de Registro")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime FechaRegistro { get; set; }

        public bool Borrado { get; set; } = false;

        [Required(ErrorMessage = "El vehículo es obligatorio")]
        [Display(Name = "Vehículo")]
        public int VehiculoId { get; set; }

        [ForeignKey("VehiculoId")]
        public virtual Vehiculo Vehiculo { get; set; }

        // Mecánico asignado a la cita (nullable: se asigna después, no al crearla)
        [Display(Name = "Mecánico Asignado")]
        public int? MecanicoId { get; set; }

        [ForeignKey("MecanicoId")]
        public virtual Cliente Mecanico { get; set; }

        public CitaSolicitud()
        {
            FechaRegistro = DateTime.Now;
        }
    }
}
