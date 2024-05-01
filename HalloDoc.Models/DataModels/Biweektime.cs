using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataModels.DataModels;

[Table("biweektime")]
public partial class Biweektime
{
    [Key]
    [Column("biweekid")]
    public int Biweekid { get; set; }

    [Column("physicianid")]
    public int? Physicianid { get; set; }

    [Column("isfinalized")]
    public bool? Isfinalized { get; set; }

    [Column("firstday", TypeName = "timestamp without time zone")]
    public DateTime? Firstday { get; set; }

    [Column("lastday", TypeName = "timestamp without time zone")]
    public DateTime? Lastday { get; set; }

    [ForeignKey("Physicianid")]
    [InverseProperty("Biweektimes")]
    public virtual Physician? Physician { get; set; }

    [InverseProperty("Biweektime")]
    public virtual ICollection<Reimbursement> Reimbursements { get; set; } = new List<Reimbursement>();

    [InverseProperty("Biweektime")]
    public virtual ICollection<Timesheet> Timesheets { get; set; } = new List<Timesheet>();
}
