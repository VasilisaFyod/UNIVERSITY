using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UNIVERSITY.Models;

[Index("InventareNum", Name = "NonClusteredIndex-20260121-201852", IsUnique = true)]
public partial class Equipment
{
    [Key]
    [Column("equipment_id")]
    public int EquipmentId { get; set; }

    [Column("weigth")]
    public double Weigth { get; set; }

    [Column("date_balance")]
    public DateOnly DateBalance { get; set; }

    [Column("inventare_num")]
    [StringLength(50)]
    [Unicode(false)]
    public string InventareNum { get; set; } = null!;

    [Column("photo")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Photo { get; set; }

    [Column("service_life")]
    public int ServiceLife { get; set; }

    [Column("description")]
    [Unicode(false)]
    public string? Description { get; set; }

    [Column("name")]
    [StringLength(50)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [Column("auditorium_id")]
    public int AuditoriumId { get; set; }

    [ForeignKey("AuditoriumId")]
    [InverseProperty("Equipment")]
    public virtual Auditorium Auditorium { get; set; } = null!;
}
