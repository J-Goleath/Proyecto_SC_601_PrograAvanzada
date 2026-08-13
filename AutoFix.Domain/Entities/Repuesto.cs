using AutoFix.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AutoFix.Domain.Entities
{
    public class Repuesto
    {
        [Key]
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

        [Display(Name = "Fecha Registro")]
        public DateTime FechaRegistro { get; set; }

        [Display(Name = "Borrado")]
        public bool Borrado { get; set; }

        // Relaciones
        public virtual ICollection<MaterialUsado> MaterialesUsados { get; set; }
    }
}
