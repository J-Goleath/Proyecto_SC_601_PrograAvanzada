using AutoFix.Application.DTOs;
using AutoFix.Application.Common;
using System.Collections.Generic;

namespace AutoFix.Application.Interfaces
{
    public interface IVehiculoService
    {
        Result<VehiculoDTO> GetById(int id);
        Result<List<VehiculoDTO>> GetAll();
        Result<List<VehiculoDTO>> GetByCliente(int clienteId);
        Result<VehiculoDTO> GetByPlaca(string placa);
        Result<VehiculoDTO> Create(CreateVehiculoDTO dto);
        Result<VehiculoDTO> Update(UpdateVehiculoDTO dto);
        Result<bool> Delete(int id);
        Result<bool> ExistePlaca(string placa);
    }
}