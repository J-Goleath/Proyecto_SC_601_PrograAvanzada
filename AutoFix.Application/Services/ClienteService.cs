using AutoFix.Application.DTOs;
using AutoFix.Application.Interfaces;
using AutoFix.Application.Common;
using AutoFix.Domain.Entities;
using AutoFix.Domain.Interfaces.Repositories;
using AutoFix.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoFix.Application.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteService(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public Result<ClienteDTO> GetById(int id)
        {
            try
            {
                var cliente = _clienteRepository.GetById(id);
                if (cliente == null || cliente.Borrado)
                    return Result<ClienteDTO>.Fail(ResultError.NotFound("Cliente"));

                return Result<ClienteDTO>.Ok(MapToDTO(cliente));
            }
            catch (Exception ex)
            {
                return Result<ClienteDTO>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<List<ClienteDTO>> GetAll()
        {
            try
            {
                var clientes = _clienteRepository.GetClientesActivos().ToList();
                return Result<List<ClienteDTO>>.Ok(clientes.Select(MapToDTO).ToList());
            }
            catch (Exception ex)
            {
                return Result<List<ClienteDTO>>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<ClienteDTO> GetByCorreo(string correo)
        {
            try
            {
                var cliente = _clienteRepository.GetByCorreo(correo);
                if (cliente == null)
                    return Result<ClienteDTO>.Fail(ResultError.NotFound("Cliente"));

                return Result<ClienteDTO>.Ok(MapToDTO(cliente));
            }
            catch (Exception ex)
            {
                return Result<ClienteDTO>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<ClienteResponseDTO> Login(LoginDTO loginDTO)
        {
            try
            {
                var cliente = _clienteRepository.Login(loginDTO.Correo, loginDTO.Contraseña);
                if (cliente == null)
                    return Result<ClienteResponseDTO>.Fail(ResultError.InvalidCredentials());

                return Result<ClienteResponseDTO>.Ok(new ClienteResponseDTO
                {
                    Id = cliente.Id,
                    Nombre = cliente.Nombre,
                    Correo = cliente.Correo,
                    Rol = cliente.Rol.ToString()
                });
            }
            catch (Exception ex)
            {
                return Result<ClienteResponseDTO>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<ClienteDTO> Create(CreateClienteDTO dto)
        {
            try
            {
                if (_clienteRepository.ExisteCorreo(dto.Correo))
                    return Result<ClienteDTO>.Fail(ResultError.AlreadyExists("Cliente con este correo"));

                var cliente = new Cliente
                {
                    Nombre = dto.Nombre,
                    Correo = dto.Correo,
                    Telefono = dto.Telefono,
                    Contraseña = PasswordHelper.Encriptar(dto.Contraseña),
                    Rol = (RolUsuario)Enum.Parse(typeof(RolUsuario), dto.Rol),
                    FechaRegistro = DateTime.Now,
                    Borrado = false
                };

                _clienteRepository.Add(cliente);
                return Result<ClienteDTO>.Ok(MapToDTO(cliente));
            }
            catch (Exception ex)
            {
                return Result<ClienteDTO>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<ClienteDTO> Update(UpdateClienteDTO dto)
        {
            try
            {
                var cliente = _clienteRepository.GetById(dto.Id);
                if (cliente == null || cliente.Borrado)
                    return Result<ClienteDTO>.Fail(ResultError.NotFound("Cliente"));

                var correoDuplicado = _clienteRepository.ExisteCorreo(dto.Correo);
                if (correoDuplicado && cliente.Correo != dto.Correo)
                    return Result<ClienteDTO>.Fail(ResultError.AlreadyExists("Cliente con este correo"));

                cliente.Nombre = dto.Nombre;
                cliente.Correo = dto.Correo;
                cliente.Telefono = dto.Telefono;
                cliente.Rol = (RolUsuario)Enum.Parse(typeof(RolUsuario), dto.Rol);

                if (!string.IsNullOrEmpty(dto.Contraseña))
                {
                    cliente.Contraseña = PasswordHelper.Encriptar(dto.Contraseña);
                }

                _clienteRepository.Update(cliente);
                return Result<ClienteDTO>.Ok(MapToDTO(cliente));
            }
            catch (Exception ex)
            {
                return Result<ClienteDTO>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<bool> Delete(int id)
        {
            try
            {
                var cliente = _clienteRepository.GetById(id);
                if (cliente == null || cliente.Borrado)
                    return Result<bool>.Fail(ResultError.NotFound("Cliente"));

                cliente.Borrado = true;
                _clienteRepository.Update(cliente);
                return Result<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<bool> ExisteCorreo(string correo)
        {
            try
            {
                return Result<bool>.Ok(_clienteRepository.ExisteCorreo(correo));
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        private ClienteDTO MapToDTO(Cliente cliente)
        {
            return new ClienteDTO
            {
                Id = cliente.Id,
                Nombre = cliente.Nombre,
                Correo = cliente.Correo,
                Telefono = cliente.Telefono,
                Rol = cliente.Rol.ToString(),
                FechaRegistro = cliente.FechaRegistro,
                Borrado = cliente.Borrado
            };
        }
    }
}
