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
    public class EventoController : Controller
    {
        private readonly VeterinariaDatabaseContext _context;

        public EventoController(VeterinariaDatabaseContext context)
        {
            _context = context;
        }

        // GET: Evento
        public async Task<IActionResult> Index()
        {
            var veterinariaDatabaseContext = _context.Evento.Include(e => e.Veterinario);
            return View(await veterinariaDatabaseContext.ToListAsync());
        }

        // GET: Evento/Details/5
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

        // GET: Evento/Create
        // Ahora idMascota es obligatorio para entrar aquí
        public async Task<IActionResult> Create(int? idMascota)
        {
            // 1. VALIDACIÓN: Si no hay mascota, no se puede crear evento.
            if (idMascota == null)
            {
                // Lo mandamos a la lista de mascotas para que elija una
                return RedirectToAction("Index", "Mascota");
            }

            // 2. Buscar el nombre de la mascota para mostrarlo en el título
            var mascota = await _context.Mascota.FindAsync(idMascota);
            if (mascota == null)
            {
                return NotFound(); // Si el ID no existe en la BD
            }
            ViewBag.NombreMascota = mascota.Nombre;

            // 3. Preparar desplegable de Veterinarios (Concatenando Apellido y Nombre)
            var veterinarios = _context.Veterinario.Select(v => new
            {
                Id = v.Id,
                NombreCompleto = v.Apellido + ", " + v.Nombre
            });
            ViewData["VeterinarioId"] = new SelectList(veterinarios, "Id", "NombreCompleto");

            // 4. Tipos de evento predefinidos
            var listaTipos = new List<string> { "Consulta General", "Vacunación", "Cirugía", "Estudio", "Urgencia", "Control" };
            ViewData["TiposEvento"] = new SelectList(listaTipos);

            // 5. Preparar el Modelo con datos iniciales
            var evento = new Evento
            {
                FechaHorario = DateTime.Now,
                MascotaId = idMascota.Value // Fijamos el ID de la mascota
            };

            return View(evento);
        }

        // POST: Evento/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TipoEvento,Detalle,FechaHorario,MascotaId,VeterinarioId")] Evento evento)
        {
            if (ModelState.IsValid)
            {
                _context.Add(evento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["VeterinarioId"] = new SelectList(_context.Veterinario, "Id", "Id", evento.VeterinarioId);
            return View(evento);
        }

        // GET: Evento/Edit/5
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

        // POST: Evento/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Evento/Delete/5
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

        // POST: Evento/Delete/5
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
