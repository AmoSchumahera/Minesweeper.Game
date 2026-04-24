using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Minesweeper.Game.Views
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        public GameWindow()
        {
            InitializeComponent();
        }
        private void OpenLeaderboard_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var leaderboardWindow = new LeaderboardWindow();
            leaderboardWindow.Owner = this; // Поставя класацията над основния прозорец
            leaderboardWindow.ShowDialog(); // ShowDialog блокира играта, докато гледаш класациите
        }
    }
}
