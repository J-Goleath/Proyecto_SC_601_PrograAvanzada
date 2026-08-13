using AutoFix.Domain.Interfaces.Repositories;
using AutoFix.Domain.Entities;
using System.Collections.Generic;

namespace AutoFix.Domain.Interfaces.Repositories
{
    public interface IRepuestoRepository : IRepository<Repuesto>
    {
        IEnumerable<Repuesto> GetRepuestosDisponibles();
        IEnumerable<Repuesto> GetRepuestosByCategoria(string categoria);
        Repuesto GetByCodigo(string codigo);
        void UpdateStock(int repuestoId, int cantidad);
    }
}
