using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UNIVERSITY.Models;

public partial class Office
{
    [Key]
    [Column("office_id")]
    public int OfficeId { get; set; }

    [Column("level")]
    [StringLength(50)]
    [Unicode(false)]
    public string Level { get; set; } = null!;

    [Column("full_name_office")]
    [StringLength(100)]
    [Unicode(false)]
    public string FullNameOffice { get; set; } = null!;

    [Column("name_office")]
    [StringLength(50)]
    [Unicode(false)]
    public string NameOffice { get; set; } = null!;

    [InverseProperty("Office")]
    public virtual ICollection<Auditorium> Auditoria { get; set; } = new List<Auditorium>();

    [InverseProperty("Office")]
    public virtual ICollection<Worker> Workers { get; set; } = new List<Worker>();
}
