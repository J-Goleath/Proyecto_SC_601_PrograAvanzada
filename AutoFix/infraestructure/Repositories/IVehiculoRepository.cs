using AutoFix.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoFix.infraestructure.Repositories
{
    public interface IVehiculoRepository : IRepository<Vehiculo>
    {
        IEnumerable<Vehiculo> GetVehiculosByCliente(int clienteId);
        bool ExistePlaca(string placa);
    }
}