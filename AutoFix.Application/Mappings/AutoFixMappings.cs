using AutoFix.Application.DTOs;
using AutoFix.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFix.Application.Mappings
{
    public static class AutoFixMappings
    {
        // Cliente
        public static ClienteDTO ToDTO(this Cliente entity)
        {
            return new ClienteDTO
            {
                Id = entity.Id,
                Nombre = entity.Nombre,
                Correo = entity.Correo,
                Telefono = entity.Telefono,
                Rol = entity.Rol.ToString(),
                FechaRegistro = entity.FechaRegistro,
                Borrado = entity.Borrado
            };
        }

        // Vehiculo
        public static VehiculoDTO ToDTO(this Vehiculo entity)
        {
            return new VehiculoDTO
            {
                Id = entity.Id,
                Placa = entity.Placa,
                Marca = entity.Marca,
                Modelo = entity.Modelo,
                Anio = entity.Anio,
                Color = entity.Color,
                ClienteId = entity.ClienteId,
                ClienteNombre = entity.Cliente?.Nombre ?? "N/A",
                FechaRegistro = entity.FechaRegistro,
                Borrado = entity.Borrado
            };
        }

        // Cita
        public static CitaDTO ToDTO(this CitaSolicitud entity)
        {
            return new CitaDTO
            {
                Id = entity.Id,
                Fecha = entity.Fecha,
                Hora = entity.Hora,
                DescripcionFallos = entity.DescripcionFallos,
                Procesada = entity.Procesada,
                VehiculoId = entity.VehiculoId,
                VehiculoPlaca = entity.Vehiculo?.Placa ?? "N/A",
                MecanicoId = entity.MecanicoId,
                MecanicoNombre = entity.Mecanico?.Nombre ?? "No asignado",
                FechaRegistro = entity.FechaRegistro,
                Borrado = entity.Borrado
            };
        }

        // OrdenTrabajo
        public static OrdenTrabajoDTO ToDTO(this OrdenTrabajo entity)
        {
            return new OrdenTrabajoDTO
            {
                Id = entity.Id,
                CitaSolicitudId = entity.CitaSolicitudId,
                ClienteId = entity.ClienteId,
                ClienteNombre = entity.Cliente?.Nombre ?? "N/A",
                MecanicoId = entity.MecanicoId,
                MecanicoNombre = entity.Mecanico?.Nombre ?? "N/A",
                Estado = entity.Estado,
                DescripcionTrabajo = entity.DescripcionTrabajo,
                Diagnostico = entity.Diagnostico,
                Observaciones = entity.Observaciones,
                FechaAsignacion = entity.FechaAsignacion,
                FechaInicio = entity.FechaInicio,
                FechaFinalizacion = entity.FechaFinalizacion,
                Prioridad = entity.Prioridad,
                Borrado = entity.Borrado
            };
        }

        // Repuesto
        public static RepuestoDTO ToDTO(this Repuesto entity)
        {
            return new RepuestoDTO
            {
                Id = entity.Id,
                Nombre = entity.Nombre,
                Codigo = entity.Codigo,
                Descripcion = entity.Descripcion,
                Stock = entity.Stock,
                Precio = entity.Precio,
                Categoria = entity.Categoria,
                Ubicacion = entity.Ubicacion,
                FechaRegistro = entity.FechaRegistro,
                Borrado = entity.Borrado
            };
        }

        // MaterialUsado
        public static MaterialUsadoDTO ToDTO(this MaterialUsado entity)
        {
            return new MaterialUsadoDTO
            {
                Id = entity.Id,
                OrdenTrabajoId = entity.OrdenTrabajoId,
                RepuestoId = entity.RepuestoId,
                RepuestoNombre = entity.Repuesto?.Nombre ?? "N/A",
                RepuestoCodigo = entity.Repuesto?.Codigo ?? "N/A",
                Cantidad = entity.Cantidad,
                CostoUnitario = entity.CostoUnitario,
                Observaciones = entity.Observaciones,
                FechaUso = entity.FechaUso,
                Borrado = entity.Borrado
            };
        }

        // Notificacion
        public static NotificacionDTO ToDTO(this Notificacion entity)
        {
            return new NotificacionDTO
            {
                Id = entity.Id,
                Mensaje = entity.Mensaje,
                FechaEnvio = entity.FechaEnvio,
                Leida = entity.Leida,
                ClienteId = entity.ClienteId,
                ClienteNombre = entity.Cliente?.Nombre ?? "N/A",
                Borrado = entity.Borrado
            };
        }
    }
}