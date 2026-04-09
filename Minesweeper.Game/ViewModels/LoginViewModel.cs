using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Minesweeper.Game.Services;

namespace Minesweeper.Game.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly AuthService _authService;

        private string _username = string.Empty;
        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }

        private string _errorMessage = string.Empty;
        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        private string _messageColor = "Red";
        public string MessageColor
        {
            get => _messageColor;
            set { _messageColor = value; OnPropertyChanged(); }
        }

        // Превключвател: true = Логин екран, false = Регистрация екран
        private bool _isLoginMode = true;
        public bool IsLoginMode
        {
            get => _isLoginMode;
            set
            {
                _isLoginMode = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Title));
                OnPropertyChanged(nameof(SubmitButtonText));
                OnPropertyChanged(nameof(ToggleModeText));
                ErrorMessage = string.Empty; // Изчистваме грешките при смяна
            }
        }

        // Динамични текстове за UI
        public string Title => IsLoginMode ? "Вход" : "Регистрация";
        public string SubmitButtonText => IsLoginMode ? "ВЛЕЗ" : "РЕГИСТРИРАЙ СЕ";
        public string ToggleModeText => IsLoginMode ? "Нямаш акаунт? Регистрирай се тук." : "Вече имаш акаунт? Влез от тук.";

        public ICommand SubmitCommand { get; }
        public ICommand ToggleModeCommand { get; }

        public LoginViewModel()
        {
            _authService = new AuthService();

            // Командата очаква PasswordBox като параметър (от съображения за сигурност WPF не позволява директен Binding на пароли)
            SubmitCommand = new RelayCommand<PasswordBox>(ExecuteSubmit);
            ToggleModeCommand = new RelayCommand<object>(_ => IsLoginMode = !IsLoginMode);
        }

        private void ExecuteSubmit(PasswordBox passwordBox)
        {
            // По подразбиране връщаме цвета на червен при всеки нов опит
            MessageColor = "Red";

            if (string.IsNullOrWhiteSpace(Username) || passwordBox == null || string.IsNullOrWhiteSpace(passwordBox.Password))
            {
                ErrorMessage = "Моля, попълнете всички полета!";
                return;
            }

            string password = passwordBox.Password;

            if (IsLoginMode)
            {
                var user = _authService.Login(Username, password);
                if (user != null)
                {
                    // УСПЕХ: Правим цвета зелен
                    MessageColor = "Green";
                    ErrorMessage = "Успешен вход! Зареждане на играта...";

                    // Създаваме новия прозорец за играта и му подаваме GameViewModel с ID-то на потребителя
                    var gameWindow = new Views.GameWindow();
                    var gameViewModel = new GameViewModel(user.Id);
                    gameWindow.DataContext = gameViewModel;

                    gameWindow.Show();

                    // Затваряме текущия прозорец за логин (може да се наложи да подадеш прозореца като параметър или да ползваш Application.Current.Windows)
                    Application.Current.Windows[0].Close();
                }
                else
                {
                    ErrorMessage = "Грешно потребителско име или парола!";
                }
            }
            else // Регистрация
            {
                bool success = _authService.Register(Username, password);
                if (success)
                {
                    //Първо се сменя режима за "ВХОД"
                    IsLoginMode = true;

                    // УСПЕХ: Правим цвета зелен
                    MessageColor = "Green";
                    ErrorMessage = "Успешна регистрация! Вече можете да влезете.";
                     // Връщаме го на екран за вход с празни полета
                    passwordBox.Clear();
                }
                else
                {
                    ErrorMessage = "Това потребителско име вече е заето!";
                }
            }
        }
    }
}
