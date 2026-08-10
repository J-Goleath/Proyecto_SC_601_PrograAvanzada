using AutoFix.Application.DTOs;
using AutoFix.Application.Common;

namespace AutoFix.Application.Interfaces
{
    public interface IExpedienteVehiculoService
    {
        Result<ExpedienteVehiculoDTO> GetExpedienteByVehiculo(int vehiculoId);
        Result<ExpedienteVehiculoDTO> GetExpedienteByPlaca(string placa);
    }
}