using Microsoft.AspNetCore.Mvc;
using MVCVeterinaria.Context;
using MVCVeterinaria.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace MVCVeterinaria.Controllers
{
    public class AccesoController : Controller
    {
        private readonly VeterinariaDatabaseContext _context;

        public AccesoController(VeterinariaDatabaseContext context)
        {
            _context = context;
        }
        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrar(Usuario usuario)
        {
            bool existeEmail = await _context.Usuarios.AnyAsync(u => u.Email == usuario.Email);
            if (existeEmail)
            {
                ModelState.AddModelError("Email", "Este correo ya está registrado por otro usuario.");
            }

            if (usuario.Nombre != null)
            {
                bool existeNombre = await _context.Usuarios.AnyAsync(u => u.Nombre == usuario.Nombre);
                if (existeNombre)
                {
                    ModelState.AddModelError("Nombre", "Este nombre de usuario ya está en uso.");
                }
            }

            if (ModelState.IsValid)
            {
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login");
            }
            return View(usuario);
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Usuario usuario)
        {
            var usuarioEncontrado = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == usuario.Email && u.Clave == usuario.Clave);

            if (usuarioEncontrado != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuarioEncontrado.Nombre ?? ""),
                    new Claim("Correo", usuarioEncontrado.Email ?? "")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Cliente");
            }
            else
            {
                ViewBag.Mensaje = "Usuario o clave incorrectos";
                return View();
            }
        }
        public async Task<IActionResult> Salir()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
