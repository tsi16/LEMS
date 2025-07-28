using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LEMS.Models;

public partial class Labratory
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    public int DepartmentId { get; set; }

    [StringLength(70)]
    public string Location { get; set; } = null!;

    [StringLength(150)]
    public string Description { get; set; } = null!;

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    [ForeignKey("DepartmentId")]
    [InverseProperty("Labratories")]
    public virtual Department Department { get; set; } = null!;

    [InverseProperty("Labratory")]
    public virtual ICollection<EquipmentEntry> EquipmentEntries { get; set; } = new List<EquipmentEntry>();

    [InverseProperty("Labratory")]
    public virtual ICollection<LaboratoryEquipment> LaboratoryEquipments { get; set; } = new List<LaboratoryEquipment>();

    [InverseProperty("Labratory")]
    public virtual ICollection<LabratoryAssignment> LabratoryAssignments { get; set; } = new List<LabratoryAssignment>();
}
