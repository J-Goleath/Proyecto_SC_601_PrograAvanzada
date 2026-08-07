using AutoFix.Entities;
using AutoFix.infraestructure.DBContext;
using AutoFix.infraestructure.Repositories;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AutoFix.Controllers
{
    [RoutePrefix("api/repuestos")]
    public class RepuestosApiController : ApiController
    {
        private readonly IRepuestoRepository _repuestoRepository;
        private readonly AutoFixContext _context;

        public RepuestosApiController()
        {
            _context = new AutoFixContext();
            _repuestoRepository = new RepuestoRepository(_context);
        }

        // GET api/repuestos
        // Devuelve todos los repuestos activos (no borrados)
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll()
        {
            var repuestos = _repuestoRepository.GetAll().Where(r => !r.Borrado).ToList();
            return Ok(repuestos);
        }

        // GET api/repuestos/5
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            var repuesto = _repuestoRepository.GetById(id);

            if (repuesto == null || repuesto.Borrado)
            {
                return NotFound();
            }

            return Ok(repuesto);
        }

        // POST api/repuestos
        // Crea un nuevo repuesto
        [HttpPost]
        [Route("")]
        public IHttpActionResult Create([FromBody] Repuesto repuesto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (repuesto == null)
            {
                return BadRequest("El cuerpo de la solicitud no puede estar vacío.");
            }

            // Evitar códigos duplicados
            var existente = _repuestoRepository.GetByCodigo(repuesto.Codigo);
            if (existente != null)
            {
                return BadRequest($"Ya existe un repuesto con el código '{repuesto.Codigo}'.");
            }

            repuesto.FechaRegistro = System.DateTime.Now;
            repuesto.Borrado = false;

            _repuestoRepository.Add(repuesto);

            return Content(HttpStatusCode.Created, repuesto);
        }

        // PUT api/repuestos/5
        // Actualiza un repuesto existente
        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult Update(int id, [FromBody] Repuesto repuesto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (repuesto == null)
            {
                return BadRequest("El cuerpo de la solicitud no puede estar vacío.");
            }

            var existente = _repuestoRepository.GetById(id);
            if (existente == null || existente.Borrado)
            {
                return NotFound();
            }

            existente.Nombre = repuesto.Nombre;
            existente.Codigo = repuesto.Codigo;
            existente.Descripcion = repuesto.Descripcion;
            existente.Stock = repuesto.Stock;
            existente.Precio = repuesto.Precio;
            existente.Categoria = repuesto.Categoria;
            existente.Ubicacion = repuesto.Ubicacion;

            _repuestoRepository.Update(existente);

            return Ok(existente);
        }

        // DELETE api/repuestos/5
        // Borrado lógico (marca Borrado = true, no elimina físicamente)
        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            var existente = _repuestoRepository.GetById(id);
            if (existente == null || existente.Borrado)
            {
                return NotFound();
            }

            existente.Borrado = true;
            _repuestoRepository.Update(existente);

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
