using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LEMS.Models;

[Table("laboratoryEquipments")]
public partial class LaboratoryEquipment
{
    [Key]
    public int Id { get; set; }

    public int DepartmentId { get; set; }

    public double Quantity { get; set; }

    public int LabratoryId { get; set; }

    public int EquipmentId { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    [ForeignKey("DepartmentId")]
    [InverseProperty("LaboratoryEquipments")]
    public virtual Department Department { get; set; } = null!;

    [ForeignKey("EquipmentId")]
    [InverseProperty("LaboratoryEquipments")]
    public virtual Equipment Equipment { get; set; } = null!;

    [ForeignKey("LabratoryId")]
    [InverseProperty("LaboratoryEquipments")]
    public virtual Labratory Labratory { get; set; } = null!;
}
