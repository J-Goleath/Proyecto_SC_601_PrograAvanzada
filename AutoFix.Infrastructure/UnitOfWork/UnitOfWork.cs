using AutoFix.infraestructure.DBContext;
using AutoFix.infraestructure.Repositories;
using AutoFix.Domain.Interfaces.Repositories;
using System;
using System.Data.Entity;

namespace AutoFix.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AutoFixContext _context;
        private DbContextTransaction _transaction;
        private bool _disposed;

        private IClienteRepository _clientes;
        private IVehiculoRepository _vehiculos;
        private ICitaSolicitudRepository _citas;
        private INotificacionRepository _notificaciones;
        private IOrdenTrabajoRepository _ordenesTrabajo;
        private IRepuestoRepository _repuestos;
        private IMaterialUsadoRepository _materialesUsados;
        private IExpedienteVehiculoRepository _expedienteVehiculo;

        public UnitOfWork(AutoFixContext context)
        {
            _context = context;
        }

        public IClienteRepository Clientes =>
            _clientes ?? (_clientes = new ClienteRepository(_context));

        public IVehiculoRepository Vehiculos =>
            _vehiculos ?? (_vehiculos = new VehiculoRepository(_context));

        public ICitaSolicitudRepository Citas =>
            _citas ?? (_citas = new CitaSolicitudRepository(_context));

        public INotificacionRepository Notificaciones =>
            _notificaciones ?? (_notificaciones = new NotificacionRepository(_context));

        public IOrdenTrabajoRepository OrdenesTrabajo =>
            _ordenesTrabajo ?? (_ordenesTrabajo = new OrdenTrabajoRepository(_context));

        public IRepuestoRepository Repuestos =>
            _repuestos ?? (_repuestos = new RepuestoRepository(_context));

        public IMaterialUsadoRepository MaterialesUsados =>
            _materialesUsados ?? (_materialesUsados = new MaterialUsadoRepository(_context));

        public IExpedienteVehiculoRepository ExpedienteVehiculo =>
            _expedienteVehiculo ?? (_expedienteVehiculo = new ExpedienteVehiculoRepository(_context));

        public void BeginTransaction()
        {
            if (_transaction != null)
                throw new InvalidOperationException("Ya hay una transacción activa.");

            _transaction = _context.Database.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                _context.SaveChanges();
                _transaction?.Commit();
            }
            catch
            {
                Rollback();
                throw;
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null;
            }
        }

        public void Rollback()
        {
            _transaction?.Rollback();
            _transaction?.Dispose();
            _transaction = null;
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                _transaction?.Dispose();
                _context?.Dispose();
            }

            _disposed = true;
        }
    }
}
