#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using myTestWork.Models;

namespace myTestWork.Data
{
#pragma warning disable CS1591 
    public class myTestWorkContext : DbContext
    {
        public myTestWorkContext(DbContextOptions<myTestWorkContext> options)
            : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Skill>()
                .HasOne(p => p.Person)
                .WithMany(c => c.Skills)
                .HasForeignKey(d => d.PersonID)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<myTestWork.Models.Person> Person { get; set; }
        public DbSet<myTestWork.Models.Skill> Skill { get; set; }
    }
#pragma warning restore CS1591
}
