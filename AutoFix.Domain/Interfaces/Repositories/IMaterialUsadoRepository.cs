using AutoFix.Domain.Interfaces.Repositories;
using AutoFix.Domain.Entities;
using System;
using System.Collections.Generic;
namespace AutoFix.Domain.Interfaces.Repositories
{
    public interface IMaterialUsadoRepository : IRepository<MaterialUsado>
    {
        IEnumerable<MaterialUsado> GetMaterialesByOrdenTrabajo(int ordenTrabajoId);
        IEnumerable<MaterialUsado> GetMaterialesByRepuesto(int repuestoId);
        IEnumerable<MaterialUsado> GetMaterialesByFecha(DateTime fechaInicio, DateTime fechaFin);
    }
}
