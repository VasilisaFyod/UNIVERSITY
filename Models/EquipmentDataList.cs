using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace UNIVERSITY
{
public partial class EquipmentListWindow
    {
        public class EquipmentDataList
        {
            public int EquipmentId { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }

            public BitmapImage Photo { get; set; }

            public string Auditorium { get; set; }

            public string FullNameOffice { get; set; }
            public string Status { get; set; }
            public Brush StatusBrush { get; set; }

        }
    }
} 