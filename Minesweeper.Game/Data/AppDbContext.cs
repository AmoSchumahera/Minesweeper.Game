using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Minesweeper.Game.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;

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
            optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));    
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Правим колоната Username уникална!
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // ТВЪРДО ЗАДАДЕНИ ДАННИ (Без никакъв DateTime.Now)
            var seedUser = new User
            {
                Id = 1,
                Username = "ProPlayer",
                PasswordHash = "$2a$11$qR7E.fBIdJ9V1N3F6mZ.OeZ/v.X0NqFzY7S3p4Q9U8h.vG1M2V4m6"
            };

            modelBuilder.Entity<User>().HasData(seedUser);

            modelBuilder.Entity<GameScore>().HasData(
                new GameScore { Id = 1, UserId = 1, MinesCount = 20, TimeInSeconds = 45, DatePlayed = new DateTime(2026, 4, 25, 10, 30, 0) },
                new GameScore { Id = 2, UserId = 1, MinesCount = 15, TimeInSeconds = 30, DatePlayed = new DateTime(2026, 4, 20, 14, 15, 0) },
                new GameScore { Id = 3, UserId = 1, MinesCount = 40, TimeInSeconds = 120, DatePlayed = new DateTime(2026, 3, 15, 9, 0, 0) }
            );
        }
    }
}
