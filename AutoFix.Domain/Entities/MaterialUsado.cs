using AutoFix.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoFix.Domain.Entities
{
    public class MaterialUsado
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "La orden de trabajo es obligatoria")]
        [Display(Name = "Orden de Trabajo")]
        public int OrdenTrabajoId { get; set; }

        [Required(ErrorMessage = "El repuesto es obligatorio")]
        [Display(Name = "Repuesto")]
        public int RepuestoId { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        [Display(Name = "Cantidad")]
        public int Cantidad { get; set; }

        [Display(Name = "Costo Unitario")]
        [DataType(DataType.Currency)]
        public decimal CostoUnitario { get; set; }

        [StringLength(500, ErrorMessage = "Las observaciones no pueden exceder los 500 caracteres")]
        [Display(Name = "Observaciones")]
        public string Observaciones { get; set; }

        [Display(Name = "Fecha de Uso")]
        public DateTime FechaUso { get; set; }

        [Display(Name = "Borrado")]
        public bool Borrado { get; set; }

        // Relaciones
        [ForeignKey("OrdenTrabajoId")]
        public virtual OrdenTrabajo OrdenTrabajo { get; set; }

        [ForeignKey("RepuestoId")]
        public virtual Repuesto Repuesto { get; set; }
    }
}
