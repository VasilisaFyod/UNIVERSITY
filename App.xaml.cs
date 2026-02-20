using System.Configuration;
using System.Data;
using System.Windows;
using UNIVERSITY.Models;

namespace UNIVERSITY
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Worker currentUser;

        public static void Login(Worker user)
        {
            currentUser = user;
        }

        public static void Logout()
        {
            currentUser = null;
        }
    }

}
