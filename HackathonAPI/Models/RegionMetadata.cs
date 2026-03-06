using System.ComponentModel.DataAnnotations;

namespace HackathonApi.Models;

public class RegionMetadata
{
    [Display(Name = "Region Code")]
    [Required(ErrorMessage = "Region code is required.")]
    [RegularExpression("^[A-Z]{2}$", ErrorMessage = "Region code must be exactly 2 capital letters.")]
    [StringLength(2)]
    public string Code { get; set; } = "";

    [Display(Name = "Region Name")]
    [Required(ErrorMessage = "Region name is required.")]
    [StringLength(50, ErrorMessage = "Region name cannot exceed 50 characters.")]
    public string Name { get; set; } = "";
}