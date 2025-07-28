using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LEMS.Models;

public partial class Department
{
    [Key]
    public int Id { get; set; }

    public int BranchId { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    [ForeignKey("BranchId")]
    [InverseProperty("Departments")]
    public virtual Branch Branch { get; set; } = null!;

    [InverseProperty("Department")]
    public virtual ICollection<LaboratoryEquipment> LaboratoryEquipments { get; set; } = new List<LaboratoryEquipment>();

    [InverseProperty("Department")]
    public virtual ICollection<Labratory> Labratories { get; set; } = new List<Labratory>();
}
