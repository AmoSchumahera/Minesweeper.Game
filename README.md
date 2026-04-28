# 💣 Minesweeper Pro

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![WPF](https://img.shields.io/badge/WPF-Windows_Presentation_Foundation-0078D7?style=for-the-badge&logo=windows)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC292B?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)
![EF Core](https://img.shields.io/badge/Entity_Framework_Core-336699?style=for-the-badge)

**Minesweeper Pro** е модерна десктоп реализация на класическата логическа игра Миночистач. Приложението е изградено със съвременни технологии, стриктно следва архитектурния шаблон **MVVM (Model-View-ViewModel)** и включва сигурна система за регистрация на играчи, локална база данни за запазване на резултатите и динамичен графичен интерфейс.

Този проект е разработен като дипломна работа и демонстрира добри практики в софтуерното инженерство, алгоритмите (рекурсия) и UI/UX дизайна.

---

## ✨ Основни функционалности

* 🔐 **Сигурна система за потребители:** Регистрация и автентикация с криптографско хеширане на паролите чрез **BCrypt**.
* 🏆 **Зала на славата (Leaderboard):** Глобални класации (Седмична, Месечна, За всички времена), които извличат и сортират данни чрез Entity Framework Core.
* 🎨 **Визуални теми (Skins):** Динамична смяна между **Светла**, **Тъмна** и **Ретро** тема в реално време, използвайки XAML DataTriggers.
* 🔍 **Мащабиране (Zoom):** Вграден слайдер за плавно увеличаване и намаляване на игралното поле (ScaleTransform).
* 🚀 **Flood-Fill алгоритъм:** Бързо рекурсивно разкриване на празни съседни клетки (Depth-First Search).
* 🛡️ **Защита на първия ход:** Гарантирано безопасно първо кликване – мините се генерират динамично *след* първия ход на играча.
* 🎵 **Аудио ефекти:** Асинхронно възпроизвеждане на звуци при ключови събития (победа/загуба) чрез `MediaPlayer`.

---

## 🛠️ Използвани технологии

* **Език:** C#
* **Платформа:** .NET 10
* **Графичен интерфейс:** WPF (Windows Presentation Foundation) & XAML
* **База данни:** Microsoft SQL Server
* **ORM:** Entity Framework Core (Code-First подход)
* **Сигурност:** BCrypt.Net-Next
* **Архитектура:** MVVM (Model-View-ViewModel)

---

## 📸 Галерия

*(Забележка: Добавете свои снимки в папка `/Screenshots` и премахнете коментарите по-долу)*

---

## 🚀 Инсталация и стартиране (Локално)

За да стартирате проекта локално на вашия компютър, следвайте тези стъпки:

### 1. Системни изисквания
* [Visual Studio 2022](https://visualstudio.microsoft.com/) (с инсталиран пакет за .NET Desktop Development)
* [.NET 10 SDK](https://dotnet.microsoft.com/)
* [Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (Express или LocalDB)

### 2. Клониране на хранилището
```bash
git clone [https://github.com/vashiq-potrebitel/MinesweeperPro.git](https://github.com/vashiq-potrebitel/MinesweeperPro.git)
