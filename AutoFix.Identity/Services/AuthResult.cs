namespace AutoFix.Identity.Services
{
    public class AuthResult
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public int ClienteId { get; set; }
        public string NombreCompleto { get; set; }
        public string Rol { get; set; }

        public static AuthResult Ok(int clienteId, string nombre, string rol)
        {
            return new AuthResult { Success = true, ClienteId = clienteId, NombreCompleto = nombre, Rol = rol };
        }

        public static AuthResult Fail(string error)
        {
            return new AuthResult { Success = false, Error = error };
        }
    }
}
