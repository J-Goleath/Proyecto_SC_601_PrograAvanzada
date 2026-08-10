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
    public class MaterialUsadoService : IMaterialUsadoService
    {
        private readonly IMaterialUsadoRepository _materialUsadoRepository;
        private readonly IRepuestoRepository _repuestoRepository;
        private readonly IOrdenTrabajoRepository _ordenTrabajoRepository;

        public MaterialUsadoService(
            IMaterialUsadoRepository materialUsadoRepository,
            IRepuestoRepository repuestoRepository,
            IOrdenTrabajoRepository ordenTrabajoRepository)
        {
            _materialUsadoRepository = materialUsadoRepository;
            _repuestoRepository = repuestoRepository;
            _ordenTrabajoRepository = ordenTrabajoRepository;
        }

        public Result<MaterialUsadoDTO> GetById(int id)
        {
            try
            {
                var material = _materialUsadoRepository.GetById(id);
                if (material == null || material.Borrado)
                    return Result<MaterialUsadoDTO>.Fail(ResultError.NotFound("Material usado"));

                return Result<MaterialUsadoDTO>.Ok(MapToDTO(material));
            }
            catch (Exception ex)
            {
                return Result<MaterialUsadoDTO>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<List<MaterialUsadoDTO>> GetAll()
        {
            try
            {
                var materiales = _materialUsadoRepository.GetAll()
                    .Where(m => !m.Borrado)
                    .OrderByDescending(m => m.FechaUso)
                    .ToList();

                return Result<List<MaterialUsadoDTO>>.Ok(materiales.Select(MapToDTO).ToList());
            }
            catch (Exception ex)
            {
                return Result<List<MaterialUsadoDTO>>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<List<MaterialUsadoDTO>> GetByOrdenTrabajo(int ordenTrabajoId)
        {
            try
            {
                var materiales = _materialUsadoRepository.GetMaterialesByOrdenTrabajo(ordenTrabajoId).ToList();
                return Result<List<MaterialUsadoDTO>>.Ok(materiales.Select(MapToDTO).ToList());
            }
            catch (Exception ex)
            {
                return Result<List<MaterialUsadoDTO>>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<List<MaterialUsadoDTO>> GetByRepuesto(int repuestoId)
        {
            try
            {
                var materiales = _materialUsadoRepository.GetMaterialesByRepuesto(repuestoId).ToList();
                return Result<List<MaterialUsadoDTO>>.Ok(materiales.Select(MapToDTO).ToList());
            }
            catch (Exception ex)
            {
                return Result<List<MaterialUsadoDTO>>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<List<MaterialUsadoDTO>> GetByFecha(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                var materiales = _materialUsadoRepository.GetMaterialesByFecha(fechaInicio, fechaFin).ToList();
                return Result<List<MaterialUsadoDTO>>.Ok(materiales.Select(MapToDTO).ToList());
            }
            catch (Exception ex)
            {
                return Result<List<MaterialUsadoDTO>>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<MaterialUsadoDTO> SolicitarMaterial(SolicitarMaterialDTO dto)
        {
            try
            {
                // Validar repuesto
                var repuesto = _repuestoRepository.GetById(dto.RepuestoId);
                if (repuesto == null || repuesto.Borrado)
                    return Result<MaterialUsadoDTO>.Fail(ResultError.NotFound("Repuesto"));

                if (repuesto.Stock < dto.Cantidad)
                    return Result<MaterialUsadoDTO>.Fail(ResultError.InsufficientStock(repuesto.Stock, dto.Cantidad));

                // Validar orden de trabajo
                var ordenTrabajo = _ordenTrabajoRepository.ObtenerPorId(dto.OrdenTrabajoId);
                if (ordenTrabajo == null || ordenTrabajo.Borrado)
                    return Result<MaterialUsadoDTO>.Fail(ResultError.NotFound("Orden de trabajo"));

                if (ordenTrabajo.Estado == "Finalizada" || ordenTrabajo.Estado == "Cancelada")
                    return Result<MaterialUsadoDTO>.Fail(ResultError.InvalidOperation("La orden de trabajo está finalizada o cancelada"));

                // Descontar stock
                repuesto.Stock -= dto.Cantidad;
                _repuestoRepository.Update(repuesto);

                // Registrar uso
                var material = new MaterialUsado
                {
                    RepuestoId = dto.RepuestoId,
                    OrdenTrabajoId = dto.OrdenTrabajoId,
                    Cantidad = dto.Cantidad,
                    CostoUnitario = repuesto.Precio,
                    Observaciones = dto.Observaciones,
                    FechaUso = DateTime.Now,
                    Borrado = false
                };

                _materialUsadoRepository.Add(material);

                return Result<MaterialUsadoDTO>.Ok(MapToDTO(material));
            }
            catch (Exception ex)
            {
                return Result<MaterialUsadoDTO>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<List<MaterialUsadoHistorialDTO>> GetHistorial()
        {
            try
            {
                var materiales = _materialUsadoRepository.GetAll()
                    .Where(m => !m.Borrado)
                    .OrderByDescending(m => m.FechaUso)
                    .ToList();

                var historial = materiales.Select(m => new MaterialUsadoHistorialDTO
                {
                    Id = m.Id,
                    OrdenTrabajoId = m.OrdenTrabajoId,
                    RepuestoId = m.RepuestoId,
                    RepuestoNombre = m.Repuesto?.Nombre ?? "N/A",
                    Cantidad = m.Cantidad,
                    CostoUnitario = m.CostoUnitario,
                    Total = m.Cantidad * m.CostoUnitario,
                    FechaUso = m.FechaUso,
                    Observaciones = m.Observaciones,
                    OrdenTrabajoEstado = m.OrdenTrabajo?.Estado ?? "N/A"
                }).ToList();

                return Result<List<MaterialUsadoHistorialDTO>>.Ok(historial);
            }
            catch (Exception ex)
            {
                return Result<List<MaterialUsadoHistorialDTO>>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        public Result<bool> Delete(int id)
        {
            try
            {
                var material = _materialUsadoRepository.GetById(id);
                if (material == null || material.Borrado)
                    return Result<bool>.Fail(ResultError.NotFound("Material usado"));

                material.Borrado = true;
                _materialUsadoRepository.Update(material);
                return Result<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail(ResultError.InternalError(ex.Message));
            }
        }

        private MaterialUsadoDTO MapToDTO(MaterialUsado material)
        {
            return new MaterialUsadoDTO
            {
                Id = material.Id,
                OrdenTrabajoId = material.OrdenTrabajoId,
                RepuestoId = material.RepuestoId,
                RepuestoNombre = material.Repuesto?.Nombre ?? "N/A",
                RepuestoCodigo = material.Repuesto?.Codigo ?? "N/A",
                Cantidad = material.Cantidad,
                CostoUnitario = material.CostoUnitario,
                Observaciones = material.Observaciones,
                FechaUso = material.FechaUso,
                Borrado = material.Borrado
            };
        }
    }
}