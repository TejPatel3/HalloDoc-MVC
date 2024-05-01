using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataModels.DataModels;

[Table("payrate")]
public partial class Payrate
{
    [Key]
    [Column("payrateid")]
    public int Payrateid { get; set; }

    [Column("physicinaid")]
    public int? Physicinaid { get; set; }

    [Column("nightshift")]
    public int? Nightshift { get; set; }

    [Column("shift")]
    public int? Shift { get; set; }

    [Column("housecall")]
    public int? Housecall { get; set; }

    [Column("nighthousecall")]
    public int? Nighthousecall { get; set; }

    [Column("consult")]
    public int? Consult { get; set; }

    [Column("nightconsult")]
    public int? Nightconsult { get; set; }

    [Column("batchtesting")]
    public int? Batchtesting { get; set; }

    [Column("modifiedby")]
    [StringLength(128)]
    public string? Modifiedby { get; set; }

    [Column("modifieddate", TypeName = "timestamp without time zone")]
    public DateTime? Modifieddate { get; set; }

    [ForeignKey("Physicinaid")]
    [InverseProperty("Payrates")]
    public virtual Physician? Physicina { get; set; }
}
