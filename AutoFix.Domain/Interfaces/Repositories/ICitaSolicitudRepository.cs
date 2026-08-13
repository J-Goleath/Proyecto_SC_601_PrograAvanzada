using AutoFix.Domain.Interfaces.Repositories;
using AutoFix.Domain.Entities;
using System;
using System.Collections.Generic;

namespace AutoFix.Domain.Interfaces.Repositories
{
    public interface ICitaSolicitudRepository : IRepository<CitaSolicitud>
    {
        IEnumerable<CitaSolicitud> GetCitasByVehiculo(int vehiculoId);
        IEnumerable<CitaSolicitud> GetCitasByCliente(int clienteId);
        IEnumerable<CitaSolicitud> GetCitasByMecanico(int mecanicoId);
        IEnumerable<CitaSolicitud> GetCitasByMecanicoYSemana(int mecanicoId, DateTime inicioSemana, DateTime finSemana);
        IEnumerable<CitaSolicitud> GetCitasPendientes();
    }
}
