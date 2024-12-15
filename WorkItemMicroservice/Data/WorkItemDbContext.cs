

using Microsoft.EntityFrameworkCore;
using WorkItemMicroservice.Models;
using System;
using System.Collections.Generic;

namespace WorkItemMicroservice.Data
{
    public class WorkItemDbContext : DbContext
    {
        public DbSet<WorkItem> WorkItems { get; set; }

        public WorkItemDbContext(DbContextOptions<WorkItemDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkItem>()
                .HasKey(w => w.Id);   // primary key Id


            //Creating the first elements for the database.

            modelBuilder.Entity<WorkItem>().HasData(
                new WorkItem
                {
                    Id = 1,
                    Description = "Revisar documentación",
                    Relevance = Relevance.High,
                    DueDate = DateTime.Today.AddDays(2),
                    AssignedUser = "UsuarioA",
                    Status = Status.Pending
                },
                new WorkItem
                {
                    Id = 2,
                    Description = "Desarrollo de módulo A",
                    Relevance = Relevance.Low,
                    DueDate = DateTime.Today.AddDays(5),
                    AssignedUser = "UsuarioB",
                    Status = Status.Pending
                },
                new WorkItem
                {
                    Id = 3,
                    Description = "Corrección de errores",
                    Relevance = Relevance.High,
                    DueDate = DateTime.Today.AddDays(1),
                    AssignedUser = "UsuarioA",
                    Status = Status.Pending
                }
            );
        }
    }
}
