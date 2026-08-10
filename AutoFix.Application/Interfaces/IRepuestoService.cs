using AutoFix.Application.DTOs;
using AutoFix.Application.Common;
using System.Collections.Generic;

namespace AutoFix.Application.Interfaces
{
    public interface IRepuestoService
    {
        Result<RepuestoDTO> GetById(int id);
        Result<List<RepuestoDTO>> GetAll();
        Result<List<RepuestoDTO>> GetDisponibles();
        Result<List<RepuestoDTO>> GetByCategoria(string categoria);
        Result<RepuestoDTO> GetByCodigo(string codigo);
        Result<RepuestoDTO> Create(CreateRepuestoDTO dto);
        Result<RepuestoDTO> Update(UpdateRepuestoDTO dto);
        Result<bool> Delete(int id);
        Result<bool> UpdateStock(int repuestoId, int cantidad);
        Result<bool> ValidateStock(int repuestoId, int cantidadSolicitada);
    }
}
