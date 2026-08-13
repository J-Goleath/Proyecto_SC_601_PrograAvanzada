using AutoFix.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoFix.Domain.Entities
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

        [Required(ErrorMessage = "La descripciÃ³n del problema es obligatoria")]
        [StringLength(500, ErrorMessage = "La descripciÃ³n no puede exceder los 500 caracteres")]
        [Display(Name = "DescripciÃ³n de Fallos")]
        public string DescripcionFallos { get; set; }

        [Display(Name = "Procesada")]
        public bool Procesada { get; set; } = false;

        [Display(Name = "Fecha de Registro")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime FechaRegistro { get; set; }

        public bool Borrado { get; set; } = false;

        [Required(ErrorMessage = "El vehÃ­culo es obligatorio")]
        [Display(Name = "VehÃ­culo")]
        public int VehiculoId { get; set; }

        [ForeignKey("VehiculoId")]
        public virtual Vehiculo Vehiculo { get; set; }

        // MecÃ¡nico asignado a la cita (nullable: se asigna despuÃ©s, no al crearla)
        [Display(Name = "MecÃ¡nico Asignado")]
        public int? MecanicoId { get; set; }

        [ForeignKey("MecanicoId")]
        public virtual Cliente Mecanico { get; set; }

        public CitaSolicitud()
        {
            FechaRegistro = DateTime.Now;
        }
    }
}

