using AutoFix.Domain.Interfaces.Repositories;
using AutoFix.Domain.Entities;
using System.Collections.Generic;

namespace AutoFix.Domain.Interfaces.Repositories
{
    public interface IOrdenTrabajoRepository
    {
        IEnumerable<OrdenTrabajo> ObtenerTodas();
        OrdenTrabajo ObtenerPorId(int id);
        void Agregar(OrdenTrabajo ordenTrabajo);
        void Actualizar(OrdenTrabajo ordenTrabajo);
        void EliminarLogico(int id);
        void Guardar();
    }
}
