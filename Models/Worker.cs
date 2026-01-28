using System;
using System.Collections.Generic;

namespace UNIVERSITY.Models;

public partial class Worker
{
    public int WorkerId { get; set; }

    public string Lastname { get; set; } = null!;

    public string Firstname { get; set; } = null!;

    public string? Fathername { get; set; }

    public int PositionId { get; set; }

    public int BirthYear { get; set; }

    public int OfficeId { get; set; }

    public string? Login { get; set; }

    public string? Password { get; set; }

    public virtual Office Office { get; set; } = null!;

    public virtual Position Position { get; set; } = null!;
}
