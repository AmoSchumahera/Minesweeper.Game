using Minesweeper.Game.Models;
using Minesweeper.Game.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Minesweeper.Game.Models
{
    public class Cell : ViewModelBase
    {
        public int Row { get; set; }
        public int Column { get; set; }

        private bool _isMine;
        public bool IsMine
        {
            get => _isMine;
            set { _isMine = value; OnPropertyChanged(); }
        }

        private bool _isRevealed;
        public bool IsRevealed
        {
            get => _isRevealed;
            set
            {
                _isRevealed = value;
                OnPropertyChanged();
                // Когато се отвори клетката, искаме интерфейсът да провери отново какво да покаже
                OnPropertyChanged(nameof(Content));
                OnPropertyChanged(nameof(Background));
            }
        }

        private bool _isFlagged;
        public bool IsFlagged
        {
            get => _isFlagged;
            set
            {
                _isFlagged = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Content));
                OnPropertyChanged(nameof(Foreground));
            }
        }

        private int _adjacentMines;
        public int AdjacentMines
        {
            get => _adjacentMines;
            set { _adjacentMines = value; OnPropertyChanged(); }
        }

        // Помощни свойства за Дизайна (UI)

        // Какво да се изпише в клетката?
        public string Content
        {
            get
            {
                if (IsFlagged && !IsRevealed) return "🚩";
                if (!IsRevealed) return "";
                if (IsMine) return "💣";
                return AdjacentMines > 0 ? AdjacentMines.ToString() : "";
            }
        }

        // Цвят на текста (различни цветове за числата 1, 2, 3 и т.н.)
        public string Foreground
        {
            get
            {
                if (IsFlagged && !IsRevealed) return "Red";
                if (!IsRevealed || IsMine) return "Black";

                return AdjacentMines switch
                {
                    1 => "Blue",
                    2 => "Green",
                    3 => "Red",
                    4 => "DarkBlue",
                    5 => "DarkRed",
                    6 => "Teal",
                    7 => "Black",
                    8 => "Gray",
                    _ => "Black"
                };
            }
        }

        // Цвят на фона на клетката (светлосиво за отворена, тъмно за затворена)
        public string Background
        {
            get
            {
                return IsRevealed ? "#E5E7EB" : "#9CA3AF";
            }
        }
    }
}
