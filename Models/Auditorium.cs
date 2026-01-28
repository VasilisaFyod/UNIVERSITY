using System;
using System.Collections.Generic;

namespace UNIVERSITY.Models;

public partial class Auditorium
{
    public int AuditoriumId { get; set; }

    public string Name { get; set; } = null!;

    public int? OfficeId { get; set; }

    public virtual ICollection<Equipment> Equipment { get; set; } = new List<Equipment>();

    public virtual Office? Office { get; set; }
}
