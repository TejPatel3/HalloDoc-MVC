using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataModels.DataModels;

[Table("timesheet")]
public partial class Timesheet
{
    [Key]
    [Column("timesheetid")]
    public int Timesheetid { get; set; }

    [Column("physicianid")]
    public int? Physicianid { get; set; }

    [Column("isweekend")]
    public bool? Isweekend { get; set; }

    [Column("oncallhours")]
    public decimal? Oncallhours { get; set; }

    [Column("housecall")]
    public int? Housecall { get; set; }

    [Column("consult")]
    public int? Consult { get; set; }

    [Column("createdby", TypeName = "character varying")]
    public string? Createdby { get; set; }

    [Column("modifiedby", TypeName = "character varying")]
    public string? Modifiedby { get; set; }

    [Column("biweektimeid")]
    public int? Biweektimeid { get; set; }

    [Column("date", TypeName = "timestamp without time zone")]
    public DateTime? Date { get; set; }

    [Column("modifieddate", TypeName = "timestamp without time zone")]
    public DateTime? Modifieddate { get; set; }

    [Column("createddate", TypeName = "timestamp without time zone")]
    public DateTime? Createddate { get; set; }

    [ForeignKey("Biweektimeid")]
    [InverseProperty("Timesheets")]
    public virtual Biweektime? Biweektime { get; set; }

    [ForeignKey("Physicianid")]
    [InverseProperty("Timesheets")]
    public virtual Physician? Physician { get; set; }
}
