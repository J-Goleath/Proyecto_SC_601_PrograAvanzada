using AutoFix.Entities;
using AutoFix.infraestructure.DBContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace AutoFix.infraestructure.Repositories
{
    public class MaterialUsadoRepository : Repository<MaterialUsado>, IMaterialUsadoRepository
    {
        public MaterialUsadoRepository(AutoFixContext context) : base(context)
        {
        }

        public IEnumerable<MaterialUsado> GetMaterialesByOrdenTrabajo(int ordenTrabajoId)
        {
            return Context.Set<MaterialUsado>()
                .Include(m => m.Repuesto)
                .Where(m => !m.Borrado && m.OrdenTrabajoId == ordenTrabajoId)
                .OrderByDescending(m => m.FechaUso)
                .ToList();
        }

        public IEnumerable<MaterialUsado> GetMaterialesByRepuesto(int repuestoId)
        {
            return Context.Set<MaterialUsado>()
                .Include(m => m.Repuesto)
                .Where(m => !m.Borrado && m.RepuestoId == repuestoId)
                .OrderByDescending(m => m.FechaUso)
                .ToList();
        }

        public IEnumerable<MaterialUsado> GetMaterialesByFecha(DateTime fechaInicio, DateTime fechaFin)
        {
            return Context.Set<MaterialUsado>()
                .Include(m => m.Repuesto)
                .Where(m => !m.Borrado && m.FechaUso >= fechaInicio && m.FechaUso <= fechaFin)
                .OrderByDescending(m => m.FechaUso)
                .ToList();
        }
    }
}