using AutoFix.Entities;
using System.Collections.Generic;

namespace AutoFix.infraestructure.Repositories
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