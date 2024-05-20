using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend_crud.Context;
using backend_crud.Models; 

[ApiController]
[Route("api/[controller]")]
public class ReservasController : ControllerBase
{
    private readonly AppDbContext _context;

    public ReservasController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]//Devuelve las reservas existentes
    public async Task<ActionResult<IEnumerable<Reserva>>> GetReservas()
    {
        return await _context.Reservas.Include(r => r.SalaDeJuntas).ToListAsync();
    }

    [HttpPost]//Crear una reserva nueva.
    public async Task<IActionResult> CrearReserva([FromBody] Reserva reserva)
    {
        var reservasSuperpuestas = await _context.Reservas
            .AnyAsync(r => r.SalaDeJuntasId == reserva.SalaDeJuntasId &&
                           r.HoraInicio < reserva.HoraFin &&
                           r.HoraFin > reserva.HoraInicio);

        if (reservasSuperpuestas)
        {
            return BadRequest("La sala ya está reservada en este horario.");
        }

        if ((reserva.HoraFin - reserva.HoraInicio).TotalHours > 2)
        {
            return BadRequest("No se puede reservar por más de 2 horas.");
        }

        _context.Reservas.Add(reserva);
        await _context.SaveChangesAsync();

        return Ok(reserva);
    }

    [HttpDelete("{id}")] //Liberar Reserva.
    public async Task<IActionResult> LiberarReserva(int id)
    {
        var reserva = await _context.Reservas.FindAsync(id);
        if (reserva == null)
        {
            return NotFound();
        }

        _context.Reservas.Remove(reserva);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}