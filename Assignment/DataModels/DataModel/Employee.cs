using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Assignment;

[Table("Employee")]
public partial class Employee
{
    [Key]
    public int Id { get; set; }

    [StringLength(80)]
    public string FirstName { get; set; } = null!;

    [StringLength(80)]
    public string? LastName { get; set; }

    public int DeptId { get; set; }

    public int? Age { get; set; }

    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(80)]
    public string? Education { get; set; }

    [StringLength(80)]
    public string? Company { get; set; }

    [StringLength(80)]
    public string? Experience { get; set; }

    [StringLength(80)]
    public string? Package { get; set; }

    [StringLength(20)]
    public string? Gender { get; set; }

    [ForeignKey("DeptId")]
    [InverseProperty("Employees")]
    public virtual Department Dept { get; set; } = null!;
}
