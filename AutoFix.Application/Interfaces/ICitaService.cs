using AutoFix.Application.DTOs;
using AutoFix.Application.Common;
using System;
using System.Collections.Generic;

namespace AutoFix.Application.Interfaces
{
    public interface ICitaService
    {
        Result<CitaDTO> GetById(int id);
        Result<List<CitaDTO>> GetAll();
        Result<List<CitaDTO>> GetByVehiculo(int vehiculoId);
        Result<List<CitaDTO>> GetByCliente(int clienteId);
        Result<List<CitaDTO>> GetByMecanico(int mecanicoId);
        Result<List<CitaDTO>> GetByMecanicoYSemana(int mecanicoId, DateTime inicioSemana, DateTime finSemana);
        Result<List<CitaDTO>> GetPendientes();
        Result<CitaDTO> Create(CreateCitaDTO dto);
        Result<CitaDTO> Update(UpdateCitaDTO dto);
        Result<bool> Delete(int id);
        Result<bool> ProcesarCita(int id);
    }
}