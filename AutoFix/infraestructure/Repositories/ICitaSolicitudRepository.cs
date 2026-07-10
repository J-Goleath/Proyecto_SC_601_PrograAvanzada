using AutoFix.Entities;
using System;
using System.Collections.Generic;

namespace AutoFix.infraestructure.Repositories
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