using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using UNIVERSITY.Models;

namespace UNIVERSITY
{
    /// <summary>
    /// Логика взаимодействия для AddUpdateEquipmentWindow.xaml
    /// </summary>
    public partial class AddUpdateEquipmentWindow : Window
    {
        private Worker _user = App.currentUser;
        public bool IsAdmin => _user.Position.PositionName.ToLower().Contains("администратор");
        private Equipment _equipment;
        private UniversityContext _context = UniversityContext.GetContext();
        public bool IsReadOnlyMode { get; private set; }
        public int? SelectedOfficeId { get; set; }
        public int? SelectedAuditoriumId { get; set; }
        public bool IsEditMode => _equipment == null || _equipment.EquipmentId == 0;
  

        public AddUpdateEquipmentWindow(Equipment equipment = null)
        {
            InitializeComponent();
            _equipment = equipment ?? new Equipment();
            

            _equipment.Offices = _context.Offices.ToList();
            if (equipment != null)
            {
                if (_equipment.Auditorium != null)
                    _equipment.OfficeId = _equipment.Auditorium.OfficeId;
            }
            else
            {
                _equipment.DateBalanceDateTime = DateTime.Now;
            }
            if (_user != null)
            {
                if (_user != null)
                {
                    if (_user.Position.PositionName.Contains("заведующий"))
                    {
                        _equipment.OfficeId = _user.OfficeId;

                        _equipment.Auditoriums = _context.Auditoriums
                            .Where(a => a.OfficeId == _user.OfficeId)
                            .ToList();

                        OfficeComboBox.IsEnabled = false;
                    }
                    else
                    {
                        _equipment.Auditoriums = _context.Auditoriums.ToList();
                    }
                }   
            }
            _equipment.Auditoriums.Insert(0, new Auditorium
            {
                AuditoriumId = 0,
                Name = "Без аудитории"
            });
            DataContext = _equipment;
            IsReadOnlyMode = !(_user.Position.PositionName.Contains("администратор") ||
                   _user.Position.PositionName.Contains("заведующий"));

            SetControlsReadOnly(IsReadOnlyMode);
        }

        private void NumericTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            foreach (char i in e.Text)
            {
                if (!char.IsDigit(i))
                {
                    e.Handled = true;
                    return;
                }
            }
        }
        private void SetControlsReadOnly(bool isReadOnly)
        {
            void SetReadOnlyRecursive(UIElement element)
            {
                if (element is TextBox tb)
                    tb.IsReadOnly = isReadOnly;

                else if (element is ComboBox cb)
                    cb.IsEnabled = !isReadOnly;

                else if (element is DatePicker dp)
                    dp.IsEnabled = !isReadOnly;

                else if (element is Button btn && (btn.Name == "SaveButton" || btn.Name == "ImageButton"))
                    btn.IsEnabled = !isReadOnly;

                else if (element is Panel panel)
                {
                    foreach (UIElement child in panel.Children)
                        SetReadOnlyRecursive(child);
                }
                else if (element is ContentControl cc && cc.Content is UIElement content)
                {
                    SetReadOnlyRecursive(content);
                }
            }

            SetReadOnlyRecursive(MainGrid);
        }

