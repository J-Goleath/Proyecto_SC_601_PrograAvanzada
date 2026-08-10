using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFix.Application.Common
{
    public static class ErrorCodes
    {
        // Errores de dominio
        public const string ENTITY_NOT_FOUND = "DOM_001";
        public const string ENTITY_ALREADY_EXISTS = "DOM_002";
        public const string INVALID_STATE = "DOM_003";

        // Errores de validación
        public const string VALIDATION_ERROR = "VAL_001";
        public const string REQUIRED_FIELD = "VAL_002";
        public const string INVALID_FORMAT = "VAL_003";

        // Errores de negocio
        public const string INSUFFICIENT_STOCK = "BUS_001";
        public const string INVALID_OPERATION = "BUS_002";

        // Errores de autenticación
        public const string UNAUTHORIZED = "AUTH_001";
        public const string INVALID_CREDENTIALS = "AUTH_002";

        // Errores del sistema
        public const string INTERNAL_ERROR = "SYS_001";
        public const string DATABASE_ERROR = "SYS_002";
    }
}
