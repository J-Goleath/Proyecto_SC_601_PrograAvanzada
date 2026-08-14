using System.Web.Mvc;
using Unity;
using Unity.Lifetime;
using Unity.Mvc5;

using AutoFix.Domain.Interfaces.Repositories;
using AutoFix.infraestructure.DBContext;
using AutoFix.infraestructure.Repositories;

using AutoFix.Application.Interfaces;
using AutoFix.Application.Services;

namespace AutoFix
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // ---- DbContext ----
            // HierarchicalLifetimeManager: una instancia por cada request HTTP
            // (Unity.Mvc5 crea un child container por request automáticamente).
            container.RegisterType<AutoFixContext>(new HierarchicalLifetimeManager());

            // ---- Repositories ----
            container.RegisterType<IVehiculoRepository, VehiculoRepository>();
            container.RegisterType<IClienteRepository, ClienteRepository>();
            container.RegisterType<ICitaSolicitudRepository, CitaSolicitudRepository>();
            container.RegisterType<INotificacionRepository, NotificacionRepository>();
            container.RegisterType<IOrdenTrabajoRepository, OrdenTrabajoRepository>();
            container.RegisterType<IRepuestoRepository, RepuestoRepository>();
            container.RegisterType<IMaterialUsadoRepository, MaterialUsadoRepository>();
            container.RegisterType<IExpedienteVehiculoRepository, ExpedienteVehiculoRepository>();

            // ---- Services (capa Application) ----
            container.RegisterType<IVehiculoService, VehiculoService>();
            container.RegisterType<IClienteService, ClienteService>();
            container.RegisterType<ICitaService, CitaService>();
            container.RegisterType<INotificacionService, NotificacionService>();
            container.RegisterType<IOrdenTrabajoService, OrdenTrabajoService>();
            container.RegisterType<IRepuestoService, RepuestoService>();
            container.RegisterType<IMaterialUsadoService, MaterialUsadoService>();
            container.RegisterType<IExpedienteVehiculoService, ExpedienteVehiculoService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}