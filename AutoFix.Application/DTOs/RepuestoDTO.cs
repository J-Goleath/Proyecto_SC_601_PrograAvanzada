using System;
using System.ComponentModel.DataAnnotations;

namespace AutoFix.Application.DTOs
{
    public class RepuestoDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public int Stock { get; set; }

        [DataType(DataType.Currency)]
        public decimal Precio { get; set; }

        public string Categoria { get; set; }
        public string Ubicacion { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool Borrado { get; set; }
    }

    public class CreateRepuestoDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        [Display(Name = "Nombre del Repuesto")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El código es obligatorio")]
        [StringLength(50, ErrorMessage = "El código no puede exceder los 50 caracteres")]
        [Display(Name = "Código")]
        public string Codigo { get; set; }

        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El stock es obligatorio")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock debe ser un número positivo")]
        [Display(Name = "Cantidad en Stock")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        [Display(Name = "Precio Unitario")]
        [DataType(DataType.Currency)]
        public decimal Precio { get; set; }

        [StringLength(50, ErrorMessage = "La categoría no puede exceder los 50 caracteres")]
        [Display(Name = "Categoría")]
        public string Categoria { get; set; }

        [StringLength(100, ErrorMessage = "La ubicación no puede exceder los 100 caracteres")]
        [Display(Name = "Ubicación")]
        public string Ubicacion { get; set; }
    }

    public class UpdateRepuestoDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        [Display(Name = "Nombre del Repuesto")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El código es obligatorio")]
        [StringLength(50, ErrorMessage = "El código no puede exceder los 50 caracteres")]
        [Display(Name = "Código")]
        public string Codigo { get; set; }

        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El stock es obligatorio")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock debe ser un número positivo")]
        [Display(Name = "Cantidad en Stock")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        [Display(Name = "Precio Unitario")]
        [DataType(DataType.Currency)]
        public decimal Precio { get; set; }

        [StringLength(50, ErrorMessage = "La categoría no puede exceder los 50 caracteres")]
        [Display(Name = "Categoría")]
        public string Categoria { get; set; }

        [StringLength(100, ErrorMessage = "La ubicación no puede exceder los 100 caracteres")]
        [Display(Name = "Ubicación")]
        public string Ubicacion { get; set; }
    }
}
