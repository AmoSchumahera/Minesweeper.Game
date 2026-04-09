using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Minesweeper.Game.Data;
using Minesweeper.Game.Models;
using BCrypt.Net;

namespace Minesweeper.Game.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;

        public AuthService()
        {
            _context = new AppDbContext();
        }

        // Регистрация на нов потребител
        public bool Register(string username, string password)
        {
            // Проверка дали потребителят вече съществува
            if (_context.Users.Any(u => u.Username == username))
            {
                return false; // Потребителят съществува
            }

            // Хеширане на паролата (никога не пазим чист текст!)
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            var newUser = new User
            {
                Username = username,
                PasswordHash = passwordHash
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return true;
        }

        // Вход на съществуващ потребител
        public User? Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                return null; // Грешно име
            }

            // Проверка дали въведената парола съвпада с хеша в базата данни
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

            if (isPasswordValid)
            {
                return user; // Успешен вход
            }

            return null; // Грешна парола
        }
    }
}
