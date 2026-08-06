using AutoFix.Entities;
using System.Collections.Generic;

namespace AutoFix.infraestructure.Repositories
{
    public interface IRepuestoRepository : IRepository<Repuesto>
    {
        IEnumerable<Repuesto> GetRepuestosDisponibles();
        IEnumerable<Repuesto> GetRepuestosByCategoria(string categoria);
        Repuesto GetByCodigo(string codigo);
        void UpdateStock(int repuestoId, int cantidad);
    }
}