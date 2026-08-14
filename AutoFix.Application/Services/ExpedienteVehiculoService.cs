using AutoFix.Application.DTOs;
using AutoFix.Application.Interfaces;
using AutoFix.Application.Common;
using AutoFix.Domain.Interfaces.Repositories;
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
                    HistorialReparaciones = historial.Select(o => new HistorialReparacionDTO
                    {
                        OrdenTrabajoId = o.Id,
                        Descripcion = o.DescripcionTrabajo,
                        Diagnostico = o.Diagnostico,
                        Estado = string.IsNullOrWhiteSpace(o.Estado) ? "Pendiente" : o.Estado,
                        Fecha = o.FechaFinalizacion ?? o.FechaAsignacion,
                        MecanicoNombre = o.Mecanico?.Nombre ?? "N/A",
                        MaterialesUsados = o.MaterialesUsados == null
                            ? new List<MaterialUsadoDTO>()
                            : o.MaterialesUsados.Where(m => !m.Borrado).Select(m => new MaterialUsadoDTO
                            {
                                Id = m.Id,
                                OrdenTrabajoId = m.OrdenTrabajoId,
                                RepuestoId = m.RepuestoId,
                                RepuestoNombre = m.Repuesto?.Nombre ?? "N/A",
                                RepuestoCodigo = m.Repuesto?.Codigo ?? "N/A",
                                Cantidad = m.Cantidad,
                                CostoUnitario = m.CostoUnitario,
                                Observaciones = m.Observaciones,
                                FechaUso = m.FechaUso,
                                Borrado = m.Borrado
                            }).ToList()
                    }).OrderByDescending(h => h.Fecha).ToList(),
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
