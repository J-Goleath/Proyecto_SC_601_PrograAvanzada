using AutoFix.Entities;
using System.Collections.Generic;

namespace AutoFix.infraestructure.Repositories
{

    public interface IExpedienteVehiculoRepository
    {
        IEnumerable<OrdenTrabajo> GetHistorialPorVehiculo(int vehiculoId);
    }
}
