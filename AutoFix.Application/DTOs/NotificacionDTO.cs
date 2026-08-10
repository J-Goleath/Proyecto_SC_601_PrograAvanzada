using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFix.Application.DTOs
{
    public class NotificacionDTO
    {
        public int Id { get; set; }
        public string Mensaje { get; set; }
        public DateTime FechaEnvio { get; set; }
        public bool Leida { get; set; }
        public int ClienteId { get; set; }
        public string ClienteNombre { get; set; }
        public bool Borrado { get; set; }
    }

    public class CreateNotificacionDTO
    {
        public string Mensaje { get; set; }
        public int ClienteId { get; set; }
    }

    public class UpdateNotificacionDTO
    {
        public int Id { get; set; }
        public bool Leida { get; set; }
    }
}