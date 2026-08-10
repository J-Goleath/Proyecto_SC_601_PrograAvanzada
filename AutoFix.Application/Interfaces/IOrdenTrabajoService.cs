using AutoFix.Application.DTOs;
using AutoFix.Application.Common;
using System.Collections.Generic;

namespace AutoFix.Application.Interfaces
{
    public interface IOrdenTrabajoService
    {
        Result<OrdenTrabajoDTO> GetById(int id);
        Result<List<OrdenTrabajoDTO>> GetAll();
        Result<OrdenTrabajoDTO> Create(CreateOrdenTrabajoDTO dto);
        Result<OrdenTrabajoDTO> Update(UpdateOrdenTrabajoDTO dto);
        Result<bool> Delete(int id);
        Result<bool> CambiarEstado(int id, string nuevoEstado);
        Result<List<OrdenTrabajoDTO>> GetByEstado(string estado);
        Result<List<OrdenTrabajoDTO>> GetByCliente(int clienteId);
        Result<List<OrdenTrabajoDTO>> GetByMecanico(int mecanicoId);
    }
}