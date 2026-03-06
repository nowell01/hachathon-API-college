using HackathonApi.Metadata;
using Microsoft.AspNetCore.Mvc;

namespace HackathonApi.Models;

[ModelMetadataType(typeof(RegionMetadata))]
public partial class Region
{
    public int ID { get; set; }

    public string Code { get; set; } = "";
    public string Name { get; set; } = "";

    public ICollection<Member> Members { get; set; } = new List<Member>();

    public byte[]? RowVersion { get; set; }
}