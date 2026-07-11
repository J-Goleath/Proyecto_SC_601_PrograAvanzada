using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoFix.Entities
{
    [Table("OrdenesTrabajo")]
    public class OrdenTrabajo
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El vehículo es obligatorio")]
        [Display(Name = "Vehículo")]
        public int VehiculoId { get; set; }

        [ForeignKey("VehiculoId")]
        public virtual Vehiculo Vehiculo { get; set; }

        [Required(ErrorMessage = "La descripción del trabajo es obligatoria")]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres")]
        [Display(Name = "Descripción del Trabajo")]
        public string DescripcionTrabajo { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio")]
        [StringLength(30, ErrorMessage = "El estado no puede exceder los 30 caracteres")]
        [Display(Name = "Estado")]
        public string Estado { get; set; }

        [Display(Name = "Fecha de Creación")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime FechaCreacion { get; set; }

        [Display(Name = "Fecha de Finalización")]
        [DataType(DataType.DateTime)]
        public DateTime? FechaFinalizacion { get; set; }

        [StringLength(500, ErrorMessage = "Las observaciones no pueden exceder los 500 caracteres")]
        [Display(Name = "Observaciones")]
        public string Observaciones { get; set; }

        public bool Borrado { get; set; } = false;

        public OrdenTrabajo()
        {
            FechaCreacion = DateTime.Now;
            Estado = "Pendiente";
        }
    }
}