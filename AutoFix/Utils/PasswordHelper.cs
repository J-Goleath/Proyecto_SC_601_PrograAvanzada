using System;
using System.Security.Cryptography;
using System.Text;

namespace AutoFix.Utils
{
    public static class PasswordHelper
    {
        /// <summary>
        /// Encripta una contraseña usando SHA256
        /// </summary>
        public static string Encriptar(string contraseña)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(contraseña));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// Verifica si la contraseña ingresada coincide con el hash almacenado
        /// </summary>
        public static bool Verificar(string contraseñaIngresada, string hashAlmacenado)
        {
            string hashIngresado = Encriptar(contraseñaIngresada);
            return hashIngresado == hashAlmacenado;
        }
    }
}