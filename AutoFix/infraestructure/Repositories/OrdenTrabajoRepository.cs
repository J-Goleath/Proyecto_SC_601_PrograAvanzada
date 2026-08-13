using AutoFix.Domain.Interfaces.Repositories;
using AutoFix.Domain.Entities;
using AutoFix.infraestructure.DBContext;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
namespace AutoFix.infraestructure.Repositories
{
    public class OrdenTrabajoRepository : IOrdenTrabajoRepository
    {
        private readonly AutoFixContext db;

        public OrdenTrabajoRepository()
        {
            db = new AutoFixContext();
        }

        public IEnumerable<OrdenTrabajo> ObtenerTodas()
        {
            return db.OrdenesTrabajo
                .Include(o => o.CitaSolicitud)
                .Include(o => o.Cliente)
                .Include(o => o.Mecanico)
                .Where(o => !o.Borrado)
                .ToList();
        }

        public OrdenTrabajo ObtenerPorId(int id)
        {
            return db.OrdenesTrabajo
                .Include(o => o.CitaSolicitud)
                .Include(o => o.Cliente)
                .Include(o => o.Mecanico)
                .FirstOrDefault(o => o.Id == id && !o.Borrado);
        }

        public void Agregar(OrdenTrabajo ordenTrabajo)
        {
            db.OrdenesTrabajo.Add(ordenTrabajo);
        }

        public void Actualizar(OrdenTrabajo ordenTrabajo)
        {
            db.Entry(ordenTrabajo).State = EntityState.Modified;
        }

        public void EliminarLogico(int id)
        {
            var ordenTrabajo = db.OrdenesTrabajo.Find(id);

            if (ordenTrabajo != null)
            {
                ordenTrabajo.Borrado = true;
                db.Entry(ordenTrabajo).State = EntityState.Modified;
            }
        }

        public void Guardar()
        {
            db.SaveChanges();
        }
    }
}


