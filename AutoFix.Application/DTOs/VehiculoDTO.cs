using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;

namespace AutoFix.Application.DTOs
{
    public class VehiculoDTO
    {
        public int Id { get; set; }

        [Display(Name = "Placa")]
        public string Placa { get; set; }

        [Display(Name = "Marca")]
        public string Marca { get; set; }

        [Display(Name = "Modelo")]
        public string Modelo { get; set; }

        [Display(Name = "Año")]
        public int Anio { get; set; }

        [Display(Name = "Color")]
        public string Color { get; set; }

        [Display(Name = "Cliente")]
        public int ClienteId { get; set; }

        public string ClienteNombre { get; set; }

        [Display(Name = "Fecha de Registro")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime FechaRegistro { get; set; }

        public bool Borrado { get; set; }
    }

    public class CreateVehiculoDTO
    {
        [Required(ErrorMessage = "La placa es obligatoria")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "La placa debe tener entre 4 y 20 caracteres")]
        [Display(Name = "Placa")]
        public string Placa { get; set; }

        [Required(ErrorMessage = "La marca es obligatoria")]
        [StringLength(50, ErrorMessage = "La marca no puede exceder los 50 caracteres")]
        [Display(Name = "Marca")]
        public string Marca { get; set; }

        [Required(ErrorMessage = "El modelo es obligatorio")]
        [StringLength(50, ErrorMessage = "El modelo no puede exceder los 50 caracteres")]
        [Display(Name = "Modelo")]
        public string Modelo { get; set; }

        [Required(ErrorMessage = "El año es obligatorio")]
        [Range(1900, 2100, ErrorMessage = "El año debe estar entre 1900 y 2100")]
        [Display(Name = "Año")]
        public int Anio { get; set; }

        [StringLength(20, ErrorMessage = "El color no puede exceder los 20 caracteres")]
        [Display(Name = "Color")]
        public string Color { get; set; }

        [Required(ErrorMessage = "El cliente es obligatorio")]
        [Display(Name = "Cliente")]
        public int ClienteId { get; set; }
    }

    public class UpdateVehiculoDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La placa es obligatoria")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "La placa debe tener entre 4 y 20 caracteres")]
        [Display(Name = "Placa")]
        public string Placa { get; set; }

        [Required(ErrorMessage = "La marca es obligatoria")]
        [StringLength(50, ErrorMessage = "La marca no puede exceder los 50 caracteres")]
        [Display(Name = "Marca")]
        public string Marca { get; set; }

        [Required(ErrorMessage = "El modelo es obligatorio")]
        [StringLength(50, ErrorMessage = "El modelo no puede exceder los 50 caracteres")]
        [Display(Name = "Modelo")]
        public string Modelo { get; set; }

        [Required(ErrorMessage = "El año es obligatorio")]
        [Range(1900, 2100, ErrorMessage = "El año debe estar entre 1900 y 2100")]
        [Display(Name = "Año")]
        public int Anio { get; set; }

        [StringLength(20, ErrorMessage = "El color no puede exceder los 20 caracteres")]
        [Display(Name = "Color")]
        public string Color { get; set; }

        [Required(ErrorMessage = "El cliente es obligatorio")]
        [Display(Name = "Cliente")]
        public int ClienteId { get; set; }
    }
}