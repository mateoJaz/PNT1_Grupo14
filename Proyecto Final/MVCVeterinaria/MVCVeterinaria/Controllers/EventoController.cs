using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCVeterinaria.Context;
using MVCVeterinaria.Models;
using Microsoft.AspNetCore.Authorization;

namespace MVCVeterinaria.Controllers
{
    [Authorize]
    public class EventoController : Controller
    {
        private readonly VeterinariaDatabaseContext _context;

        public EventoController(VeterinariaDatabaseContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var veterinariaDatabaseContext = _context.Evento.Include(e => e.Veterinario);
            return View(await veterinariaDatabaseContext.ToListAsync());
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = await _context.Evento
                .Include(e => e.Veterinario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (evento == null)
            {
                return NotFound();
            }

            return View(evento);
        }
        public async Task<IActionResult> Create(int? idMascota)
        {
            if (idMascota == null)
            {
                return RedirectToAction("Index", "Mascota");
            }

            var mascota = await _context.Mascota.FindAsync(idMascota);
            if (mascota == null)
            {
                return NotFound();
            }
            ViewBag.NombreMascota = mascota.Nombre;

            var veterinarios = _context.Veterinario.Select(v => new
            {
                Id = v.Id,
                NombreCompleto = v.Apellido + ", " + v.Nombre
            });
            ViewData["VeterinarioId"] = new SelectList(veterinarios, "Id", "NombreCompleto");

            var listaTipos = new List<string> { "Consulta General", "Vacunación", "Cirugía", "Estudio", "Urgencia", "Control" };
            ViewData["TiposEvento"] = new SelectList(listaTipos);

            var evento = new Evento
            {
                FechaHorario = DateTime.Now,
                MascotaId = idMascota.Value
            };

            return View(evento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TipoEvento,Detalle,FechaHorario,MascotaId,VeterinarioId")] Evento evento)
        {
            ModelState.Remove("Mascota");
            ModelState.Remove("Veterinario");
            if (ModelState.IsValid)
            {
                _context.Add(evento);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Mascota", new { id = evento.MascotaId });
            }
            ViewData["VeterinarioId"] = new SelectList(_context.Veterinario, "Id", "Id", evento.VeterinarioId);
            return View(evento);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = await _context.Evento.FindAsync(id);
            if (evento == null)
            {
                return NotFound();
            }
            ViewData["VeterinarioId"] = new SelectList(_context.Veterinario, "Id", "Id", evento.VeterinarioId);
            return View(evento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TipoEvento,Detalle,FechaHorario,MascotaId,VeterinarioId")] Evento evento)
        {
            if (id != evento.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(evento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventoExists(evento.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["VeterinarioId"] = new SelectList(_context.Veterinario, "Id", "Id", evento.VeterinarioId);
            return View(evento);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = await _context.Evento
                .Include(e => e.Veterinario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (evento == null)
            {
                return NotFound();
            }

            return View(evento);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var evento = await _context.Evento.FindAsync(id);
            if (evento != null)
            {
                _context.Evento.Remove(evento);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventoExists(int id)
        {
            return _context.Evento.Any(e => e.Id == id);
        }
    }
}
