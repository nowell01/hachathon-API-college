using HackathonApi.Data;
using HackathonApi.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HackathonApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RegionController : ControllerBase
{
    private readonly HackathonContext _context;

    public RegionController(HackathonContext context)
    {
        _context = context;
    }

    // GET: api/Region
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RegionReadDto>>> GetAll()
    {
        var regions = await _context.Regions
            .AsNoTracking()
            .OrderBy(r => r.Name)
            .ToListAsync();

        return Ok(regions.Select(r => new RegionReadDto { ID = r.ID, Code = r.Code, Name = r.Name }));
    }

    // GET: api/Region/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<RegionReadDto>> GetById(int id)
    {
        var r = await _context.Regions.AsNoTracking().FirstOrDefaultAsync(x => x.ID == id);
        if (r == null) return NotFound();

        return Ok(new RegionReadDto { ID = r.ID, Code = r.Code, Name = r.Name });
    }

    // GET: api/Region/inc
    [HttpGet("inc")]
    public async Task<ActionResult<IEnumerable<RegionWithMembersDto>>> GetAllWithMembers()
    {
        var regions = await _context.Regions
            .AsNoTracking()
            .Include(r => r.Members)
                .ThenInclude(m => m.Challenge)
            .OrderBy(r => r.Name)
            .ToListAsync();

        var result = regions.Select(r => new RegionWithMembersDto
        {
            ID = r.ID,
            Code = r.Code,
            Name = r.Name,
            Members = r.Members.Select(m => new MemberDTO
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

    // GET: api/Region/inc/{id}
    [HttpGet("inc/{id:int}")]
    public async Task<ActionResult<RegionWithMembersDto>> GetByIdWithMembers(int id)
    {
        var r = await _context.Regions
            .AsNoTracking()
            .Include(x => x.Members)
                .ThenInclude(m => m.Challenge)
            .FirstOrDefaultAsync(x => x.ID == id);

        if (r == null) return NotFound();

        var dto = new RegionWithMembersDto
        {
            ID = r.ID,
            Code = r.Code,
            Name = r.Name,
            Members = r.Members.Select(m => new MemberDTO
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