using AutoFix.Entities;
using AutoFix.infraestructure.DBContext;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace AutoFix.infraestructure.Repositories
{
    public class ExpedienteVehiculoRepository : IExpedienteVehiculoRepository
    {
        private readonly AutoFixContext _context;

        public ExpedienteVehiculoRepository(AutoFixContext context)
        {
            _context = context;
        }

        public IEnumerable<OrdenTrabajo> GetHistorialPorVehiculo(int vehiculoId)
        {
            return _context.OrdenesTrabajo
                .Include(o => o.Mecanico)
                .Include(o => o.CitaSolicitud)
                .Include(o => o.MaterialesUsados.Select(m => m.Repuesto))
                .Where(o => !o.Borrado && o.CitaSolicitud.VehiculoId == vehiculoId)
                .OrderByDescending(o => o.FechaFinalizacion ?? o.FechaAsignacion)
                .ToList();
        }
    }
}
