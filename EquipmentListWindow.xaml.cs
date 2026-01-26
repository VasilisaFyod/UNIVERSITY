using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UNIVERSITY.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace UNIVERSITY
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class EquipmentList : Window
    {
        private Worker _user = null;
        private UniversityContext _context = new UniversityContext();
        public class EquipmentViewModel
        {
            public string Name { get; set; }
            public string Description { get; set; }

            public string Photo { get; set; }

            public string Auditorium { get; set; }

            public string FullNameOffice { get; set; }
            public string Status { get; set; }
            public Brush StatusBrush { get; set; }

        }
        public bool CanSeeStatus {  get; set; }

        public EquipmentList(Worker user = null)
        {
            InitializeComponent();
            DataContext = this;
            if (user != null)
            {
                _user = user;
                UserNameTextBlock.Text = $"ФИО: {user.Lastname} {user.Firstname} {user.Fathername}";
            }
            else
            {
                UserNameTextBlock.Text = $"Гость";
            }
            LoadData();


        }

        private void LoadData()
        {
            DateTime currentYear = DateTime.Now;

            var equipments = _context.Equipments
                .Include(x => x.Auditorium)
                .ThenInclude(a => a.Office)
                .AsEnumerable();

            if (_user == null)
            {
                CanSeeStatus = false;
                equipments = equipments.Where(x => x.Auditorium != null && x.Auditorium.Name.Trim().ToLower() != "засекречено");
            }

            else if (_user.PositionId == 3 || _user.PositionId == 2 || _user.PositionId == 4) 
            {
                CanSeeStatus = false;
                if (_user.PositionId == 3 || _user.PositionId == 2)
                {
                    equipments = equipments.Where(x =>
                        x.Auditorium != null &&
                        x.Auditorium.OfficeId == _user.OfficeId);
                }
            }
            else if (_user.PositionId == 1 || _user.PositionId == 6 || _user.PositionId == 7)
            {
                CanSeeStatus = true;
                if (_user.PositionId == 1 || _user.PositionId == 7)
                {
                    equipments = equipments.Where(x =>
                        x.Auditorium != null &&
                        x.Auditorium.OfficeId == _user.OfficeId);
                }
            }



            List.ItemsSource = equipments.Select(x =>
            {
                DateTime startDate = x.DateBalance.ToDateTime(TimeOnly.MinValue);
                DateTime endDate = startDate.AddYears(x.ServiceLife);

                string statusText;
                Brush statusBrush = Brushes.Transparent;

                bool isSklad = x.Auditorium != null &&
                                x.Auditorium.Name.ToLower().Contains("склад");

                if (endDate < currentYear && !isSklad)
                {
                    statusText = "На списание";
                    statusBrush = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#E32636"));
                }
                else if (endDate == currentYear)
                {
                    statusText = "Срок службы истекает в этом году";
                    statusBrush = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#FFA500"));
                }
                else
                {
                    statusText = $"Срок службы до: {endDate:dd.MM.yyyy}";
                }

                return new EquipmentViewModel
                {
                    Name = x.Name,
                    Description = x.Description,
                    Photo = string.IsNullOrEmpty(x.Photo)
                            ? "Images/stub.jpg"
                            : $"Images/{x.Photo.Trim()}",
                    FullNameOffice = x.Auditorium?.Office?.FullNameOffice,
                    Auditorium = x.Auditorium?.Name,
                    Status = statusText,
                    StatusBrush = statusBrush
                };
            }).ToList();
        }
        

        private void ExitButton(object sender, RoutedEventArgs e)
        {
            LoginWindow window = new LoginWindow();
            window.Show();
            this.Close();
        }
    }
} 