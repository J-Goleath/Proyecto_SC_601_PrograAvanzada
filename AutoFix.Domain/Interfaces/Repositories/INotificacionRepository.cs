using AutoFix.Domain.Interfaces.Repositories;
using AutoFix.Domain.Entities;
using System.Collections.Generic;

namespace AutoFix.Domain.Interfaces.Repositories
{
    public interface INotificacionRepository : IRepository<Notificacion>
    {
        IEnumerable<Notificacion> GetByCliente(int clienteId);
        IEnumerable<Notificacion> GetNoLeidasByCliente(int clienteId);
    }
}

