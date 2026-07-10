using AutoFix.Entities;
using AutoFix.infraestructure.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoFix.infraestructure.Repositories
{
    public class CitaSolicitudRepository : Repository<CitaSolicitud>, ICitaSolicitudRepository
    {
        public CitaSolicitudRepository(AutoFixContext context) : base(context)
        {
        }

        public IEnumerable<CitaSolicitud> GetCitasByVehiculo(int vehiculoId)
        {
            return Context.CitasSolicitud
                .Where(c => c.VehiculoId == vehiculoId && !c.Borrado)
                .ToList();
        }

        public IEnumerable<CitaSolicitud> GetCitasByCliente(int clienteId)
        {
            return Context.CitasSolicitud
                .Where(c => c.Vehiculo.ClienteId == clienteId && !c.Borrado)
                .OrderByDescending(c => c.Fecha)
                .ToList();
        }

        public IEnumerable<CitaSolicitud> GetCitasByMecanico(int mecanicoId)
        {
            return Context.CitasSolicitud
                .Where(c => c.MecanicoId == mecanicoId && !c.Borrado)
                .OrderBy(c => c.Fecha)
                .ThenBy(c => c.Hora)
                .ToList();
        }

        public IEnumerable<CitaSolicitud> GetCitasByMecanicoYSemana(int mecanicoId, DateTime inicioSemana, DateTime finSemana)
        {
            return Context.CitasSolicitud
                .Where(c => c.MecanicoId == mecanicoId
                    && !c.Borrado
                    && c.Fecha >= inicioSemana
                    && c.Fecha <= finSemana)
                .OrderBy(c => c.Fecha)
                .ThenBy(c => c.Hora)
                .ToList();
        }

        public IEnumerable<CitaSolicitud> GetCitasPendientes()
        {
            return Context.CitasSolicitud
                .Where(c => !c.Procesada && !c.Borrado)
                .ToList();
        }
    }
}
