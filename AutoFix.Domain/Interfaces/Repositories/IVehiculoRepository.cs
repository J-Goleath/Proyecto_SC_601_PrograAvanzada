using AutoFix.Domain.Interfaces.Repositories;
using AutoFix.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace AutoFix.Domain.Interfaces.Repositories
{
    public interface IVehiculoRepository : IRepository<Vehiculo>
    {
        IEnumerable<Vehiculo> GetVehiculosByCliente(int clienteId);
        bool ExistePlaca(string placa);
        Vehiculo GetByPlaca(string placa);
    }
}
