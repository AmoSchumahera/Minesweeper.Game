using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace Minesweeper.Game.Services
{
    public class AudioService
    {
        private readonly MediaPlayer _player;

        public AudioService()
        {
            _player = new MediaPlayer();
        }

        public void PlayWinSound()
        {
            try
            {
                // Зареждаме и пускаме звука за победа
                _player.Open(new Uri("Sounds/win.mp3", UriKind.Relative));
                _player.Play();
            }
            catch
            {
                // Ако файлът липсва, просто игнорираме, за да не крашне играта
            }
        }

        public void PlayLoseSound()
        {
            try
            {
                // Зареждаме и пускаме звука за загуба (експлозия)
                _player.Open(new Uri("Sounds/lose.mp3", UriKind.Relative));
                _player.Play();
            }
            catch
            { }
        }
    }
}
