using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Minesweeper.Game.Models
{
    public class GameScore
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int TimeInSeconds { get; set; }

        [Required]
        public int MinesCount { get; set; }

        public DateTime DatePlayed { get; set; } = DateTime.Now;

        // Навигационно свойство, което свързва резултата с конкретния потребител
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;
    }
}
