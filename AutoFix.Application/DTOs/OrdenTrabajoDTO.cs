using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFix.Application.DTOs
{
    public class OrdenTrabajoDTO
    {
        public int Id { get; set; }
        public int CitaSolicitudId { get; set; }
        public int ClienteId { get; set; }
        public string ClienteNombre { get; set; }
        public int MecanicoId { get; set; }
        public string MecanicoNombre { get; set; }
        public string Estado { get; set; }
        public string DescripcionTrabajo { get; set; }
        public string Diagnostico { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaAsignacion { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFinalizacion { get; set; }
        public int Prioridad { get; set; }
        public bool Borrado { get; set; }
    }

    public class CreateOrdenTrabajoDTO
    {
        public int CitaSolicitudId { get; set; }
        public int ClienteId { get; set; }
        public int MecanicoId { get; set; }
        public string DescripcionTrabajo { get; set; }
        public int Prioridad { get; set; }
    }

    public class UpdateOrdenTrabajoDTO
    {
        public int Id { get; set; }
        public string Estado { get; set; }
        public string Diagnostico { get; set; }
        public string Observaciones { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFinalizacion { get; set; }
    }
}