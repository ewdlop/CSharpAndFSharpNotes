#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EFCoreTesting;

namespace EFCoreTesting.Data
{
    public class EFCoreTestingContext : DbContext
    {
        public EFCoreTestingContext (DbContextOptions<EFCoreTestingContext> options)
            : base(options)
        {
        }

        public DbSet<EFCoreTesting.Movie> Movie { get; set; }
    }
}
