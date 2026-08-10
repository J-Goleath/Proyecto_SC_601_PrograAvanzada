using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFix.Application.DTOs
{
    public class VehiculoDTO
    {
        public int Id { get; set; }
        public string Placa { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Anio { get; set; }
        public string Color { get; set; }
        public int ClienteId { get; set; }
        public string ClienteNombre { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool Borrado { get; set; }
    }

    public class CreateVehiculoDTO
    {
        public string Placa { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Anio { get; set; }
        public string Color { get; set; }
        public int ClienteId { get; set; }
    }

    public class UpdateVehiculoDTO
    {
        public int Id { get; set; }
        public string Placa { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Anio { get; set; }
        public string Color { get; set; }
        public int ClienteId { get; set; }
    }
}