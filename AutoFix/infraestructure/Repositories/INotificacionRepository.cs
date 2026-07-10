using AutoFix.Entities;
using System.Collections.Generic;

namespace AutoFix.infraestructure.Repositories
{
    public interface INotificacionRepository : IRepository<Notificacion>
    {
        IEnumerable<Notificacion> GetByCliente(int clienteId);
        IEnumerable<Notificacion> GetNoLeidasByCliente(int clienteId);
    }
}
