using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Minesweeper.Game.Models;

namespace Minesweeper.Game.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<GameScore> GameScores { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Тук задаваме връзката към SQL Server. 
            // Ако имаш инсталиран SQL Server Express, промени "Server=(localdb)\\mssqllocaldb" на "Server=.\\SQLEXPRESS"
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MinesweeperDb;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Гарантираме, че не може да има двама потребители с едно и също име
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();
        }
    }
}
