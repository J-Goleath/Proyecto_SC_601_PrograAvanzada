using AutoFix.Application.DTOs;
using AutoFix.Application.Common;
using System;
using System.Collections.Generic;

namespace AutoFix.Application.Interfaces
{
    public interface IMaterialUsadoService
    {
        Result<MaterialUsadoDTO> GetById(int id);
        Result<List<MaterialUsadoDTO>> GetAll();
        Result<List<MaterialUsadoDTO>> GetByOrdenTrabajo(int ordenTrabajoId);
        Result<List<MaterialUsadoDTO>> GetByRepuesto(int repuestoId);
        Result<List<MaterialUsadoDTO>> GetByFecha(DateTime fechaInicio, DateTime fechaFin);
        Result<MaterialUsadoDTO> SolicitarMaterial(SolicitarMaterialDTO dto);
        Result<List<MaterialUsadoHistorialDTO>> GetHistorial();
        Result<bool> Delete(int id);
    }
}
