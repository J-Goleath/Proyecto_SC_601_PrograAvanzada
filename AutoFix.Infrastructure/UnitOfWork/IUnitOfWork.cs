using AutoFix.Domain.Interfaces.Repositories;
using System;

namespace AutoFix.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IClienteRepository Clientes { get; }
        IVehiculoRepository Vehiculos { get; }
        ICitaSolicitudRepository Citas { get; }
        INotificacionRepository Notificaciones { get; }
        IOrdenTrabajoRepository OrdenesTrabajo { get; }
        IRepuestoRepository Repuestos { get; }
        IMaterialUsadoRepository MaterialesUsados { get; }
        IExpedienteVehiculoRepository ExpedienteVehiculo { get; }

        void BeginTransaction();
        void Commit();
        void Rollback();

        int Save();
    }
}
