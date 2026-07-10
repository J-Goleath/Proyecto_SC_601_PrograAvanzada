using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoFix.Entities
{
    [Table("Repuestos")]
    public class Repuesto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del repuesto es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [StringLength(300, ErrorMessage = "La descripción no puede exceder los 300 caracteres")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Range(0, 9999, ErrorMessage = "La cantidad debe ser mayor o igual a 0")]
        [Display(Name = "Cantidad Disponible")]
        public int CantidadDisponible { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0, 9999999, ErrorMessage = "El precio debe ser mayor o igual a 0")]
        [Display(Name = "Precio Unitario")]
        public decimal PrecioUnitario { get; set; }

        [Display(Name = "Fecha de Registro")]
        [DataType(DataType.DateTime)]
        public DateTime FechaRegistro { get; set; }

        public bool Borrado { get; set; } = false;

        public Repuesto()
        {
            FechaRegistro = DateTime.Now;
        }
    }
}