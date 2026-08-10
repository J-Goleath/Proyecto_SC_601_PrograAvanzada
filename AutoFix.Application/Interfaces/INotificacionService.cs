using AutoFix.Application.DTOs;
using AutoFix.Application.Common;
using System.Collections.Generic;

namespace AutoFix.Application.Interfaces
{
    public interface INotificacionService
    {
        Result<NotificacionDTO> GetById(int id);
        Result<List<NotificacionDTO>> GetAll();
        Result<List<NotificacionDTO>> GetByCliente(int clienteId);
        Result<List<NotificacionDTO>> GetNoLeidasByCliente(int clienteId);
        Result<NotificacionDTO> Create(CreateNotificacionDTO dto);
        Result<NotificacionDTO> Update(UpdateNotificacionDTO dto);
        Result<bool> Delete(int id);
        Result<bool> MarcarComoLeida(int id);
        Result<bool> MarcarTodasComoLeidas(int clienteId);
    }
}