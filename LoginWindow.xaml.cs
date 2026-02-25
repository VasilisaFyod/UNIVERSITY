using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using UNIVERSITY.Models;

namespace UNIVERSITY
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private UniversityContext _context;

        public LoginWindow()
        {
            InitializeComponent();

            try
            {
                _context = new UniversityContext();

                if (!_context.Database.CanConnect())
                    throw new Exception("База данных недоступна");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Не удалось подключиться к базе данных.\n" + 
                    ex.Message,
                    "Ошибка подключения",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );

                _context = null; 
                LoginButton.IsEnabled = false; 
                GuestButton.IsEnabled = false;  
            }
        }


        private void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text.Trim();
            string password = PasswordTextBox.Password.Trim();

            var users = _context.Workers
                .Include(w => w.Position)
                .Include(w => w.Office)
                .FirstOrDefault(u => u.Login == login && u.Password == password);

            if (users != null)
            {
                App.Login(users);
                EquipmentListWindow main = new EquipmentListWindow();
                main.Show();
                this.Close();
            }
            else {
                ErrorTextBlock.Text = "Неверный логин или пароль";
            }
        }

        private void GuestButtonClick(object sender, RoutedEventArgs e)
        {
            App.Login(null);
            EquipmentListWindow main = new EquipmentListWindow();
            main.Show();
            this.Close();
        }
    }
}
