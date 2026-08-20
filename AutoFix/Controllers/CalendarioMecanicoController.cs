using AutoFix.Application.DTOs;
using AutoFix.Application.Interfaces;
using AutoFix.Domain.Interfaces.Repositories;
using AutoFix.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AutoFix.Controllers
{
    [RoutePrefix("CalendarioMecanico")]
    [CustomAuthorize(Roles = "Mecanico")]
    public class CalendarioMecanicoController : Controller
    {
        private readonly ICitaSolicitudRepository _citaRepository;
        private readonly IOrdenTrabajoRepository _ordenTrabajoRepository;
        private readonly IOrdenTrabajoService _ordenTrabajoService;

        private static readonly int[] HorasVisibles = { 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };

        public CalendarioMecanicoController(
            ICitaSolicitudRepository citaRepository,
            IOrdenTrabajoRepository ordenTrabajoRepository,
            IOrdenTrabajoService ordenTrabajoService)
        {
            _citaRepository = citaRepository;
            _ordenTrabajoRepository = ordenTrabajoRepository;
            _ordenTrabajoService = ordenTrabajoService;
        }

        private int MecanicoId
        {
            get { return Convert.ToInt32(Session["UsuarioId"]); }
        }

        [HttpGet]
        public ActionResult Index(DateTime? fecha)
        {
            var referencia = fecha ?? DateTime.Today;
            var inicioSemana = InicioDeSemana(referencia);
            var finSemana = inicioSemana.AddDays(4); // lunes a viernes

            var citasSemana = _citaRepository
                .GetCitasByMecanicoYSemana(MecanicoId, inicioSemana, finSemana)
                .ToList();

            var todasLasCitas = _citaRepository.GetCitasByMecanico(MecanicoId).ToList();
            var estados = ObtenerEstados(todasLasCitas.Select(c => c.Id));

            var dias = new List<DateTime>();
            for (int i = 0; i < 5; i++)
            {
                dias.Add(inicioSemana.AddDays(i));
            }

            ViewBag.Dias = dias;
            ViewBag.Horas = HorasVisibles;
            ViewBag.InicioSemana = inicioSemana;
            ViewBag.FinSemana = finSemana;
            ViewBag.SemanaAnterior = inicioSemana.AddDays(-7);
            ViewBag.SemanaSiguiente = inicioSemana.AddDays(7);
            ViewBag.Estados = estados;

            ViewBag.TotalPendientes = todasLasCitas.Count(c => estados[c.Id] == "Pendiente");
            ViewBag.TotalEnProgreso = todasLasCitas.Count(c => estados[c.Id] == "EnProgreso");
            ViewBag.TotalCompletadas = todasLasCitas.Count(c => estados[c.Id] == "Completado");

            ViewBag.ProximaCita = todasLasCitas
                .Where(c => c.Fecha.Date >= DateTime.Today)
                .OrderBy(c => c.Fecha).ThenBy(c => c.Hora)
                .FirstOrDefault();

            return View(citasSemana);
        }

        [HttpGet]
        public ActionResult Listado()
        {
            var citas = _citaRepository.GetCitasByMecanico(MecanicoId).ToList();
            ViewBag.Estados = ObtenerEstados(citas.Select(c => c.Id));
            return View(citas);
        }

        [HttpGet]
        public ActionResult Detalle(int id)
        {
            var cita = _citaRepository.GetById(id);
            if (cita == null || cita.Borrado || cita.MecanicoId != MecanicoId)
            {
                TempData["MensajeError"] = "La cita no existe o no está asignada a usted";
                return RedirectToAction(nameof(Index));
            }

            var orden = _ordenTrabajoRepository.ObtenerTodas()
                .Where(o => o.CitaSolicitudId == id)
                .OrderByDescending(o => o.FechaAsignacion)
                .FirstOrDefault();

            ViewBag.Orden = orden;
            ViewBag.Estado = (orden == null || string.IsNullOrWhiteSpace(orden.Estado)) ? "Pendiente" : orden.Estado;

            return View(cita);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearOrden(int citaId, string descripcionTrabajo, int prioridad)
        {
            var cita = _citaRepository.GetById(citaId);
            if (cita == null || cita.Borrado || cita.MecanicoId != MecanicoId)
            {
                TempData["MensajeError"] = "La cita no existe o no está asignada a usted";
                return RedirectToAction(nameof(Index));
            }

            if (cita.Vehiculo == null)
            {
                TempData["MensajeError"] = "No se pudo determinar el cliente de esta cita";
                return RedirectToAction(nameof(Detalle), new { id = citaId });
            }

            var dto = new CreateOrdenTrabajoDTO
            {
                CitaSolicitudId = cita.Id,
                ClienteId = cita.Vehiculo.ClienteId,
                MecanicoId = MecanicoId,
                DescripcionTrabajo = descripcionTrabajo,
                Prioridad = prioridad
            };

            var resultado = _ordenTrabajoService.Create(dto);
            if (!resultado.Success)
            {
                TempData["MensajeError"] = resultado.Error;
            }
            else
            {
                TempData["MensajeExito"] = "Orden de trabajo creada correctamente.";
            }

            return RedirectToAction(nameof(Detalle), new { id = citaId });
        }

        private Dictionary<int, string> ObtenerEstados(IEnumerable<int> citaIds)
        {
            var ids = citaIds.ToList();

            var ordenesPorCita = _ordenTrabajoRepository.ObtenerTodas()
                .Where(o => ids.Contains(o.CitaSolicitudId))
                .ToList()
                .GroupBy(o => o.CitaSolicitudId)
                .ToDictionary(g => g.Key, g => g.OrderByDescending(o => o.FechaAsignacion).First().Estado);

            var resultado = new Dictionary<int, string>();
            foreach (var id in ids)
            {
                string estado;
                if (ordenesPorCita.TryGetValue(id, out estado) && !string.IsNullOrWhiteSpace(estado))
                {
                    resultado[id] = estado;
                }
                else
                {
                    resultado[id] = "Pendiente";
                }
            }
            return resultado;
        }

        private DateTime InicioDeSemana(DateTime fecha)
        {
            int dow = fecha.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)fecha.DayOfWeek;
            return fecha.Date.AddDays(1 - dow); // retrocede hasta el lunes de esa semana
        }
    }
}
