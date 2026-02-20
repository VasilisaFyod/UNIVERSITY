using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using UNIVERSITY.Models;
namespace UNIVERSITY
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class EquipmentListWindow : Window
    {
        private Worker _user = App.currentUser;
        private UniversityContext _context = UniversityContext.GetContext();
        public int? SelectedOfficeId { get; set; } = null;
        public string SelectedWeightSort { get; set; } = "Без сортировки";
        public List<Office> Offices { get; set; } = new List<Office>();
        public bool CanSeeStatus { get; set; } = false;

        public List<string> WeightSortOptions { get; set; } = new List<string>
{
            "Без сортировки",
            "По возрастанию",
            "По убыванию"
        };
        public bool IsSortFilterVisible =>
            _user != null &&
            (_user.Position.PositionName == "инженер" ||
             _user.Position.PositionName == "администратор бд");

        public EquipmentListWindow()
        {
            InitializeComponent();

            Offices = new List<Office>();

            DataContext = this;

            if (_user != null)
            {
                UserNameTextBlock.Text = $"ФИО: {_user.Lastname} {_user.Firstname} {_user.Fathername}";
                if (
                    (_user.Position.PositionName == "инженер" ||
                     _user.Position.PositionName == "администратор бд"))
                {
                    Offices = _context.Offices.ToList();

                    Offices.Insert(0, new Office
                    {
                        OfficeId = 0,
                        NameOffice = "Все подразделения"
                    });
                    SelectedOfficeId = 0;
                }
                else
                {
                }
            }
            else
            {
                UserNameTextBlock.Text = $"Гость";
            }
            LoadEquipments();
        }

        private void LoadEquipments(string? searchText = null)
        {
            DateTime currentDate = DateTime.Now;

            var equipments = _context.Equipments
                .Include(e => e.Office)
                .Include(e => e.Auditorium)
                .ThenInclude(a => a.Office)
                .AsQueryable();

            if (_user == null)
            {
                equipments = equipments.Where(x =>
                    (x.Auditorium != null && x.Auditorium.Office == null)
                    ||
                    (x.Office != null && x.Office.NameOffice == "столовая")
                    ||
                    (x.Auditorium != null && x.Auditorium.Office.NameOffice == "столовая"));
            }
            else
            {
                CanSeeStatus =
                    _user.Position.PositionName == "заведующий лабораторией" ||
                    _user.Position.PositionName == "администратор бд" ||
                    _user.Position.PositionName == "заведующий складом";

                if (_user.Position.PositionName != "инженер" &&
                    _user.Position.PositionName != "администратор бд")
                {
                    equipments = equipments.Where(x =>
                        (x.Auditorium != null && x.Auditorium.OfficeId == _user.OfficeId)
                        ||
                        (x.OfficeId == _user.OfficeId));
                }
            }
            if (SelectedOfficeId.HasValue)
            {
                equipments = equipments.Where(x =>
                    x.OfficeId == SelectedOfficeId ||
                    (x.Auditorium != null && x.Auditorium.OfficeId == SelectedOfficeId)
                );
            }
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                var words = searchText
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                foreach (var word in words)
                {
                    string pattern = $"%{word}%";

                    equipments = equipments.Where(x =>
                        EF.Functions.Like(x.InventareNum!, pattern) ||
                        EF.Functions.Like(x.Name!, pattern) ||
                        EF.Functions.Like(x.Description!, pattern) ||
                        (x.Auditorium != null &&
                            EF.Functions.Like(x.Auditorium.Name!, pattern)) ||
                        (x.Office != null &&
                            EF.Functions.Like(x.Office.NameOffice!, pattern)) ||
                        (x.Auditorium != null && x.Auditorium.Office != null &&
                            EF.Functions.Like(x.Auditorium.Office.NameOffice!, pattern))
                    );
                }
            }
            if (IsSortFilterVisible)
            {
                if (SelectedWeightSort == "По возрастанию")
                {
                    equipments = equipments.OrderBy(x => x.Weigth);
                }
                else if (SelectedWeightSort == "По убыванию")
                {
                    equipments = equipments.OrderByDescending(x => x.Weigth);
                }
            }

            ListEquipments.ItemsSource = equipments
                .AsEnumerable()
                .Select(x =>
                {
                    DateTime startDate = x.DateBalance.ToDateTime(TimeOnly.MinValue);
                    DateTime endDate = startDate.AddYears(x.ServiceLife);

                    string statusText;
                    Brush statusBrush = Brushes.Transparent;

                    bool isSklad = x.Auditorium != null &&
                                   x.Auditorium.Name.Contains("склад");

                    if (endDate < currentDate && !isSklad)
                    {
                        statusText = "На списание";
                        statusBrush = new SolidColorBrush(
                            (Color)ColorConverter.ConvertFromString("#E32636"));
                    }
                    else if (endDate.Year == currentDate.Year)
                    {
                        statusText = "Срок службы истекает в этом году";
                        statusBrush = new SolidColorBrush(
                            (Color)ColorConverter.ConvertFromString("#FFA500"));
                    }
                    else
                    {
                        statusText = $"Срок службы до: {endDate:dd.MM.yyyy}";
                    }

                    return new EquipmentDataList
                    {
                        EquipmentId = x.EquipmentId,
                        Name = x.Name,
                        Description = x.Description,
                        Photo = x.PhotoImage,
                        FullNameOffice = x.Auditorium?.Office?.FullNameOffice ?? x.Office?.FullNameOffice,
                        Auditorium = x.Auditorium?.Name,
                        Status = statusText,
                        StatusBrush = statusBrush
                    };
                })
                .ToList();
        }

        private void SearchEquipment_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadEquipments(SearchEquipment.Text.Trim());
        }

        private void ExitButton(object sender, RoutedEventArgs e)
        {
            App.Logout();
            LoginWindow window = new LoginWindow();
            window.Show();
            this.Close();
        }

        private void AddButton(object sender, RoutedEventArgs e)
        {
            AddUpdateEquipmentWindow window = new AddUpdateEquipmentWindow();
            window.ShowDialog();
        }

        private void ViewEditEquipment(object sender, SelectionChangedEventArgs e)
        {
            var selected = ListEquipments.SelectedItem as EquipmentDataList;
            if (_user == null || selected == null)
                return;

            var allowedRoles = new[] { "администратор", "заведующий", "инженер", "техник" };
            string userRole = _user.Position.PositionName.ToLower().Trim();

            bool isAllowed = allowedRoles.Any(r => userRole.Contains(r));

            if (!isAllowed)
                return;
            var equipment = _context.Equipments
                .Include(e => e.Office)
                .Include(e => e.Auditorium)
                .ThenInclude(a => a.Office)
                .FirstOrDefault(e => e.EquipmentId == selected.EquipmentId);

            if (equipment != null)
            {
                AddUpdateEquipmentWindow window = new AddUpdateEquipmentWindow(equipment);
                window.ShowDialog();
            }
        }

        private void OfficeFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedOfficeId == 0)
                SelectedOfficeId = null; 

            LoadEquipments(SearchEquipment.Text.Trim());
        }
        private void WeightSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadEquipments(SearchEquipment.Text.Trim());
        }

    }
} 