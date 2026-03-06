using System.ComponentModel.DataAnnotations;

namespace HackathonApi.Metadata;

public class MemberMetadata
{
    [Display(Name = "First Name")]
    [Required(ErrorMessage = "First name is required.")]
    [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
    public string FirstName { get; set; } = "";

    [Display(Name = "Middle Name")]
    [StringLength(50, ErrorMessage = "Middle name cannot exceed 50 characters.")]
    public string? MiddleName { get; set; }

    [Display(Name = "Last Name")]
    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters.")]
    public string LastName { get; set; } = "";

    [Display(Name = "Member Code")]
    [Required(ErrorMessage = "Member code is required.")]
    [RegularExpression("^\\d{7}$", ErrorMessage = "Member code must be exactly 7 digits.")]
    [StringLength(7)]
    public string MemberCode { get; set; } = "0000000";

    [Display(Name = "Date of Birth")]
    [Required(ErrorMessage = "Date of birth is required.")]
    [DataType(DataType.Date)]
    public DateTime DOB { get; set; }

    [Display(Name = "Skill Rating")]
    [Required(ErrorMessage = "Skill rating is required.")]
    [Range(1, 10, ErrorMessage = "Skill rating must be between 1 and 10.")]
    public int SkillRating { get; set; }

    [Display(Name = "Years of Experience")]
    [Required(ErrorMessage = "Years of experience is required.")]
    [Range(0, 20, ErrorMessage = "Years of experience must be between 0 and 20.")]
    public int YearsExperience { get; set; }

    [Display(Name = "Competition Category")]
    [Required(ErrorMessage = "Category is required.")]
    [RegularExpression("^[JA]$", ErrorMessage = "Category must be either J (Junior) or A (Advanced).")]
    public string Category { get; set; } = "";

    [Display(Name = "Organization")]
    [Required(ErrorMessage = "Organization is required.")]
    [StringLength(255, ErrorMessage = "Organization name cannot exceed 255 characters.")]
    public string Organization { get; set; } = "";

    [Timestamp]
    public Byte[]? RowVersion { get; set; }
}