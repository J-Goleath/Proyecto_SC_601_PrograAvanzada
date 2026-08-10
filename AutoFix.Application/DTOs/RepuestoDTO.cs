using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFix.Application.DTOs
{
    public class RepuestoDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public int Stock { get; set; }
        public decimal Precio { get; set; }
        public string Categoria { get; set; }
        public string Ubicacion { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool Borrado { get; set; }
    }

    public class CreateRepuestoDTO
    {
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public int Stock { get; set; }
        public decimal Precio { get; set; }
        public string Categoria { get; set; }
        public string Ubicacion { get; set; }
    }

    public class UpdateRepuestoDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public int Stock { get; set; }
        public decimal Precio { get; set; }
        public string Categoria { get; set; }
        public string Ubicacion { get; set; }
    }
}