namespace HackathonApi.DTO
{
    public class ChallengeReadDto
    {
        public int ID { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
    }

    public class ChallengeWithMembersDto
    {
        public int ID { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }

        public List<MemberDTO> Members { get; set; } = new();
    }
}