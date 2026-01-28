using System;
using System.Collections.Generic;

namespace UNIVERSITY.Models;

public partial class Position
{
    public int PositionId { get; set; }

    public string PositionName { get; set; } = null!;

    public decimal Salary { get; set; }

    public virtual ICollection<Worker> Workers { get; set; } = new List<Worker>();
}
