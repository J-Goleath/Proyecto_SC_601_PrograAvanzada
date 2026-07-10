using AutoFix.Filters;
using AutoFix.infraestructure.DBContext;
using AutoFix.infraestructure.Repositories;
using System;
using System.Web.Mvc;

namespace AutoFix.Controllers
{
    [RoutePrefix("CalendarioMecanico")]
    [CustomAuthorize(Roles = "Mecanico")]
    public class CalendarioMecanicoController : Controller
    {
        private readonly ICitaSolicitudRepository _citaRepository;
        private readonly AutoFixContext _context;

        public CalendarioMecanicoController()
        {
            _context = new AutoFixContext();
            _citaRepository = new CitaSolicitudRepository(_context);
        }

        private int MecanicoId
        {
            get { return Convert.ToInt32(Session["UsuarioId"]); }
        }

        [HttpGet]
        public ActionResult Index()
        {
            var citas = _citaRepository.GetCitasByMecanico(MecanicoId);
            return View(citas);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
