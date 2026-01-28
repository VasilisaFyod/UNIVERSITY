using System;
using System.Collections.Generic;

namespace UNIVERSITY.Models;

public partial class Office
{
    public int OfficeId { get; set; }

    public string Level { get; set; } = null!;

    public string FullNameOffice { get; set; } = null!;

    public string NameOffice { get; set; } = null!;

    public virtual ICollection<Auditorium> Auditoria { get; set; } = new List<Auditorium>();

    public virtual ICollection<Equipment> Equipment { get; set; } = new List<Equipment>();

    public virtual ICollection<Worker> Workers { get; set; } = new List<Worker>();
}
