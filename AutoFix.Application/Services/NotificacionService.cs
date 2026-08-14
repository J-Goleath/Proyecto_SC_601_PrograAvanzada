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
    public class NotificacionService : INotificacionService
    {
        private readonly INotificacionRepository _notificacionRepository;
        private readonly IClienteRepository _clienteRepository;

        public NotificacionService(INotificacionRepository notificacionRepository, IClienteRepository clienteRepository)
        {
            _notificacionRepository = notificacionRepository;
            _clienteRepository = clienteRepository;
        }

        public Result<NotificacionDTO> GetById(int id)
        {
            try
            {
                var notificacion = _notificacionRepository.GetById(id);
                if (notificacion == null || notificacion.Borrado)
                    return Result<NotificacionDTO>.Fail(ResultError.NotFound("Notificación"));

                return Result<NotificacionDTO>.Ok(MapToDTO(notificacion));
            }
            catch (Exception ex)
            {
                return Result<NotificacionDTO>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<List<NotificacionDTO>> GetAll()
        {
            try
            {
                var notificaciones = _notificacionRepository.GetAll()
                    .Where(n => !n.Borrado)
                    .OrderByDescending(n => n.FechaEnvio)
                    .ToList();

                return Result<List<NotificacionDTO>>.Ok(notificaciones.Select(MapToDTO).ToList());
            }
            catch (Exception ex)
            {
                return Result<List<NotificacionDTO>>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<List<NotificacionDTO>> GetByCliente(int clienteId)
        {
            try
            {
                var notificaciones = _notificacionRepository.GetByCliente(clienteId).ToList();
                return Result<List<NotificacionDTO>>.Ok(notificaciones.Select(MapToDTO).ToList());
            }
            catch (Exception ex)
            {
                return Result<List<NotificacionDTO>>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<List<NotificacionDTO>> GetNoLeidasByCliente(int clienteId)
        {
            try
            {
                var notificaciones = _notificacionRepository.GetNoLeidasByCliente(clienteId).ToList();
                return Result<List<NotificacionDTO>>.Ok(notificaciones.Select(MapToDTO).ToList());
            }
            catch (Exception ex)
            {
                return Result<List<NotificacionDTO>>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<NotificacionDTO> Create(CreateNotificacionDTO dto)
        {
            try
            {
                var cliente = _clienteRepository.GetById(dto.ClienteId);
                if (cliente == null || cliente.Borrado)
                    return Result<NotificacionDTO>.Fail(ResultError.NotFound("Cliente"));

                var notificacion = new Notificacion
                {
                    Mensaje = dto.Mensaje,
                    ClienteId = dto.ClienteId,
                    FechaEnvio = DateTime.Now,
                    Leida = false,
                    Borrado = false
                };

                _notificacionRepository.Add(notificacion);
                return Result<NotificacionDTO>.Ok(MapToDTO(notificacion));
            }
            catch (Exception ex)
            {
                return Result<NotificacionDTO>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<NotificacionDTO> Update(UpdateNotificacionDTO dto)
        {
            try
            {
                var notificacion = _notificacionRepository.GetById(dto.Id);
                if (notificacion == null || notificacion.Borrado)
                    return Result<NotificacionDTO>.Fail(ResultError.NotFound("Notificación"));

                notificacion.Leida = dto.Leida;
                _notificacionRepository.Update(notificacion);

                return Result<NotificacionDTO>.Ok(MapToDTO(notificacion));
            }
            catch (Exception ex)
            {
                return Result<NotificacionDTO>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<bool> Delete(int id)
        {
            try
            {
                var notificacion = _notificacionRepository.GetById(id);
                if (notificacion == null || notificacion.Borrado)
                    return Result<bool>.Fail(ResultError.NotFound("Notificación"));

                notificacion.Borrado = true;
                _notificacionRepository.Update(notificacion);
                return Result<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<bool> MarcarComoLeida(int id)
        {
            try
            {
                var notificacion = _notificacionRepository.GetById(id);
                if (notificacion == null || notificacion.Borrado)
                    return Result<bool>.Fail(ResultError.NotFound("Notificación"));

                notificacion.Leida = true;
                _notificacionRepository.Update(notificacion);
                return Result<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<bool> MarcarTodasComoLeidas(int clienteId)
        {
            try
            {
                var notificaciones = _notificacionRepository.GetNoLeidasByCliente(clienteId).ToList();

                foreach (var notificacion in notificaciones)
                {
                    notificacion.Leida = true;
                    _notificacionRepository.Update(notificacion);
                }

                return Result<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        private NotificacionDTO MapToDTO(Notificacion notificacion)
        {
            return new NotificacionDTO
            {
                Id = notificacion.Id,
                Mensaje = notificacion.Mensaje,
                FechaEnvio = notificacion.FechaEnvio,
                Leida = notificacion.Leida,
                ClienteId = notificacion.ClienteId,
                ClienteNombre = notificacion.Cliente?.Nombre ?? "N/A",
                Borrado = notificacion.Borrado
            };
        }
    }
}