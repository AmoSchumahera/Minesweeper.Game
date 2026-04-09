using Minesweeper.Game.Data;
using Minesweeper.Game.Models;
using Minesweeper.Game.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Threading;

namespace Minesweeper.Game.ViewModels
{
    public class GameViewModel : ViewModelBase
    {
        private readonly int _loggedInUserId;
        private readonly DispatcherTimer _timer;
        private readonly AudioService _audioService;
        private bool _isFirstClick = true;

        // Настройки на дъската
        public int Rows { get; } = 15;
        public int Columns { get; } = 15;

        private int _mineCount = 30; // По подразбиране са 30
        public int MineCount
        {
            get => _mineCount;
            set
            {
                // Ограничаваме мините до максимум 216, както изисква заданието ти
                if (value >= 1 && value <= 216)
                {
                    _mineCount = value;
                    OnPropertyChanged();
                }
            }
        }

        // Самата дъска с клетки
        public ObservableCollection<Cell> Board { get; set; }

        private int _timeElapsed;
        public int TimeElapsed
        {
            get => _timeElapsed;
            set { _timeElapsed = value; OnPropertyChanged(); }
        }

        private string _gameStatus = "Играта е в ход...";
        public string GameStatus
        {
            get => _gameStatus;
            set { _gameStatus = value; OnPropertyChanged(); }
        }

        public bool IsGameOver { get; private set; }

        // Команди за лев и десен клик
        public ICommand RevealCommand { get; }
        public ICommand FlagCommand { get; }
        public ICommand RestartCommand { get; }

        public GameViewModel(int loggedInUserId)
        {
            _loggedInUserId = loggedInUserId;
            _audioService = new AudioService();
            Board = new ObservableCollection<Cell>();

            RevealCommand = new RelayCommand<Cell>(RevealCell, canExecute: _ => !IsGameOver);
            FlagCommand = new RelayCommand<Cell>(FlagCell, canExecute: _ => !IsGameOver);
            RestartCommand = new RelayCommand<object>(_ => StartNewGame());

            // Настройка на таймера (отчита на всяка 1 секунда)
            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += (s, e) => TimeElapsed++;

            StartNewGame();
        }

        public void StartNewGame()
        {
            _timer.Stop();
            TimeElapsed = 0;
            _isFirstClick = true;
            IsGameOver = false;
            GameStatus = "Късмет!";

            Board.Clear();
            // Създаваме празна дъска 15x15
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    Board.Add(new Cell { Row = r, Column = c });
                }
            }
        }

        private void PlaceMinesAndCalculateNumbers(Cell firstClickedCell)
        {
            Random rand = new Random();
            int minesPlaced = 0;

            while (minesPlaced < MineCount)
            {
                int randomRow = rand.Next(Rows);
                int randomCol = rand.Next(Columns);

                var cell = GetCell(randomRow, randomCol);

                // Гарантираме, че първата кликната клетка НИКОГА не е мина
                if (cell != null && !cell.IsMine && cell != firstClickedCell)
                {
                    cell.IsMine = true;
                    minesPlaced++;
                }
            }

            // Изчисляваме цифрите за съседните мини на всяка клетка
            foreach (var cell in Board.Where(c => !c.IsMine))
            {
                cell.AdjacentMines = GetAdjacentCells(cell).Count(c => c.IsMine);
            }
        }

        private void RevealCell(Cell cell)
        {
            if (cell == null || cell.IsRevealed || cell.IsFlagged || IsGameOver) return;

            // Генерираме мините чак ПРИ ПЪРВИЯ КЛИК
            if (_isFirstClick)
            {
                _isFirstClick = false;
                PlaceMinesAndCalculateNumbers(cell);
                _timer.Start();
            }

            cell.IsRevealed = true;

            if (cell.IsMine)
            {
                GameOver(win: false);
                return;
            }

            // Алгоритъм Flood-Fill: Ако клетката няма съседни мини, отваряме съседите й автоматично
            if (cell.AdjacentMines == 0)
            {
                foreach (var neighbor in GetAdjacentCells(cell).Where(n => !n.IsRevealed && !n.IsFlagged))
                {
                    RevealCell(neighbor); // Рекурсия
                }
            }

            CheckWinCondition();
        }

        private void FlagCell(Cell cell)
        {
            if (cell == null || cell.IsRevealed || IsGameOver) return;

            cell.IsFlagged = !cell.IsFlagged;
        }

        private void CheckWinCondition()
        {
            // Играта е спечелена, ако всички клетки БЕЗ мини са отворени
            bool isWon = Board.Where(c => !c.IsMine).All(c => c.IsRevealed);

            if (isWon)
            {
                GameOver(win: true);
            }
        }

        private void GameOver(bool win)
        {
            IsGameOver = true;
            _timer.Stop();

            if (win)
            {
                _audioService.PlayWinSound(); // ПУСКАМЕ ЗВУКА ЗА ПОБЕДА
                GameStatus = $"Победа! Време: {TimeElapsed} сек.";
                SaveScoreToDatabase();
            }
            else
            {
                _audioService.PlayLoseSound(); // ПУСКАМЕ ЗВУКА ЗА ЗАГУБА
                GameStatus = "Бум! Опитай пак.";

                // Показваме всички мини при загуба
                foreach (var mine in Board.Where(c => c.IsMine))
                {
                    mine.IsRevealed = true;
                }
            }
        }

        private void SaveScoreToDatabase()
        {
            using var context = new AppDbContext();
            var score = new GameScore
            {
                UserId = _loggedInUserId,
                TimeInSeconds = TimeElapsed,
                MinesCount = MineCount,
                DatePlayed = DateTime.Now
            };

            context.GameScores.Add(score);
            context.SaveChanges();
        }

        // Помощен метод за намиране на клетка по координати
        private Cell? GetCell(int row, int col)
        {
            return Board.FirstOrDefault(c => c.Row == row && c.Column == col);
        }

        // Помощен метод за взимане на всички съседи (до 8 на брой) на дадена клетка
        private System.Collections.Generic.IEnumerable<Cell> GetAdjacentCells(Cell cell)
        {
            for (int r = -1; r <= 1; r++)
            {
                for (int c = -1; c <= 1; c++)
                {
                    if (r == 0 && c == 0) continue; // Пропускаме самата клетка

                    var neighbor = GetCell(cell.Row + r, cell.Column + c);
                    if (neighbor != null) yield return neighbor;
                }
            }
        }
    }
}
