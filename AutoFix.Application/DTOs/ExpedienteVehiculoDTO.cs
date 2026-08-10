using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFix.Application.DTOs
{
    public class ExpedienteVehiculoDTO
    {
        public VehiculoDTO Vehiculo { get; set; }
        public List<OrdenTrabajoDTO> HistorialReparaciones { get; set; }
        public int TotalReparaciones { get; set; }
        public DateTime? UltimaReparacion { get; set; }
    }

    public class HistorialReparacionDTO
    {
        public int OrdenTrabajoId { get; set; }
        public string Descripcion { get; set; }
        public string Diagnostico { get; set; }
        public string Estado { get; set; }
        public DateTime Fecha { get; set; }
        public string MecanicoNombre { get; set; }
        public List<MaterialUsadoDTO> MaterialesUsados { get; set; }
    }
}