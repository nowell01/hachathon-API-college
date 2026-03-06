using HackathonApi.Metadata;
using HackathonApi.Models;
using Microsoft.AspNetCore.Mvc;
using HackathonAPI.Metadata;

namespace HackathonApi.DTO
{
    [ModelMetadataType(typeof(MemberMetadata))]
    public class ChallengeDTO
    {
        public int ID { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }

        public List<MemberDTO> Members { get; set; } = new();
    }
}