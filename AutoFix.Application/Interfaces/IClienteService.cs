using AutoFix.Application.DTOs;
using AutoFix.Application.Common;
using System.Collections.Generic;

namespace AutoFix.Application.Interfaces
{
    public interface IClienteService
    {
        Result<ClienteDTO> GetById(int id);
        Result<List<ClienteDTO>> GetAll();
        Result<ClienteDTO> GetByCorreo(string correo);
        Result<ClienteResponseDTO> Login(LoginDTO loginDTO);
        Result<ClienteDTO> Create(CreateClienteDTO dto);
        Result<ClienteDTO> Update(ClienteDTO dto);
        Result<bool> Delete(int id);
        Result<bool> ExisteCorreo(string correo);
    }
}