using AutoFix.Domain.Interfaces.Repositories;
using AutoFix.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoFix.Domain.Interfaces.Repositories
{
    public interface IClienteRepository : IRepository<Cliente>
    {
        Cliente GetByCorreo(string correo);
        bool ExisteCorreo(string correo);
        IEnumerable<Cliente> GetClientesActivos();
        Cliente Login(string correo, string contraseña);
    }
}
