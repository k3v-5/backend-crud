using backend_crud.Context;
using backend_crud.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalaDeJuntasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalaDeJuntasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SalaDeJuntasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]//Devuelve las salas de juntas.
        public async Task<ActionResult<IEnumerable<SalaDeJuntas>>> GetSalaDeJuntas()
        {
            return await _context.SalaDeJuntas.ToListAsync();
        }

        [HttpGet("{id}")]//Devolver una sala de juntas basada en ID.
        public async Task<ActionResult<SalaDeJuntas>> GetSalaDeJuntas(int id)
        {
            var salaDeJuntas = await _context.SalaDeJuntas.FindAsync(id);
            if (salaDeJuntas == null)
            {
                return NotFound();
            }
            return salaDeJuntas;
        }

        [HttpPost]//crear una reserva para una sala de juntas específica.
        public async Task<IActionResult> CrearReserva([FromBody] Reserva reserva)
        {
            var reservasSuperpuestas = await _context.Reservas
                .AnyAsync(r => r.SalaDeJuntasId == reserva.SalaDeJuntasId &&
                               r.HoraInicio < reserva.HoraFin &&
                               r.HoraFin > reserva.HoraInicio);

            if (reservasSuperpuestas)
            {
                return BadRequest(new { message = "La sala ya está reservada en este horario." });
            }

            if ((reserva.HoraFin - reserva.HoraInicio).TotalHours > 2)
            {
                return BadRequest(new { message = "No se puede reservar por más de 2 horas." });
            }

            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            return Ok(reserva);
        }

        [HttpPut("{id}")]//actualizar una sala de juntas existente.
        public async Task<IActionResult> UpdateSalaDeJuntas(int id, SalaDeJuntas salaDeJuntas)
        {
            if (id != salaDeJuntas.Id)
            {
                return BadRequest();
            }

            _context.Entry(salaDeJuntas).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]//eliminar una sala de juntas existente.
        public async Task<IActionResult> DeleteSalaDeJuntas(int id)
        {
            var salaDeJuntas = await _context.SalaDeJuntas.FindAsync(id);
            if (salaDeJuntas == null)
            {
                return NotFound();
            }

            _context.SalaDeJuntas.Remove(salaDeJuntas);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
