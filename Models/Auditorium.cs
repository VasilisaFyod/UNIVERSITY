using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UNIVERSITY.Models;

public partial class Auditorium
{
    [Key]
    [Column("auditorium_id")]
    public int AuditoriumId { get; set; }

    [Column("name")]
    [StringLength(50)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [Column("office_id")]
    public int? OfficeId { get; set; }

    [InverseProperty("Auditorium")]
    public virtual ICollection<Equipment> Equipment { get; set; } = new List<Equipment>();

    [ForeignKey("OfficeId")]
    [InverseProperty("Auditoria")]
    public virtual Office? Office { get; set; }
}
