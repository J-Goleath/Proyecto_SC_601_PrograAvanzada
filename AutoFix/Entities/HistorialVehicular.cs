using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoFix.Entities
{
    [Table("HistorialVehicular")]
    public class HistorialVehicular
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El vehículo es obligatorio")]
        [Display(Name = "Vehículo")]
        public int VehiculoId { get; set; }

        [ForeignKey("VehiculoId")]
        public virtual Vehiculo Vehiculo { get; set; }

        [Display(Name = "Orden de Trabajo")]
        public int? OrdenTrabajoId { get; set; }

        [ForeignKey("OrdenTrabajoId")]
        public virtual OrdenTrabajo OrdenTrabajo { get; set; }

        [Required(ErrorMessage = "La descripción del servicio es obligatoria")]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres")]
        [Display(Name = "Descripción del Servicio")]
        public string DescripcionServicio { get; set; }

        [Display(Name = "Fecha del Servicio")]
        [DataType(DataType.DateTime)]
        public DateTime FechaServicio { get; set; }

        [StringLength(500, ErrorMessage = "Las observaciones no pueden exceder los 500 caracteres")]
        [Display(Name = "Observaciones")]
        public string Observaciones { get; set; }

        public bool Borrado { get; set; } = false;

        public HistorialVehicular()
        {
            FechaServicio = DateTime.Now;
        }
    }
}