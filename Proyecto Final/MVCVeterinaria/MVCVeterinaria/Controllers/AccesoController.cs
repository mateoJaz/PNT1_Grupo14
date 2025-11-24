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

        // 1. REGISTRO (GET)
        public IActionResult Registrar()
        {
            return View();
        }

        // 1. REGISTRO (POST)
        [HttpPost]
        public async Task<IActionResult> Registrar(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login");
            }
            return View(usuario);
        }

        // 2. LOGIN (GET)
        public IActionResult Login()
        {
            return View();
        }

        // 2. LOGIN (POST)
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

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Mensaje = "Usuario o clave incorrectos";
                return View();
            }
        }

        // 3. SALIR
        public async Task<IActionResult> Salir()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
