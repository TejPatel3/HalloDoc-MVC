using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Assignment;

[Table("Department")]
public partial class Department
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "character varying")]
    public string Name { get; set; } = null!;

    [InverseProperty("Dept")]
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
