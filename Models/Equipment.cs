using System.IO;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Media.Imaging;

namespace UNIVERSITY.Models;

public partial class Equipment
{

    public int EquipmentId { get; set; }

    public double Weigth { get; set; }

    public DateOnly DateBalance { get; set; }

    public string InventareNum { get; set; } = null!;

    public string? Photo { get; set; }

    public int ServiceLife { get; set; }

    public string? Description { get; set; }

    public string Name { get; set; } = null!;

    public int? AuditoriumId { get; set; }

    public int? OfficeId { get; set; }

    public virtual Auditorium? Auditorium { get; set; }

    public virtual Office? Office { get; set; }

    [NotMapped]
    public List<Office> Offices { get; set; }
    [NotMapped]
    public List<Auditorium> Auditoriums { get; set; }
    [NotMapped]
    public BitmapImage PhotoImage
    {
        get
        {
            string fileName = string.IsNullOrWhiteSpace(Photo)
            ? "stub.jpg"
            : Photo.Trim();

            string path = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Images",
                fileName);

            if (!File.Exists(path))
                return null;

            return new BitmapImage(new Uri(path, UriKind.Absolute));
        }
    }
    [NotMapped]
    public DateTime DateBalanceDateTime
    {
        get => DateBalance.ToDateTime(TimeOnly.MinValue);
        set => DateBalance = DateOnly.FromDateTime(value);
    }
}
