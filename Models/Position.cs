using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UNIVERSITY.Models;

public partial class Position
{
    [Key]
    [Column("position_id")]
    public int PositionId { get; set; }

    [Column("position_name")]
    [StringLength(50)]
    [Unicode(false)]
    public string PositionName { get; set; } = null!;

    [Column("salary", TypeName = "money")]
    public decimal Salary { get; set; }

    [InverseProperty("Position")]
    public virtual ICollection<Worker> Workers { get; set; } = new List<Worker>();
}
