using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoFix.Entities
{
    [Table("Notificaciones")]
    public class Notificacion
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El mensaje es obligatorio")]
        [StringLength(250, ErrorMessage = "El mensaje no puede exceder los 250 caracteres")]
        [Display(Name = "Mensaje")]
        public string Mensaje { get; set; }

        [Display(Name = "Fecha de Envío")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime FechaEnvio { get; set; }

        [Display(Name = "Leída")]
        public bool Leida { get; set; } = false;

        public bool Borrado { get; set; } = false;

        [Required(ErrorMessage = "El usuario destinatario es obligatorio")]
        [Display(Name = "Usuario")]
        public int ClienteId { get; set; }

        [ForeignKey("ClienteId")]
        public virtual Cliente Cliente { get; set; }

        public Notificacion()
        {
            FechaEnvio = DateTime.Now;
        }
    }
}
