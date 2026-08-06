using AutoFix.Entities;
using AutoFix.infraestructure.DBContext;
using AutoFix.infraestructure.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace AutoFix.infraestructure.Repositories
{
    public class RepuestoRepository : Repository<Repuesto>, IRepuestoRepository
    {
        public RepuestoRepository(AutoFixContext context) : base(context)
        {
        }

        public IEnumerable<Repuesto> GetRepuestosDisponibles()
        {
            return Context.Set<Repuesto>()
                .Where(r => !r.Borrado && r.Stock > 0)
                .OrderBy(r => r.Nombre)
                .ToList();
        }

        public IEnumerable<Repuesto> GetRepuestosByCategoria(string categoria)
        {
            return Context.Set<Repuesto>()
                .Where(r => !r.Borrado && r.Categoria == categoria)
                .OrderBy(r => r.Nombre)
                .ToList();
        }

        public Repuesto GetByCodigo(string codigo)
        {
            return Context.Set<Repuesto>()
                .FirstOrDefault(r => r.Codigo == codigo && !r.Borrado);
        }

        public void UpdateStock(int repuestoId, int cantidad)
        {
            var repuesto = GetById(repuestoId);
            if (repuesto != null)
            {
                repuesto.Stock -= cantidad;
                Update(repuesto);
            }
        }
    }
}