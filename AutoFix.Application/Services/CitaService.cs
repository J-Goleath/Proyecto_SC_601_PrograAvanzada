using AutoFix.Application.DTOs;
using AutoFix.Application.Interfaces;
using AutoFix.Application.Common;
using AutoFix.Domain.Entities;
using AutoFix.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoFix.Application.Services
{
    public class CitaService : ICitaService
    {
        private readonly ICitaSolicitudRepository _citaRepository;
        private readonly IVehiculoRepository _vehiculoRepository;
        private readonly IClienteRepository _clienteRepository;

        public CitaService(
            ICitaSolicitudRepository citaRepository,
            IVehiculoRepository vehiculoRepository,
            IClienteRepository clienteRepository)
        {
            _citaRepository = citaRepository;
            _vehiculoRepository = vehiculoRepository;
            _clienteRepository = clienteRepository;
        }

        public Result<CitaDTO> GetById(int id)
        {
            try
            {
                var cita = _citaRepository.GetById(id);
                if (cita == null || cita.Borrado)
                    return Result<CitaDTO>.Fail(ResultError.NotFound("Cita"));

                return Result<CitaDTO>.Ok(MapToDTO(cita));
            }
            catch (Exception ex)
            {
                return Result<CitaDTO>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<List<CitaDTO>> GetAll()
        {
            try
            {
                var citas = _citaRepository.GetAll()
                    .Where(c => !c.Borrado)
                    .OrderByDescending(c => c.Fecha)
                    .ThenBy(c => c.Hora)
                    .ToList();

                return Result<List<CitaDTO>>.Ok(citas.Select(MapToDTO).ToList());
            }
            catch (Exception ex)
            {
                return Result<List<CitaDTO>>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<List<CitaDTO>> GetByVehiculo(int vehiculoId)
        {
            try
            {
                var citas = _citaRepository.GetCitasByVehiculo(vehiculoId).ToList();
                return Result<List<CitaDTO>>.Ok(citas.Select(MapToDTO).ToList());
            }
            catch (Exception ex)
            {
                return Result<List<CitaDTO>>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<List<CitaDTO>> GetByCliente(int clienteId)
        {
            try
            {
                var citas = _citaRepository.GetCitasByCliente(clienteId).ToList();
                return Result<List<CitaDTO>>.Ok(citas.Select(MapToDTO).ToList());
            }
            catch (Exception ex)
            {
                return Result<List<CitaDTO>>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<List<CitaDTO>> GetByMecanico(int mecanicoId)
        {
            try
            {
                var citas = _citaRepository.GetCitasByMecanico(mecanicoId).ToList();
                return Result<List<CitaDTO>>.Ok(citas.Select(MapToDTO).ToList());
            }
            catch (Exception ex)
            {
                return Result<List<CitaDTO>>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<List<CitaDTO>> GetByMecanicoYSemana(int mecanicoId, DateTime inicioSemana, DateTime finSemana)
        {
            try
            {
                var citas = _citaRepository.GetCitasByMecanicoYSemana(mecanicoId, inicioSemana, finSemana).ToList();
                return Result<List<CitaDTO>>.Ok(citas.Select(MapToDTO).ToList());
            }
            catch (Exception ex)
            {
                return Result<List<CitaDTO>>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<List<CitaDTO>> GetPendientes()
        {
            try
            {
                var citas = _citaRepository.GetCitasPendientes().ToList();
                return Result<List<CitaDTO>>.Ok(citas.Select(MapToDTO).ToList());
            }
            catch (Exception ex)
            {
                return Result<List<CitaDTO>>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<CitaDTO> Create(CreateCitaDTO dto)
        {
            try
            {
                var vehiculo = _vehiculoRepository.GetById(dto.VehiculoId);
                if (vehiculo == null || vehiculo.Borrado)
                    return Result<CitaDTO>.Fail(ResultError.NotFound("Vehículo"));

                if (dto.MecanicoId.HasValue)
                {
                    var mecanico = _clienteRepository.GetById(dto.MecanicoId.Value);
                    if (mecanico == null || mecanico.Borrado)
                        return Result<CitaDTO>.Fail(ResultError.NotFound("Mecánico"));
                }

                var cita = new CitaSolicitud
                {
                    Fecha = dto.Fecha,
                    Hora = dto.Hora,
                    DescripcionFallos = dto.DescripcionFallos,
                    VehiculoId = dto.VehiculoId,
                    MecanicoId = dto.MecanicoId,
                    Procesada = false,
                    FechaRegistro = DateTime.Now,
                    Borrado = false
                };

                _citaRepository.Add(cita);
                return Result<CitaDTO>.Ok(MapToDTO(cita));
            }
            catch (Exception ex)
            {
                return Result<CitaDTO>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<CitaDTO> Update(UpdateCitaDTO dto)
        {
            try
            {
                var cita = _citaRepository.GetById(dto.Id);
                if (cita == null || cita.Borrado)
                    return Result<CitaDTO>.Fail(ResultError.NotFound("Cita"));

                var vehiculo = _vehiculoRepository.GetById(dto.VehiculoId);
                if (vehiculo == null || vehiculo.Borrado)
                    return Result<CitaDTO>.Fail(ResultError.NotFound("Vehículo"));

                if (dto.MecanicoId.HasValue)
                {
                    var mecanico = _clienteRepository.GetById(dto.MecanicoId.Value);
                    if (mecanico == null || mecanico.Borrado)
                        return Result<CitaDTO>.Fail(ResultError.NotFound("Mecánico"));
                }

                cita.Fecha = dto.Fecha;
                cita.Hora = dto.Hora;
                cita.DescripcionFallos = dto.DescripcionFallos;
                cita.VehiculoId = dto.VehiculoId;
                cita.MecanicoId = dto.MecanicoId;
                cita.Procesada = dto.Procesada;

                _citaRepository.Update(cita);
                return Result<CitaDTO>.Ok(MapToDTO(cita));
            }
            catch (Exception ex)
            {
                return Result<CitaDTO>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<bool> Delete(int id)
        {
            try
            {
                var cita = _citaRepository.GetById(id);
                if (cita == null || cita.Borrado)
                    return Result<bool>.Fail(ResultError.NotFound("Cita"));

                cita.Borrado = true;
                _citaRepository.Update(cita);
                return Result<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<bool> ProcesarCita(int id)
        {
            try
            {
                var cita = _citaRepository.GetById(id);
                if (cita == null || cita.Borrado)
                    return Result<bool>.Fail(ResultError.NotFound("Cita"));

                cita.Procesada = true;
                _citaRepository.Update(cita);
                return Result<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        private CitaDTO MapToDTO(CitaSolicitud cita)
        {
            return new CitaDTO
            {
                Id = cita.Id,
                Fecha = cita.Fecha,
                Hora = cita.Hora,
                DescripcionFallos = cita.DescripcionFallos,
                Procesada = cita.Procesada,
                VehiculoId = cita.VehiculoId,
                VehiculoPlaca = cita.Vehiculo?.Placa ?? "N/A",
                ClienteId = cita.Vehiculo?.ClienteId ?? 0,
                ClienteNombre = cita.Vehiculo?.Cliente?.Nombre ?? "N/A",
                MecanicoId = cita.MecanicoId,
                MecanicoNombre = cita.Mecanico?.Nombre ?? "No asignado",
                FechaRegistro = cita.FechaRegistro,
                Borrado = cita.Borrado
            };
        }
    }
}
