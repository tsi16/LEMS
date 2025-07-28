using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LEMS.Models;

public partial class LabratoryAssignment
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }

    public int LabratoryId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime AssignedDate { get; set; }

    public int ActionBy { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    [ForeignKey("ActionBy")]
    [InverseProperty("LabratoryAssignmentActionByNavigations")]
    public virtual User ActionByNavigation { get; set; } = null!;

    [ForeignKey("LabratoryId")]
    [InverseProperty("LabratoryAssignments")]
    public virtual Labratory Labratory { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("LabratoryAssignmentUsers")]
    public virtual User User { get; set; } = null!;
}
