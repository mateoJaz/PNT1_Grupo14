using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCVeterinaria.Context;
using MVCVeterinaria.Models;
using MVCVeterinaria.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace MVCVeterinaria.Controllers
{
    [Authorize]
    public class TurnoController : Controller
    {
        private readonly VeterinariaDatabaseContext _context;
        private readonly TurnoService _turnoService;

        public TurnoController(VeterinariaDatabaseContext context, TurnoService turnoService)
        {
            _context = context;
            _turnoService = turnoService;
        }
        public async Task<IActionResult> Index()
        {
            var veterinariaDatabaseContext = _context.Turno.Include(t => t.Mascota).Include(t => t.Veterinario);
            return View(await veterinariaDatabaseContext.ToListAsync());
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turno = await _context.Turno
                .Include(t => t.Mascota)
                .Include(t => t.Veterinario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (turno == null)
            {
                return NotFound();
            }

            return View(turno);
        }
        public async Task<IActionResult> Create(int? mascotaId)
        {

            if (mascotaId == null)
            {

                return RedirectToAction("Index", "Cliente");
            }


            bool mascotaExiste = await _context.Mascota.AnyAsync(m => m.Id == mascotaId.Value);

            if (!mascotaExiste)
            {
                return RedirectToAction("Index", "Cliente");
            }

            var model = new TurnoSearchViewModel { MascotaId = mascotaId.Value };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MascotaId,VeterinarioId,FechaHorario,Detalle")] Turno turno)
        {
            ModelState.Remove("Turnos");
            if (ModelState.IsValid)
            {
                _context.Add(turno);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MascotaId"] = new SelectList(_context.Mascota, "Id", "Id", turno.MascotaId);
            ViewData["VeterinarioId"] = new SelectList(_context.Veterinario, "Id", "Id", turno.VeterinarioId);
            return View(turno);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turno = await _context.Turno.FindAsync(id);
            if (turno == null)
            {
                return NotFound();
            }
            ViewData["MascotaId"] = new SelectList(_context.Mascota, "Id", "Id", turno.MascotaId);
            ViewData["VeterinarioId"] = new SelectList(_context.Veterinario, "Id", "Id", turno.VeterinarioId);
            return View(turno);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MascotaId,VeterinarioId,FechaHorario,Detalle")] Turno turno)
        {
            if (id != turno.Id)
            {
                return NotFound();
            }
            ModelState.Remove("Turnos");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(turno);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TurnoExists(turno.Id))
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
            ViewData["MascotaId"] = new SelectList(_context.Mascota, "Id", "Id", turno.MascotaId);
            ViewData["VeterinarioId"] = new SelectList(_context.Veterinario, "Id", "Id", turno.VeterinarioId);
            return View(turno);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turno = await _context.Turno
                .Include(t => t.Mascota)
                .Include(t => t.Veterinario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (turno == null)
            {
                return NotFound();
            }

            return View(turno);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var turno = await _context.Turno.FindAsync(id);
            if (turno != null)
            {
                _context.Turno.Remove(turno);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TurnoExists(int id)
        {
            return _context.Turno.Any(e => e.Id == id);
        }
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> SearchAvailability(TurnoSearchViewModel model)
        {
            if (model.FechaInicio.HasValue && model.FechaFin.HasValue)
            {
                var disponibles = await _turnoService.EncontrarTurnosDisponibles(
                    model.FechaInicio.Value,
                    model.FechaFin.Value);

                model.TurnosDisponibles = disponibles;

                return View("Create", model);
            }

            return View("Create", model);
        }
    }
}
