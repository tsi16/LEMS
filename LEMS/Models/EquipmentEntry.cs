using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LEMS.Models;

public partial class EquipmentEntry
{
    [Key]
    public int Id { get; set; }

    public int EquipmentId { get; set; }

    public int LabratoryId { get; set; }

    public double Quantity { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime EntryDate { get; set; }

    public double WithdrawalQuantity { get; set; }

    public int ActionBy { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    [ForeignKey("ActionBy")]
    [InverseProperty("EquipmentEntries")]
    public virtual User ActionByNavigation { get; set; } = null!;

    [ForeignKey("EquipmentId")]
    [InverseProperty("EquipmentEntries")]
    public virtual Equipment Equipment { get; set; } = null!;

    [InverseProperty("EquipmentEntry")]
    public virtual ICollection<EquipmentSerial> EquipmentSerials { get; set; } = new List<EquipmentSerial>();

    [ForeignKey("LabratoryId")]
    [InverseProperty("EquipmentEntries")]
    public virtual Labratory Labratory { get; set; } = null!;

    [InverseProperty("Entry")]
    public virtual ICollection<Withdrawl> Withdrawls { get; set; } = new List<Withdrawl>();
}
