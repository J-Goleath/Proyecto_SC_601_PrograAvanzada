using AutoFix.Entities;
using AutoFix.infraestructure.DBContext;
using AutoFix.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoFix.infraestructure.Repositories
{
    public class ClienteRepository : Repository<Cliente>, IClienteRepository
    {
        public ClienteRepository(AutoFixContext context) : base(context)
        {
        }

        public Cliente GetByCorreo(string correo)
        {
            return Context.Clientes.FirstOrDefault(c => c.Correo == correo && !c.Borrado);
        }

        public bool ExisteCorreo(string correo)
        {
            return Context.Clientes.Any(c => c.Correo == correo && !c.Borrado);
        }

        public IEnumerable<Cliente> GetClientesActivos()
        {
            return Context.Clientes.Where(c => !c.Borrado).ToList();
        }

        public Cliente Login(string correo, string contraseña)
        {
            var cliente = GetByCorreo(correo);
            if (cliente == null)
                return null;

            // Verificar contraseña encriptada
            if (PasswordHelper.Verificar(contraseña, cliente.Contraseña))
                return cliente;

            return null;
        }
    }
}