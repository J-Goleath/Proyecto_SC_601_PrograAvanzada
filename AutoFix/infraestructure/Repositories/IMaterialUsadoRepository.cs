using AutoFix.Entities;
using System;
using System.Collections.Generic;

namespace AutoFix.infraestructure.Repositories
{
    public interface IMaterialUsadoRepository : IRepository<MaterialUsado>
    {
        IEnumerable<MaterialUsado> GetMaterialesByOrdenTrabajo(int ordenTrabajoId);
        IEnumerable<MaterialUsado> GetMaterialesByRepuesto(int repuestoId);
        IEnumerable<MaterialUsado> GetMaterialesByFecha(DateTime fechaInicio, DateTime fechaFin);
    }
}