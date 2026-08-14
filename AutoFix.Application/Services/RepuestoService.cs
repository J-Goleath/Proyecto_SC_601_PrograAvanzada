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
    public class RepuestoService : IRepuestoService
    {
        private readonly IRepuestoRepository _repuestoRepository;

        public RepuestoService(IRepuestoRepository repuestoRepository)
        {
            _repuestoRepository = repuestoRepository;
        }

        public Result<RepuestoDTO> GetById(int id)
        {
            try
            {
                var repuesto = _repuestoRepository.GetById(id);
                if (repuesto == null || repuesto.Borrado)
                    return Result<RepuestoDTO>.Fail(ResultError.NotFound("Repuesto"));

                return Result<RepuestoDTO>.Ok(MapToDTO(repuesto));
            }
            catch (Exception ex)
            {
                return Result<RepuestoDTO>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<List<RepuestoDTO>> GetAll()
        {
            try
            {
                var repuestos = _repuestoRepository.GetAll()
                    .Where(r => !r.Borrado)
                    .OrderBy(r => r.Nombre)
                    .ToList();

                return Result<List<RepuestoDTO>>.Ok(repuestos.Select(MapToDTO).ToList());
            }
            catch (Exception ex)
            {
                return Result<List<RepuestoDTO>>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<List<RepuestoDTO>> GetDisponibles()
        {
            try
            {
                var repuestos = _repuestoRepository.GetRepuestosDisponibles().ToList();
                return Result<List<RepuestoDTO>>.Ok(repuestos.Select(MapToDTO).ToList());
            }
            catch (Exception ex)
            {
                return Result<List<RepuestoDTO>>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<List<RepuestoDTO>> GetByCategoria(string categoria)
        {
            try
            {
                var repuestos = _repuestoRepository.GetRepuestosByCategoria(categoria).ToList();
                return Result<List<RepuestoDTO>>.Ok(repuestos.Select(MapToDTO).ToList());
            }
            catch (Exception ex)
            {
                return Result<List<RepuestoDTO>>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<RepuestoDTO> GetByCodigo(string codigo)
        {
            try
            {
                var repuesto = _repuestoRepository.GetByCodigo(codigo);
                if (repuesto == null)
                    return Result<RepuestoDTO>.Fail(ResultError.NotFound("Repuesto"));

                return Result<RepuestoDTO>.Ok(MapToDTO(repuesto));
            }
            catch (Exception ex)
            {
                return Result<RepuestoDTO>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<RepuestoDTO> Create(CreateRepuestoDTO dto)
        {
            try
            {
                var existe = _repuestoRepository.GetByCodigo(dto.Codigo) != null;
                if (existe)
                    return Result<RepuestoDTO>.Fail(ResultError.AlreadyExists("Repuesto con este código"));

                var repuesto = new Repuesto
                {
                    Nombre = dto.Nombre,
                    Codigo = dto.Codigo,
                    Descripcion = dto.Descripcion,
                    Stock = dto.Stock,
                    Precio = dto.Precio,
                    Categoria = dto.Categoria,
                    Ubicacion = dto.Ubicacion,
                    FechaRegistro = DateTime.Now,
                    Borrado = false
                };

                _repuestoRepository.Add(repuesto);
                return Result<RepuestoDTO>.Ok(MapToDTO(repuesto));
            }
            catch (Exception ex)
            {
                var detalle = ex.InnerException?.InnerException?.Message ?? ex.InnerException?.Message ?? ex.Message;
                return Result<RepuestoDTO>.Fail(ResultError.InternalError(detalle));
            }
        }

        public Result<RepuestoDTO> Update(UpdateRepuestoDTO dto)
        {
            try
            {
                var repuesto = _repuestoRepository.GetById(dto.Id);
                if (repuesto == null || repuesto.Borrado)
                    return Result<RepuestoDTO>.Fail(ResultError.NotFound("Repuesto"));

                // Validar que el código no esté en uso por otro repuesto
                var existente = _repuestoRepository.GetAll()
                    .FirstOrDefault(r => r.Codigo == dto.Codigo && r.Id != dto.Id && !r.Borrado);

                if (existente != null)
                    return Result<RepuestoDTO>.Fail(ResultError.AlreadyExists("Repuesto con este código"));

                repuesto.Nombre = dto.Nombre;
                repuesto.Codigo = dto.Codigo;
                repuesto.Descripcion = dto.Descripcion;
                repuesto.Stock = dto.Stock;
                repuesto.Precio = dto.Precio;
                repuesto.Categoria = dto.Categoria;
                repuesto.Ubicacion = dto.Ubicacion;

                _repuestoRepository.Update(repuesto);
                return Result<RepuestoDTO>.Ok(MapToDTO(repuesto));
            }
            catch (Exception ex)
            {
                return Result<RepuestoDTO>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<bool> Delete(int id)
        {
            try
            {
                var repuesto = _repuestoRepository.GetById(id);
                if (repuesto == null || repuesto.Borrado)
                    return Result<bool>.Fail(ResultError.NotFound("Repuesto"));

                repuesto.Borrado = true;
                _repuestoRepository.Update(repuesto);
                return Result<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<bool> UpdateStock(int repuestoId, int cantidad)
        {
            try
            {
                var repuesto = _repuestoRepository.GetById(repuestoId);
                if (repuesto == null || repuesto.Borrado)
                    return Result<bool>.Fail(ResultError.NotFound("Repuesto"));

                if (repuesto.Stock < cantidad)
                    return Result<bool>.Fail(ResultError.InsufficientStock(repuesto.Stock, cantidad));

                repuesto.Stock -= cantidad;
                _repuestoRepository.Update(repuesto);
                return Result<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<bool> ValidateStock(int repuestoId, int cantidadSolicitada)
        {
            try
            {
                var repuesto = _repuestoRepository.GetById(repuestoId);
                if (repuesto == null || repuesto.Borrado)
                    return Result<bool>.Fail(ResultError.NotFound("Repuesto"));

                if (repuesto.Stock < cantidadSolicitada)
                    return Result<bool>.Fail(ResultError.InsufficientStock(repuesto.Stock, cantidadSolicitada));

                return Result<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        private RepuestoDTO MapToDTO(Repuesto repuesto)
        {
            return new RepuestoDTO
            {
                Id = repuesto.Id,
                Nombre = repuesto.Nombre,
                Codigo = repuesto.Codigo,
                Descripcion = repuesto.Descripcion,
                Stock = repuesto.Stock,
                Precio = repuesto.Precio,
                Categoria = repuesto.Categoria,
                Ubicacion = repuesto.Ubicacion,
                FechaRegistro = repuesto.FechaRegistro,
                Borrado = repuesto.Borrado
            };
        }
    }
}