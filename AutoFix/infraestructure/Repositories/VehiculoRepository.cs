using AutoFix.Entities;
using AutoFix.infraestructure.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoFix.infraestructure.Repositories
{
    public class VehiculoRepository : Repository<Vehiculo>, IVehiculoRepository
    {
        public VehiculoRepository(AutoFixContext context) : base(context)
        {
        }

        public IEnumerable<Vehiculo> GetVehiculosByCliente(int clienteId)
        {
            return Context.Vehiculos.Where(v => v.ClienteId == clienteId && !v.Borrado).ToList();
        }

        public bool ExistePlaca(string placa)
        {
            return Context.Vehiculos.Any(v => v.Placa.Equals(placa, System.StringComparison.OrdinalIgnoreCase) && !v.Borrado);
        }
    }
}