using System;

namespace AutoFix.Domain.Common
{
    public abstract class EntidadBase
    {
        public int Id { get; protected set; }

        public DateTime FechaRegistro { get; protected set; }

        public bool Borrado { get; protected set; }

        protected EntidadBase()
        {
            FechaRegistro = DateTime.Now;
            Borrado = false;
        }

        public void MarcarComoEliminado()
        {
            Borrado = true;
        }

        public void Restaurar()
        {
            Borrado = false;
        }
    }
}
