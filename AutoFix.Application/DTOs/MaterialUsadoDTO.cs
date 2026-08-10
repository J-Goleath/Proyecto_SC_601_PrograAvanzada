using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFix.Application.DTOs
{
    public class MaterialUsadoDTO
    {
        public int Id { get; set; }
        public int OrdenTrabajoId { get; set; }
        public int RepuestoId { get; set; }
        public string RepuestoNombre { get; set; }
        public string RepuestoCodigo { get; set; }
        public int Cantidad { get; set; }
        public decimal CostoUnitario { get; set; }
        public decimal Total => Cantidad * CostoUnitario;
        public string Observaciones { get; set; }
        public DateTime FechaUso { get; set; }
        public bool Borrado { get; set; }
    }

    public class SolicitarMaterialDTO
    {
        public int RepuestoId { get; set; }
        public int Cantidad { get; set; }
        public int OrdenTrabajoId { get; set; }
        public string Observaciones { get; set; }
    }

    public class MaterialUsadoHistorialDTO
    {
        public int Id { get; set; }
        public int OrdenTrabajoId { get; set; }
        public string OrdenTrabajoEstado { get; set; }
        public int RepuestoId { get; set; }
        public string RepuestoNombre { get; set; }
        public int Cantidad { get; set; }
        public decimal CostoUnitario { get; set; }
        public decimal Total { get; set; }
        public DateTime FechaUso { get; set; }
        public string Observaciones { get; set; }
    }
}
