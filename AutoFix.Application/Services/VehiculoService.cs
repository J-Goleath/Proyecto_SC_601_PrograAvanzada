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
    public class VehiculoService : IVehiculoService
    {
        private readonly IVehiculoRepository _vehiculoRepository;
        private readonly IClienteRepository _clienteRepository;

        public VehiculoService(IVehiculoRepository vehiculoRepository, IClienteRepository clienteRepository)
        {
            _vehiculoRepository = vehiculoRepository;
            _clienteRepository = clienteRepository;
        }

        public Result<VehiculoDTO> GetById(int id)
        {
            try
            {
                var vehiculo = _vehiculoRepository.GetById(id);
                if (vehiculo == null || vehiculo.Borrado)
                    return Result<VehiculoDTO>.Fail(ResultError.NotFound("Vehículo"));

                return Result<VehiculoDTO>.Ok(MapToDTO(vehiculo));
            }
            catch (Exception ex)
            {
                return Result<VehiculoDTO>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<List<VehiculoDTO>> GetAll()
        {
            try
            {
                var vehiculos = _vehiculoRepository.GetAll()
                    .Where(v => !v.Borrado)
                    .OrderBy(v => v.Placa)
                    .ToList();

                return Result<List<VehiculoDTO>>.Ok(vehiculos.Select(MapToDTO).ToList());
            }
            catch (Exception ex)
            {
                return Result<List<VehiculoDTO>>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<List<VehiculoDTO>> GetByCliente(int clienteId)
        {
            try
            {
                var vehiculos = _vehiculoRepository.GetVehiculosByCliente(clienteId).ToList();
                return Result<List<VehiculoDTO>>.Ok(vehiculos.Select(MapToDTO).ToList());
            }
            catch (Exception ex)
            {
                return Result<List<VehiculoDTO>>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<VehiculoDTO> GetByPlaca(string placa)
        {
            try
            {
                var vehiculo = _vehiculoRepository.GetByPlaca(placa);
                if (vehiculo == null)
                    return Result<VehiculoDTO>.Fail(ResultError.NotFound("Vehículo"));

                return Result<VehiculoDTO>.Ok(MapToDTO(vehiculo));
            }
            catch (Exception ex)
            {
                return Result<VehiculoDTO>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<VehiculoDTO> Create(CreateVehiculoDTO dto)
        {
            try
            {
                if (_vehiculoRepository.ExistePlaca(dto.Placa))
                    return Result<VehiculoDTO>.Fail(ResultError.AlreadyExists("Vehículo con esta placa"));

                var cliente = _clienteRepository.GetById(dto.ClienteId);
                if (cliente == null || cliente.Borrado)
                    return Result<VehiculoDTO>.Fail(ResultError.NotFound("Cliente"));

                var vehiculo = new Vehiculo
                {
                    Placa = dto.Placa.ToUpper(),
                    Marca = dto.Marca,
                    Modelo = dto.Modelo,
                    Anio = dto.Anio,
                    Color = dto.Color,
                    ClienteId = dto.ClienteId,
                    FechaRegistro = DateTime.Now,
                    Borrado = false
                };

                _vehiculoRepository.Add(vehiculo);
                return Result<VehiculoDTO>.Ok(MapToDTO(vehiculo));
            }
            catch (Exception ex)
            {
                return Result<VehiculoDTO>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<VehiculoDTO> Update(UpdateVehiculoDTO dto)
        {
            try
            {
                var vehiculo = _vehiculoRepository.GetById(dto.Id);
                if (vehiculo == null || vehiculo.Borrado)
                    return Result<VehiculoDTO>.Fail(ResultError.NotFound("Vehículo"));

                var existePlaca = _vehiculoRepository.GetAll()
                    .Any(v => v.Placa == dto.Placa.ToUpper() && v.Id != dto.Id && !v.Borrado);

                if (existePlaca)
                    return Result<VehiculoDTO>.Fail(ResultError.AlreadyExists("Vehículo con esta placa"));

                vehiculo.Placa = dto.Placa.ToUpper();
                vehiculo.Marca = dto.Marca;
                vehiculo.Modelo = dto.Modelo;
                vehiculo.Anio = dto.Anio;
                vehiculo.Color = dto.Color;
                vehiculo.ClienteId = dto.ClienteId;

                _vehiculoRepository.Update(vehiculo);
                return Result<VehiculoDTO>.Ok(MapToDTO(vehiculo));
            }
            catch (Exception ex)
            {
                return Result<VehiculoDTO>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<bool> Delete(int id)
        {
            try
            {
                var vehiculo = _vehiculoRepository.GetById(id);
                if (vehiculo == null || vehiculo.Borrado)
                    return Result<bool>.Fail(ResultError.NotFound("Vehículo"));

                vehiculo.Borrado = true;
                _vehiculoRepository.Update(vehiculo);
                return Result<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<bool> ExistePlaca(string placa)
        {
            try
            {
                return Result<bool>.Ok(_vehiculoRepository.ExistePlaca(placa));
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        private VehiculoDTO MapToDTO(Vehiculo vehiculo)
        {
            return new VehiculoDTO
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
            };
        }
    }
}