using AutoFix.Application.DTOs;
using AutoFix.Application.Interfaces;
using AutoFix.Application.Common;
using AutoFix.infraestructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoFix.Application.Services
{
    public class ExpedienteVehiculoService : IExpedienteVehiculoService
    {
        private readonly IExpedienteVehiculoRepository _expedienteRepository;
        private readonly IVehiculoRepository _vehiculoRepository;

        public ExpedienteVehiculoService(
            IExpedienteVehiculoRepository expedienteRepository,
            IVehiculoRepository vehiculoRepository)
        {
            _expedienteRepository = expedienteRepository;
            _vehiculoRepository = vehiculoRepository;
        }

        public Result<ExpedienteVehiculoDTO> GetExpedienteByVehiculo(int vehiculoId)
        {
            try
            {
                var vehiculo = _vehiculoRepository.GetById(vehiculoId);
                if (vehiculo == null || vehiculo.Borrado)
                    return Result<ExpedienteVehiculoDTO>.Fail(ResultError.NotFound("Vehículo"));

                var historial = _expedienteRepository.GetHistorialPorVehiculo(vehiculoId).ToList();

                var dto = new ExpedienteVehiculoDTO
                {
                    Vehiculo = new VehiculoDTO
                    {
                        Id = vehiculo.Id,
                        Placa = vehiculo.Placa,
                        Marca = vehiculo.Marca,
                        Modelo = vehiculo.Modelo,
                        Anio = vehiculo.Anio,
                        Color = vehiculo.Color,
                        ClienteId = vehiculo.ClienteId,
                        ClienteNombre = vehiculo.Cliente?.Nombre ?? "N/A",
                        FechaRegistro = vehiculo.FechaRegistro,
                        Borrado = vehiculo.Borrado
                    },
                    HistorialReparaciones = historial.Select(o => new OrdenTrabajoDTO
                    {
                        Id = o.Id,
                        DescripcionTrabajo = o.DescripcionTrabajo,
                        Diagnostico = o.Diagnostico,
                        Estado = o.Estado,
                        FechaAsignacion = o.FechaAsignacion,
                        FechaInicio = o.FechaInicio,
                        FechaFinalizacion = o.FechaFinalizacion,
                        MecanicoNombre = o.Mecanico?.Nombre ?? "N/A",
                        Prioridad = o.Prioridad
                    }).ToList(),
                    TotalReparaciones = historial.Count,
                    UltimaReparacion = historial.Any() ? historial.Max(o => o.FechaFinalizacion ?? o.FechaAsignacion) : (DateTime?)null
                };

                return Result<ExpedienteVehiculoDTO>.Ok(dto);
            }
            catch (Exception ex)
            {
                return Result<ExpedienteVehiculoDTO>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<ExpedienteVehiculoDTO> GetExpedienteByPlaca(string placa)
        {
            try
            {
                var vehiculo = _vehiculoRepository.GetByPlaca(placa);
                if (vehiculo == null)
                    return Result<ExpedienteVehiculoDTO>.Fail(ResultError.NotFound("Vehículo con esta placa"));

                return GetExpedienteByVehiculo(vehiculo.Id);
            }
            catch (Exception ex)
            {
                return Result<ExpedienteVehiculoDTO>.Fail(ResultError.InternalError(ex.Message));
            }
        }
    }
}