        private void OfficeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedOfficeId != null)
            {
                _equipment.Auditoriums = _context.Auditoriums
                    .Where(a => a.OfficeId == SelectedOfficeId)
                    .ToList();

                AuditoriumComboBox.ItemsSource = _equipment.Auditoriums;
                SelectedAuditoriumId = null;
            }
        }

        private void Auditorium_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedAuditorium = (sender as ComboBox)?.SelectedItem as Auditorium;

            if (selectedAuditorium == null)
                return;

            if (selectedAuditorium.AuditoriumId == 0)
            {
                _equipment.AuditoriumId = null;
                if (_user.Position.PositionName != "заведующий")
                    OfficeComboBox.IsEnabled = true;
            }
            else
            {
                _equipment.AuditoriumId = selectedAuditorium.AuditoriumId;
                _equipment.OfficeId = selectedAuditorium.OfficeId;
                OfficeComboBox.IsEnabled = false;
                OfficeComboBox.SelectedValue = selectedAuditorium.OfficeId;
            }
        }


        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            if (!ValidateForm())
                return;

            if (_equipment.EquipmentId == 0)
            {
                _context.Equipments.Add(_equipment);
            }
            if (_equipment.AuditoriumId == 0)
            {
                _equipment.AuditoriumId = null;
            }
            _context.SaveChanges();

            MessageBox.Show("Данные сохранены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

            this.Close();
        }

        private void BackButton(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private bool ValidateForm()
        {
            if (string.IsNullOrEmpty(_equipment.Name) || string.IsNullOrEmpty(_equipment.InventareNum))
            {
                MessageBox.Show("Заполните все обязательные поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (_equipment.Weigth <= 0)
            {
                MessageBox.Show("Вес должен быть больше 0","Ошибка", MessageBoxButton.OK,MessageBoxImage.Warning);
                return false;
            }
            
            if (_equipment.ServiceLife <= 0)
            {
                MessageBox.Show("Срок службы должен быть больше 0","Ошибка",MessageBoxButton.OK,MessageBoxImage.Warning);
                return false;
            }
            if (_equipment.OfficeId == null && _equipment.AuditoriumId == null)
            {
                MessageBox.Show("Выберите подразделение или аудиторию", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            _equipment.InventareNum = _equipment.InventareNum.Trim();

            bool isDuplicate = _context.Equipments
                .Any(e => e.InventareNum == _equipment.InventareNum
                       && e.EquipmentId != _equipment.EquipmentId);

            if (isDuplicate)
            {
                MessageBox.Show("Инвентарный номер уже существует",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }
        private void ImageButtonClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "Image files (*.jpg;*.png)|*.jpg;*.png|All files (*.*)|*.*";

            if (dialog.ShowDialog() != true)
                return;

            string selectedFilePath = dialog.FileName;

            BitmapImage bitmap = new BitmapImage(new Uri(selectedFilePath));
            if (bitmap.PixelWidth > 300 || bitmap.PixelHeight > 200)
            {
                MessageBox.Show("Максимальный размер изображения: 300x200", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string imagesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
            if (!Directory.Exists(imagesFolder))
                Directory.CreateDirectory(imagesFolder);

            string newFileName = Guid.NewGuid() + Path.GetExtension(selectedFilePath);
            string destPath = Path.Combine(imagesFolder, newFileName);

            File.Copy(selectedFilePath, destPath, true);

            if (!string.IsNullOrEmpty(_equipment.Photo))
            {
                string oldPhotoPath = Path.Combine(imagesFolder, _equipment.Photo);
                if (File.Exists(oldPhotoPath))
                {
                    try { File.Delete(oldPhotoPath); } catch { }
                }
            }

            _equipment.Photo = newFileName;

            DataContext = null;
            DataContext = _equipment;
        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            if (_equipment == null || _equipment.EquipmentId == 0)
                return;

            DateTime balanceDate = _equipment.DateBalanceDateTime;
            DateTime endOfLife = balanceDate.AddYears(_equipment.ServiceLife);

            bool isOnSklad = _equipment.Office != null &&
                             _equipment.Office.NameOffice.ToLower().Equals("склад");

            if (!isOnSklad)
            {
                MessageBox.Show("Удалять можно только оборудование со склада", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (endOfLife >= DateTime.Now)
            {
                MessageBox.Show("Удалять можно только оборудование с превышенным сроком использования", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show("Вы уверены, что хотите удалить это оборудование?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _context.Equipments.Remove(_equipment);
                _context.SaveChanges();
                MessageBox.Show("Оборудование удалено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
        }
    }
}
