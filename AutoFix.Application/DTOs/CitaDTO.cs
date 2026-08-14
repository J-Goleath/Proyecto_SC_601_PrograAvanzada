using System;
using System.ComponentModel.DataAnnotations;

namespace AutoFix.Application.DTOs
{
    public class CitaDTO
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public string DescripcionFallos { get; set; }
        public bool Procesada { get; set; }
        public int VehiculoId { get; set; }
        public string VehiculoPlaca { get; set; }
        public int ClienteId { get; set; }
        public string ClienteNombre { get; set; }
        public int? MecanicoId { get; set; }
        public string MecanicoNombre { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool Borrado { get; set; }
    }

    public class CreateCitaDTO
    {
        [Required(ErrorMessage = "El vehículo es obligatorio")]
        [Display(Name = "Vehículo")]
        public int VehiculoId { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "La hora es obligatoria")]
        [Display(Name = "Hora")]
        public TimeSpan Hora { get; set; }

        [Required(ErrorMessage = "Debe describir el problema")]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres")]
        [Display(Name = "Descripción del Problema")]
        public string DescripcionFallos { get; set; }

        public int? MecanicoId { get; set; }
    }

    public class UpdateCitaDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El vehículo es obligatorio")]
        [Display(Name = "Vehículo")]
        public int VehiculoId { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "La hora es obligatoria")]
        [Display(Name = "Hora")]
        public TimeSpan Hora { get; set; }

        [Required(ErrorMessage = "Debe describir el problema")]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres")]
        [Display(Name = "Descripción del Problema")]
        public string DescripcionFallos { get; set; }

        public bool Procesada { get; set; }
        public int? MecanicoId { get; set; }
    }
}
