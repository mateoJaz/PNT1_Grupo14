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

    }
}
