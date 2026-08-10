using AutoFix.Application.DTOs;
using AutoFix.Application.Interfaces;
using AutoFix.Application.Common;
using AutoFix.Entities;
using AutoFix.infraestructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoFix.Application.Services
{
    public class OrdenTrabajoService : IOrdenTrabajoService
    {
        private readonly IOrdenTrabajoRepository _ordenTrabajoRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly ICitaSolicitudRepository _citaRepository;

        public OrdenTrabajoService(
            IOrdenTrabajoRepository ordenTrabajoRepository,
            IClienteRepository clienteRepository,
            ICitaSolicitudRepository citaRepository)
        {
            _ordenTrabajoRepository = ordenTrabajoRepository;
            _clienteRepository = clienteRepository;
            _citaRepository = citaRepository;
        }

        public Result<OrdenTrabajoDTO> GetById(int id)
        {
            try
            {
                var orden = _ordenTrabajoRepository.ObtenerPorId(id);
                if (orden == null || orden.Borrado)
                    return Result<OrdenTrabajoDTO>.Fail(ResultError.NotFound("Orden de trabajo"));

                return Result<OrdenTrabajoDTO>.Ok(MapToDTO(orden));
            }
            catch (Exception ex)
            {
                return Result<OrdenTrabajoDTO>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<List<OrdenTrabajoDTO>> GetAll()
        {
            try
            {
                var ordenes = _ordenTrabajoRepository.ObtenerTodas()
                    .Where(o => !o.Borrado)
                    .OrderByDescending(o => o.FechaAsignacion)
                    .ToList();

                return Result<List<OrdenTrabajoDTO>>.Ok(ordenes.Select(MapToDTO).ToList());
            }
            catch (Exception ex)
            {
                return Result<List<OrdenTrabajoDTO>>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<OrdenTrabajoDTO> Create(CreateOrdenTrabajoDTO dto)
        {
            try
            {
                var cliente = _clienteRepository.GetById(dto.ClienteId);
                if (cliente == null || cliente.Borrado)
                    return Result<OrdenTrabajoDTO>.Fail(ResultError.NotFound("Cliente"));

                var mecanico = _clienteRepository.GetById(dto.MecanicoId);
                if (mecanico == null || mecanico.Borrado)
                    return Result<OrdenTrabajoDTO>.Fail(ResultError.NotFound("Mecánico"));

                var cita = _citaRepository.GetById(dto.CitaSolicitudId);
                if (cita == null || cita.Borrado)
                    return Result<OrdenTrabajoDTO>.Fail(ResultError.NotFound("Cita"));

                var orden = new OrdenTrabajo
                {
                    CitaSolicitudId = dto.CitaSolicitudId,
                    ClienteId = dto.ClienteId,
                    MecanicoId = dto.MecanicoId,
                    Estado = "Pendiente",
                    DescripcionTrabajo = dto.DescripcionTrabajo,
                    Prioridad = dto.Prioridad,
                    FechaAsignacion = DateTime.Now,
                    Borrado = false
                };

                _ordenTrabajoRepository.Agregar(orden);
                _ordenTrabajoRepository.Guardar();

                return Result<OrdenTrabajoDTO>.Ok(MapToDTO(orden));
            }
            catch (Exception ex)
            {
                return Result<OrdenTrabajoDTO>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<OrdenTrabajoDTO> Update(UpdateOrdenTrabajoDTO dto)
        {
            try
            {
                var orden = _ordenTrabajoRepository.ObtenerPorId(dto.Id);
                if (orden == null || orden.Borrado)
                    return Result<OrdenTrabajoDTO>.Fail(ResultError.NotFound("Orden de trabajo"));

                orden.Estado = dto.Estado;
                orden.Diagnostico = dto.Diagnostico;
                orden.Observaciones = dto.Observaciones;
                orden.FechaInicio = dto.FechaInicio;
                orden.FechaFinalizacion = dto.FechaFinalizacion;

                _ordenTrabajoRepository.Actualizar(orden);
                _ordenTrabajoRepository.Guardar();

                return Result<OrdenTrabajoDTO>.Ok(MapToDTO(orden));
            }
            catch (Exception ex)
            {
                return Result<OrdenTrabajoDTO>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<bool> Delete(int id)
        {
            try
            {
                var orden = _ordenTrabajoRepository.ObtenerPorId(id);
                if (orden == null || orden.Borrado)
                    return Result<bool>.Fail(ResultError.NotFound("Orden de trabajo"));

                _ordenTrabajoRepository.EliminarLogico(id);
                _ordenTrabajoRepository.Guardar();

                return Result<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<bool> CambiarEstado(int id, string nuevoEstado)
        {
            try
            {
                var orden = _ordenTrabajoRepository.ObtenerPorId(id);
                if (orden == null || orden.Borrado)
                    return Result<bool>.Fail(ResultError.NotFound("Orden de trabajo"));

                // Validar transiciones de estado
                var estadosValidos = new[] { "Pendiente", "En Proceso", "Finalizada", "Cancelada" };
                if (!estadosValidos.Contains(nuevoEstado))
                    return Result<bool>.Fail(ResultError.ValidationError("Estado inválido"));

                orden.Estado = nuevoEstado;

                if (nuevoEstado == "En Proceso" && !orden.FechaInicio.HasValue)
                    orden.FechaInicio = DateTime.Now;

                if (nuevoEstado == "Finalizada")
                    orden.FechaFinalizacion = DateTime.Now;

                _ordenTrabajoRepository.Actualizar(orden);
                _ordenTrabajoRepository.Guardar();

                return Result<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<List<OrdenTrabajoDTO>> GetByEstado(string estado)
        {
            try
            {
                var ordenes = _ordenTrabajoRepository.ObtenerTodas()
                    .Where(o => !o.Borrado && o.Estado == estado)
                    .OrderByDescending(o => o.FechaAsignacion)
                    .ToList();

                return Result<List<OrdenTrabajoDTO>>.Ok(ordenes.Select(MapToDTO).ToList());
            }
            catch (Exception ex)
            {
                return Result<List<OrdenTrabajoDTO>>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<List<OrdenTrabajoDTO>> GetByCliente(int clienteId)
        {
            try
            {
                var ordenes = _ordenTrabajoRepository.ObtenerTodas()
                    .Where(o => !o.Borrado && o.ClienteId == clienteId)
                    .OrderByDescending(o => o.FechaAsignacion)
                    .ToList();

                return Result<List<OrdenTrabajoDTO>>.Ok(ordenes.Select(MapToDTO).ToList());
            }
            catch (Exception ex)
            {
                return Result<List<OrdenTrabajoDTO>>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<List<OrdenTrabajoDTO>> GetByMecanico(int mecanicoId)
        {
            try
            {
                var ordenes = _ordenTrabajoRepository.ObtenerTodas()
                    .Where(o => !o.Borrado && o.MecanicoId == mecanicoId)
                    .OrderByDescending(o => o.FechaAsignacion)
                    .ToList();

                return Result<List<OrdenTrabajoDTO>>.Ok(ordenes.Select(MapToDTO).ToList());
            }
            catch (Exception ex)
            {
                return Result<List<OrdenTrabajoDTO>>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        private OrdenTrabajoDTO MapToDTO(OrdenTrabajo orden)
        {
            return new OrdenTrabajoDTO
            {
                Id = orden.Id,
                CitaSolicitudId = orden.CitaSolicitudId,
                ClienteId = orden.ClienteId,
                ClienteNombre = orden.Cliente?.Nombre ?? "N/A",
                MecanicoId = orden.MecanicoId,
                MecanicoNombre = orden.Mecanico?.Nombre ?? "N/A",
                Estado = orden.Estado,
                DescripcionTrabajo = orden.DescripcionTrabajo,
                Diagnostico = orden.Diagnostico,
                Observaciones = orden.Observaciones,
                FechaAsignacion = orden.FechaAsignacion,
                FechaInicio = orden.FechaInicio,
                FechaFinalizacion = orden.FechaFinalizacion,
                Prioridad = orden.Prioridad,
                Borrado = orden.Borrado
            };
        }
    }
}