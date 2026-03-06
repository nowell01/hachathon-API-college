using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace HackathonApi.Models;

public interface IAuditable
{
    string? CreatedBy { get; set; }
    DateTime? CreatedOn { get; set; }
    string? UpdatedBy { get; set; }
    DateTime? UpdatedOn { get; set; }
}

public abstract class Auditable : IAuditable
{
    [ScaffoldColumn(false)]
    [StringLength(256)]
    public string? CreatedBy { get; set; } = "Unknown";

    [ScaffoldColumn(false)]
    public DateTime? CreatedOn { get; set; }

    [ScaffoldColumn(false)]
    [StringLength(256)]
    public string? UpdatedBy { get; set; } = "Unknown";

    [ScaffoldColumn(false)]
    public DateTime? UpdatedOn { get; set; }

    [ScaffoldColumn(false)]
    [Timestamp]
    public byte[]? RowVersion { get; set; }
}