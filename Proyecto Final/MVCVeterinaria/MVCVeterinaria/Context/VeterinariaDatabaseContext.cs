using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVCVeterinaria.Models;
using System.Collections.Generic;

namespace MVCVeterinaria.Context
{
    public class VeterinariaDatabaseContext : DbContext
    {
        public VeterinariaDatabaseContext(DbContextOptions<VeterinariaDatabaseContext> options) : base(options)
        {
        }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<MVCVeterinaria.Models.Evento> Evento { get; set; } = default!;
        public DbSet<MVCVeterinaria.Models.Mascota> Mascota { get; set; } = default!;
        public DbSet<MVCVeterinaria.Models.Turno> Turno { get; set; } = default!;
        public DbSet<MVCVeterinaria.Models.Veterinario> Veterinario { get; set; } = default!;

        public DbSet<MVCVeterinaria.Models.Usuario> Usuarios { get; set; }

    }
}
