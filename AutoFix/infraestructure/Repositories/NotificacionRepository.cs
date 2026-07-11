using AutoFix.Entities;
using AutoFix.infraestructure.DBContext;
using System.Collections.Generic;
using System.Linq;

namespace AutoFix.infraestructure.Repositories
{
    public class NotificacionRepository : Repository<Notificacion>, INotificacionRepository
    {
        public NotificacionRepository(AutoFixContext context) : base(context)
        {
        }

        public IEnumerable<Notificacion> GetByCliente(int clienteId)
        {
            return Context.Notificaciones
                .Where(n => n.ClienteId == clienteId && !n.Borrado)
                .OrderByDescending(n => n.FechaEnvio)
                .ToList();
        }

        public IEnumerable<Notificacion> GetNoLeidasByCliente(int clienteId)
        {
            return Context.Notificaciones
                .Where(n => n.ClienteId == clienteId && !n.Leida && !n.Borrado)
                .OrderByDescending(n => n.FechaEnvio)
                .ToList();
        }
    }
}
