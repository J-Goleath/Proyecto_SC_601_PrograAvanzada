using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFix.Application.Common
{
    public class ResultError
    {
        public string Code { get; set; }
        public string Message { get; set; }

        public ResultError(string code, string message)
        {
            Code = code;
            Message = message;
        }

        // Errores predefinidos
        public static ResultError NotFound(string entityName = "Entidad")
        {
            return new ResultError("NOT_FOUND", $"{entityName} no encontrada");
        }

        public static ResultError AlreadyExists(string entityName = "Entidad")
        {
            return new ResultError("ALREADY_EXISTS", $"{entityName} ya existe");
        }

        public static ResultError ValidationError(string message)
        {
            return new ResultError("VALIDATION_ERROR", message);
        }

        public static ResultError InternalError(string message = "Error interno del servidor")
        {
            return new ResultError("INTERNAL_ERROR", message);
        }

        public static ResultError Unauthorized(string message = "No autorizado")
        {
            return new ResultError("UNAUTHORIZED", message);
        }

        public static ResultError InsufficientStock(int available, int requested)
        {
            return new ResultError("INSUFFICIENT_STOCK", $"Stock insuficiente. Disponible: {available}, Solicitado: {requested}");
        }

        public static ResultError InvalidOperation(string message)
        {
            return new ResultError("INVALID_OPERATION", message);
        }

        public static ResultError InvalidCredentials()
        {
            return new ResultError("INVALID_CREDENTIALS", "Credenciales inválidas");
        }
    }
}