using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCVeterinaria.Context;
using MVCVeterinaria.Models;

namespace MVCVeterinaria.Controllers
{
    public class TurnoController : Controller
    {
        private readonly VeterinariaDatabaseContext _context;

        public TurnoController(VeterinariaDatabaseContext context)
        {
            _context = context;
        }

        // GET: Turno
        public async Task<IActionResult> Index()
        {
            var veterinariaDatabaseContext = _context.Turno.Include(t => t.Mascota).Include(t => t.Veterinario);
            return View(await veterinariaDatabaseContext.ToListAsync());
        }

        // GET: Turno/Details/5
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

        // GET: Turno/Create
        public IActionResult Create()
        {
            ViewData["MascotaId"] = new SelectList(_context.Mascota, "Id", "Id");
            ViewData["VeterinarioId"] = new SelectList(_context.Veterinario, "Id", "Id");
            return View();
        }

        // POST: Turno/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Turno/Edit/5
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

        // POST: Turno/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Turno/Delete/5
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

        // POST: Turno/Delete/5
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
    }
}
