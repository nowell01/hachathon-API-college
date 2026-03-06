namespace HackathonApi.DTO
{
    public class RegionReadDto
    {
        public int ID { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
    }

    public class RegionWithMembersDto
    {
        public int ID { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }

        public List<MemberDTO> Members { get; set; } = new();
    }
}