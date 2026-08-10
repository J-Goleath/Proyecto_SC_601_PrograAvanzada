using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFix.Application.DTOs
{
    public class ClienteDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Rol { get; set; }  // "Cliente", "Mecanico", "Administrador"
        public DateTime FechaRegistro { get; set; }
        public bool Borrado { get; set; }
    }

    public class CreateClienteDTO
    {
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Contraseña { get; set; }
        public string Rol { get; set; }
    }

    public class LoginDTO
    {
        public string Correo { get; set; }
        public string Contraseña { get; set; }
    }

    public class ClienteResponseDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Rol { get; set; }
    }
}
