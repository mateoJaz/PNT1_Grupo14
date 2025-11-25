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
    public class MascotaController : Controller
    {
        private readonly VeterinariaDatabaseContext _context;

        public MascotaController(VeterinariaDatabaseContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var veterinariaDatabaseContext = _context.Mascota.Include(m => m.Cliente);
            return View(await veterinariaDatabaseContext.ToListAsync());
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var mascota = await _context.Mascota
                .Include(m => m.Cliente)
                .Include(m => m.HistorialClinico).ThenInclude(e => e.Veterinario)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (mascota == null) return NotFound();

            return View(mascota);
        }
        public async Task<IActionResult> Create(int? clienteId)
        {
            if (clienteId == null)
            {
                return RedirectToAction("Index", "Clientes");
            }
            var cliente = await _context.Clientes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == clienteId);
            if (cliente == null)
            {
                return RedirectToAction("Index", "Clientes");
            }
            var mascota = new Mascota
            {
                ClienteId = cliente.Id,
                FechaNacimiento = new DateTime(1980, 1, 1)
            };
            return View(mascota);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClienteId, Nombre,Especie,Raza,FechaNacimiento,Peso")] Mascota mascota)
        {
            mascota.Vivo = true;
            ModelState.Remove("HistorialClinico");
            ModelState.Remove("Cliente");
            ModelState.Remove("Turnos");

            if (mascota.FechaNacimiento > DateTime.Today)
            {
                ModelState.AddModelError("FechaNacimiento", "La fecha de nacimiento no puede ser mayor a la fecha actual.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(mascota);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Cliente", new { id = mascota.ClienteId });
            }
            return View(mascota);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var mascota = await _context.Mascota.FindAsync(id);
            if (mascota == null) return NotFound();

            if (!mascota.Vivo)
            {
                TempData["MensajeError"] = "No se puede editar una mascota fallecida.";
                return RedirectToAction("Details", "Cliente", new { id = mascota.ClienteId });
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "NombreCompleto", mascota.ClienteId);
            return View(mascota);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClienteId,Nombre,Especie,Raza,FechaNacimiento,Peso,Vivo")] Mascota mascota)
        {
            if (id != mascota.Id)
            {
                return NotFound();
            }
            ModelState.Remove("HistorialClinico");
            ModelState.Remove("Cliente");
            ModelState.Remove("Turnos");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mascota);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MascotaExists(mascota.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Cliente", new { id = mascota.ClienteId });
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "NombreCompleto", mascota.ClienteId);
            return View(mascota);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var mascota = await _context.Mascota.Include(m => m.Cliente).FirstOrDefaultAsync(m => m.Id == id);
            if (!mascota.Vivo)
            {
                TempData["MensajeError"] = "Esta mascota ya figura como fallecida.";
                return RedirectToAction("Details", "Cliente", new { id = mascota.ClienteId });
            }
            return View(mascota);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mascota = await _context.Mascota.FindAsync(id);

            if (mascota != null)
            {
                mascota.Vivo = false;
                _context.Mascota.Update(mascota);

                await _context.SaveChangesAsync();

                return RedirectToAction("Details", "Cliente", new { id = mascota.ClienteId });
            }

            return RedirectToAction(nameof(Index));
        }
        private bool MascotaExists(int id)
        {
            return _context.Mascota.Any(e => e.Id == id);
        }
    }
}
