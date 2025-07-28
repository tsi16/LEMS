using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LEMS.Models;

public partial class WithdrawalSerial
{
    [Key]
    public int Id { get; set; }

    public int WithdrawalId { get; set; }

    public int SerialId { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    [ForeignKey("SerialId")]
    [InverseProperty("WithdrawalSerials")]
    public virtual EquipmentSerial Serial { get; set; } = null!;

    [ForeignKey("WithdrawalId")]
    [InverseProperty("WithdrawalSerials")]
    public virtual Withdrawl Withdrawal { get; set; } = null!;
}
