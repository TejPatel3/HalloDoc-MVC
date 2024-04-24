using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment;

[Table("Login")]
public class Login
{
    [Column(TypeName = "character varying")]
    public string? Email { get; set; }

    [Column(TypeName = "character varying")]
    public string? Password { get; set; }

    [Key]
    public int Id { get; set; }
}
