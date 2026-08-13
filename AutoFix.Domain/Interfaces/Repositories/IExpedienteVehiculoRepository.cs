using AutoFix.Domain.Interfaces.Repositories;
using AutoFix.Domain.Entities;
using System.Collections.Generic;

namespace AutoFix.Domain.Interfaces.Repositories
{

    public interface IExpedienteVehiculoRepository
    {
        IEnumerable<OrdenTrabajo> GetHistorialPorVehiculo(int vehiculoId);
    }
}

