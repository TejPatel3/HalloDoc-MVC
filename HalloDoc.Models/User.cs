using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalloDoc.DataModels;

[Table("User")]
public partial class User
{
    [Key]
    public int UserId { get; set; }

    [StringLength(128)]
    public string? Id { get; set; }

    [Required(ErrorMessage = "First Name is required"), Display(Name = "First Name")]
    [StringLength(100)]
    [RegularExpression(@"^(?=.*\S)[a-zA-Z\s.'-]+$", ErrorMessage = "Enter a valid first name")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Last Name is required"), Display(Name = "Last Name")]
    [StringLength(100)]
    [RegularExpression(@"^(?=.*\S)[a-zA-Z\s.'-]+$", ErrorMessage = "Enter a valid first name")]
    public string? LastName { get; set; }

    [StringLength(50)]
    public string Email { get; set; } = null!;

    [StringLength(23)]
    public string? Mobile { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? IsMobile { get; set; }

    [StringLength(100, MinimumLength = 2, ErrorMessage = "Enter valid Street")]
    [RegularExpression(@"^(?=.*\S)[a-zA-Z0-9\s.,'-]+$", ErrorMessage = "Enter a valid street address")]
    public string? Street { get; set; }

    [StringLength(100, MinimumLength = 2, ErrorMessage = "Enter valid City")]
    [RegularExpression(@"^(?=.*\S)[a-zA-Z\s.'-]+$", ErrorMessage = "Enter a valid city name")]
    public string? City { get; set; }

    [StringLength(50, MinimumLength = 2, ErrorMessage = "Enter valid State")]
    [RegularExpression(@"^(?=.*\S)[a-zA-Z\s.'-]+$", ErrorMessage = "Enter a valid State name")]
    public string? State { get; set; }

    public int? RegionId { get; set; }

    [StringLength(10, ErrorMessage = "Enter valid Zip Code")]
    [RegularExpression(@"^\d{5}$", ErrorMessage = "Enter a valid 5-digit zip code")]
    public string? ZipCode { get; set; }

    [Column("strMonth")]
    [StringLength(20)]
    public string? StrMonth { get; set; }

    [Column("intYear")]
    public int? IntYear { get; set; }

    [Column("intDate")]
    public int? IntDate { get; set; }

    [StringLength(128)]
    public string CreatedBy { get; set; } = null!;

    [Column(TypeName = "timestamp without time zone")]
    public DateTime CreatedDate { get; set; }

    [StringLength(128)]
    public string? ModifiedBy { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? ModifiedDate { get; set; }

    public short? Status { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? IsDeleted { get; set; }

    [Column("IP")]
    [StringLength(20)]
    public string? Ip { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? IsRequestWithEmail { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("UserCreatedByNavigations")]
    public virtual AspNetUser CreatedByNavigation { get; set; } = null!;

    [ForeignKey("Id")]
    [InverseProperty("UserIdNavigations")]
    public virtual AspNetUser? IdNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("UserModifiedByNavigations")]
    public virtual AspNetUser? ModifiedByNavigation { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
}
