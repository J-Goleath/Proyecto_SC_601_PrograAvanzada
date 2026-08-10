using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFix.Application.DTOs
{
    public class CitaDTO
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public string DescripcionFallos { get; set; }
        public bool Procesada { get; set; }
        public int VehiculoId { get; set; }
        public string VehiculoPlaca { get; set; }
        public int? MecanicoId { get; set; }
        public string MecanicoNombre { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool Borrado { get; set; }
    }

    public class CreateCitaDTO
    {
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public string DescripcionFallos { get; set; }
        public int VehiculoId { get; set; }
        public int? MecanicoId { get; set; }
    }

    public class UpdateCitaDTO
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public string DescripcionFallos { get; set; }
        public bool Procesada { get; set; }
        public int? MecanicoId { get; set; }
    }
}
