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
        private UniversityContext _context = new UniversityContext();
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text.Trim();
            string password = PasswordTextBox.Password.Trim();

            var users = _context.Workers
                .Include(w => w.Position)
                .Include(w => w.Office)
                .FirstOrDefault(u => u.Login == login && u.Password == password);

            if (users != null)
            {
                EquipmentList main = new EquipmentList(users);
                main.Show();
                this.Close();
            }
            else {
                ErrorTextBlock.Text = "Неверный логин или пароль";
            }
        }

        private void GuestButton(object sender, RoutedEventArgs e)
        {
            EquipmentList main = new EquipmentList();
            main.Show();
            this.Close();
        }
    }
}
