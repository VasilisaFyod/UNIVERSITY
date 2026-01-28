using System;
using System.Collections.Generic;

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
}
