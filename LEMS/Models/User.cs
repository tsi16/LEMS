using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LEMS.Models;

public partial class User
{
    [Key]
    public int Id { get; set; }

    public int GenderId { get; set; }

    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    public string MiddleName { get; set; } = null!;

    [StringLength(50)]
    public string? LastName { get; set; }

    [StringLength(50)]
    public string? Email { get; set; }

    [StringLength(200)]
    public string Password { get; set; } = null!;

    [StringLength(50)]
    public string UserName { get; set; } = null!;

    [StringLength(50)]
    public string? PhoneNumber { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastLogon { get; set; }

    public int? FailureCount { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? BlockEndDate { get; set; }

    public int? DefaultLanguageId { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    [InverseProperty("ActionByNavigation")]
    public virtual ICollection<EquipmentEntry> EquipmentEntries { get; set; } = new List<EquipmentEntry>();

    [ForeignKey("GenderId")]
    [InverseProperty("Users")]
    public virtual Gender Gender { get; set; } = null!;

    [InverseProperty("ActionByNavigation")]
    public virtual ICollection<LabratoryAssignment> LabratoryAssignmentActionByNavigations { get; set; } = new List<LabratoryAssignment>();

    [InverseProperty("User")]
    public virtual ICollection<LabratoryAssignment> LabratoryAssignmentUsers { get; set; } = new List<LabratoryAssignment>();

    [InverseProperty("User")]
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

    [InverseProperty("ActionByNavigation")]
    public virtual ICollection<Withdrawl> Withdrawls { get; set; } = new List<Withdrawl>();
}
