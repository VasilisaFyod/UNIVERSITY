using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UNIVERSITY.Models;

public partial class Worker
{
    [Key]
    [Column("worker_id")]
    public int WorkerId { get; set; }

    [Column("lastname")]
    [StringLength(50)]
    [Unicode(false)]
    public string Lastname { get; set; } = null!;

    [Column("firstname")]
    [StringLength(50)]
    [Unicode(false)]
    public string Firstname { get; set; } = null!;

    [Column("fathername")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Fathername { get; set; }

    [Column("position_id")]
    public int PositionId { get; set; }

    [Column("birth_year")]
    public int BirthYear { get; set; }

    [Column("office_id")]
    public int OfficeId { get; set; }

    [Column("login")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Login { get; set; }

    [Column("password")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Password { get; set; }

    [ForeignKey("OfficeId")]
    [InverseProperty("Workers")]
    public virtual Office Office { get; set; } = null!;

    [ForeignKey("PositionId")]
    [InverseProperty("Workers")]
    public virtual Position Position { get; set; } = null!;
}
