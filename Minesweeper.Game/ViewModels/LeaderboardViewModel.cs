using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Minesweeper.Game.Data;
using Minesweeper.Game.Models;

namespace Minesweeper.Game.ViewModels
{
    public class LeaderboardViewModel : ViewModelBase
    {
        public ObservableCollection<GameScore> WeeklyScores { get; set; }
        public ObservableCollection<GameScore> MonthlyScores { get; set; }
        public ObservableCollection<GameScore> AllTimeScores { get; set; }

        public LeaderboardViewModel()
        {
            LoadScores();
        }

        private void LoadScores()
        {
            using var context = new AppDbContext();

            // Взимаме всички резултати от базата и включваме данните за Потребителя (за да му вземем името)
            var allScores = context.GameScores
                .Include(g => g.User)
                .ToList();

            var now = DateTime.Now;

            // Седмична класация (последните 7 дни)
            WeeklyScores = new ObservableCollection<GameScore>(
                allScores.Where(s => s.DatePlayed >= now.AddDays(-7))
                         .OrderBy(s => s.TimeInSeconds) // Първо по най-бързо време
                         .ThenByDescending(s => s.MinesCount) // После по най-много мини
                         .Take(10)); // Взимаме само Топ 10

            // Месечна класация (последните 30 дни)
            MonthlyScores = new ObservableCollection<GameScore>(
                allScores.Where(s => s.DatePlayed >= now.AddDays(-30))
                         .OrderBy(s => s.TimeInSeconds)
                         .ThenByDescending(s => s.MinesCount)
                         .Take(10));

            // За всички времена
            AllTimeScores = new ObservableCollection<GameScore>(
                allScores.OrderBy(s => s.TimeInSeconds)
                         .ThenByDescending(s => s.MinesCount)
                         .Take(10));
        }
    }
}
