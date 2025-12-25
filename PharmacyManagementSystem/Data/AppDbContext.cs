using PharmacyManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace PharmacyManagementSystem.Data
{
    public class AppDbContext : DbContext
    {
       

  
            public AppDbContext(DbContextOptions<AppDbContext> options)
                : base(options)
            {
            }


        // DbSets for all entities
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<BillDetails> BillDetails { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<User> User { get; set; }  // ← Add karo


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Bill -> Customer relationship (using CusId foreign key)
            modelBuilder.Entity<Bill>()
                .HasOne(b => b.Customer)
                .WithMany(c => c.Bills)
                .HasForeignKey(b => b.CusId)
                .OnDelete(DeleteBehavior.Restrict);

            // Bill -> Employee relationship (using EmpId foreign key)
            modelBuilder.Entity<Bill>()
                .HasOne(b => b.Employee)
                .WithMany(e => e.Bills)
                .HasForeignKey(b => b.EmpId)
                .OnDelete(DeleteBehavior.Restrict);

            // BillDetails -> Bill relationship (using BillId foreign key)
            modelBuilder.Entity<BillDetails>()
                .HasOne(bd => bd.Bill)
                .WithMany(b => b.BillItems)
                .HasForeignKey(bd => bd.BillId)
                .OnDelete(DeleteBehavior.Cascade);

            // BillDetails -> Medicine relationship (using MedId foreign key)
            modelBuilder.Entity<BillDetails>()
                .HasOne(bd => bd.Medicine)
                .WithMany()
                .HasForeignKey(bd => bd.MedId)
                .OnDelete(DeleteBehavior.Restrict);

            // Medicine -> Supplier relationship (using SupplierId foreign key)
            modelBuilder.Entity<Medicine>()
                .HasOne(m => m.Supplier)
                .WithMany(s => s.Medicines)
                .HasForeignKey(m => m.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Employee)
                .WithOne(e => e.User)
                .HasForeignKey<User>(u => u.EmployeeId);
        }
    }
}
