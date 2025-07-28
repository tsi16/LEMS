using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LEMS.Models;

public partial class Equipment
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    public int ModelId { get; set; }

    public int ManufacturerId { get; set; }

    [StringLength(150)]
    public string SerialNumber { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime PurchaseDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime MaintenanceDate { get; set; }

    [StringLength(50)]
    public string Location { get; set; } = null!;

    public int MeasurmentUnitId { get; set; }

    [StringLength(50)]
    public string Code { get; set; } = null!;

    public int EquipmentsTypeId { get; set; }

    [StringLength(150)]
    public string PhotoUrl { get; set; } = null!;

    [StringLength(150)]
    public string Discription { get; set; } = null!;

    public bool IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    [InverseProperty("Equipment")]
    public virtual ICollection<EquipmentEntry> EquipmentEntries { get; set; } = new List<EquipmentEntry>();

    [InverseProperty("Equipment")]
    public virtual ICollection<EquipmentUse> EquipmentUses { get; set; } = new List<EquipmentUse>();

    [ForeignKey("EquipmentsTypeId")]
    [InverseProperty("Equipment")]
    public virtual EquipmentsType EquipmentsType { get; set; } = null!;

    [InverseProperty("Equipment")]
    public virtual ICollection<LaboratoryEquipment> LaboratoryEquipments { get; set; } = new List<LaboratoryEquipment>();

    [ForeignKey("ManufacturerId")]
    [InverseProperty("Equipment")]
    public virtual Manufacturer Manufacturer { get; set; } = null!;

    [ForeignKey("MeasurmentUnitId")]
    [InverseProperty("Equipment")]
    public virtual MeasurmentUnit MeasurmentUnit { get; set; } = null!;

    [ForeignKey("ModelId")]
    [InverseProperty("Equipment")]
    public virtual Model Model { get; set; } = null!;
}
