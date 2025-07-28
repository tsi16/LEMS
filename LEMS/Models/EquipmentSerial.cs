using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LEMS.Models;

public partial class EquipmentSerial
{
    [Key]
    public int Id { get; set; }

    [StringLength(150)]
    public string Number { get; set; } = null!;

    public int EquipmentEntryId { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    [ForeignKey("EquipmentEntryId")]
    [InverseProperty("EquipmentSerials")]
    public virtual EquipmentEntry EquipmentEntry { get; set; } = null!;

    [InverseProperty("Serial")]
    public virtual ICollection<WithdrawalSerial> WithdrawalSerials { get; set; } = new List<WithdrawalSerial>();
}
