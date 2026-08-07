using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoFix.Entities
{
    [Table("OrdenesTrabajo")]
    public class OrdenTrabajo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Cita")]
        public int CitaSolicitudId { get; set; }

        [Required]
        [Display(Name = "Cliente")]
        public int ClienteId { get; set; }

        [Required]
        [Display(Name = "Mecánico Asignado")]
        public int MecanicoId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Estado")]
        public string Estado { get; set; }

        [StringLength(500)]
        [Display(Name = "Descripción del Trabajo")]
        public string DescripcionTrabajo { get; set; }

        [StringLength(500)]
        [Display(Name = "Diagnóstico")]
        public string Diagnostico { get; set; }

        [StringLength(500)]
        [Display(Name = "Observaciones del Mecánico")]
        public string Observaciones { get; set; }

        [Display(Name = "Fecha de Asignación")]
        public DateTime FechaAsignacion { get; set; }

        [Display(Name = "Fecha de Inicio")]
        public DateTime? FechaInicio { get; set; }

        [Display(Name = "Fecha de Finalización")]
        public DateTime? FechaFinalizacion { get; set; }

        [Display(Name = "Prioridad")]
        public int Prioridad { get; set; }

        [Display(Name = "Borrado")]
        public bool Borrado { get; set; }

        // Relaciones
        [ForeignKey("CitaSolicitudId")]
        public virtual CitaSolicitud CitaSolicitud { get; set; }

        [ForeignKey("ClienteId")]
        public virtual Cliente Cliente { get; set; }

        [ForeignKey("MecanicoId")]
        public virtual Cliente Mecanico { get; set; }

        public virtual ICollection<MaterialUsado> MaterialesUsados { get; set; }
    }
}