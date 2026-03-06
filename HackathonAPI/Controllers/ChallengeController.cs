using HackathonApi.Data;
using HackathonApi.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HackathonApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChallengeController : ControllerBase
{
    private readonly HackathonContext _context;

    public ChallengeController(HackathonContext context)
    {
        _context = context;
    }

    // GET: api/Challenge
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ChallengeDTO>>> GetAll()
    {
        var challenges = await _context.Challenges
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .ToListAsync();

        return Ok(challenges.Select(c => new ChallengeDTO { ID = c.ID, Code = c.Code, Name = c.Name }));
    }

    // GET: api/Challenge/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ChallengeDTO>> GetById(int id)
    {
        var c = await _context.Challenges.AsNoTracking().FirstOrDefaultAsync(x => x.ID == id);
        if (c == null) return NotFound();

        return Ok(new ChallengeDTO { ID = c.ID, Code = c.Code, Name = c.Name });
    }

    // GET: api/Challenge/inc
    [HttpGet("inc")]
    public async Task<ActionResult<IEnumerable<ChallengeDTO>>> GetAllWithMembers()
    {
        var challenges = await _context.Challenges
            .AsNoTracking()
            .Include(c => c.Members)
                .ThenInclude(m => m.Region)
            .OrderBy(c => c.Name)
            .ToListAsync();

        var result = challenges.Select(c => new ChallengeDTO
        {
            ID = c.ID,
            Code = c.Code,
            Name = c.Name,
            Members = c.Members.Select(m => new MemberDTO
            {
                ID = m.ID,
                MemberCode = m.MemberCode,
                DOB = m.DOB,
                SkillRating = m.SkillRating,
                YearsExperience = m.YearsExperience,
                Category = m.Category,
                Organization = m.Organization,
                RegionID = m.RegionID,
                ChallengeID = m.ChallengeID,
                RowVersion = m.RowVersion
            }).ToList()
        });

        return Ok(result);
    }

    // GET: api/Challenge/inc/{id}
    [HttpGet("inc/{id:int}")]
    public async Task<ActionResult<ChallengeDTO>> GetByIdWithMembers(int id)
    {
        var c = await _context.Challenges
            .AsNoTracking()
            .Include(x => x.Members)
                .ThenInclude(m => m.Region)
            .FirstOrDefaultAsync(x => x.ID == id);

        if (c == null) return NotFound();

        var dto = new ChallengeDTO
        {
            ID = c.ID,
            Code = c.Code,
            Name = c.Name,
            Members = c.Members.Select(m => new MemberDTO
            {
                ID = m.ID,
                MemberCode = m.MemberCode,
                DOB = m.DOB,
                SkillRating = m.SkillRating,
                YearsExperience = m.YearsExperience,
                Category = m.Category,
                Organization = m.Organization,
                RegionID = m.RegionID,
                ChallengeID = m.ChallengeID,
                RowVersion = m.RowVersion
            }).ToList()
        };

        return Ok(dto);
    }
}