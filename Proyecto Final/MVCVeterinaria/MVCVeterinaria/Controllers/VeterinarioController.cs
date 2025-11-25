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
    public class VeterinarioController : Controller
    {
        private readonly VeterinariaDatabaseContext _context;

        public VeterinarioController(VeterinariaDatabaseContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Veterinario.ToListAsync());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string apellido, string matricula)
        {
            var query = _context.Veterinario.AsQueryable();

            if (!string.IsNullOrEmpty(apellido))
            {
                query = query.Where(v => v.Apellido.Contains(apellido));
            }
            if (!string.IsNullOrEmpty(matricula))
            {
                query = query.Where(v => v.Matricula.Equals(matricula));
            }
            var veterinarios = await query.ToListAsync();

            return View(veterinarios);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var fechaActual = DateTime.Now;
            var veterinario = await _context.Veterinario
                .Include(v => v.Turnos
                    .Where(t => t.FechaHorario >= fechaActual).OrderBy(t => t.FechaHorario))
                        .ThenInclude(t => t.Mascota)
                            .ThenInclude(c => c.Cliente)
                                .FirstOrDefaultAsync(m => m.Id == id);
            if (veterinario == null)
            {
                return NotFound();
            }

            return View(veterinario);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DNI,Nombre,Apellido,Matricula,Telefono,Direccion,Especialidad")] Veterinario veterinario)
        {
            ModelState.Remove("Turnos");

            if (ModelState.IsValid)
            {
                _context.Add(veterinario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(veterinario);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var veterinario = await _context.Veterinario.FindAsync(id);
            if (veterinario == null)
            {
                return NotFound();
            }
            return View(veterinario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DNI,Nombre,Apellido,Matricula,Telefono,Direccion,Especialidad")] Veterinario veterinario)
        {
            if (id != veterinario.Id)
            {
                return NotFound();
            }

            ModelState.Remove("Turnos");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(veterinario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VeterinarioExists(veterinario.Id))
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
            return View(veterinario);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var veterinario = await _context.Veterinario
                .FirstOrDefaultAsync(m => m.Id == id);
            if (veterinario == null)
            {
                return NotFound();
            }

            return View(veterinario);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var veterinario = await _context.Veterinario.FindAsync(id);
            if (veterinario != null)
            {
                _context.Veterinario.Remove(veterinario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VeterinarioExists(int id)
        {
            return _context.Veterinario.Any(e => e.Id == id);
        }
    }
}
