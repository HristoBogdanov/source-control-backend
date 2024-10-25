using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SourceControlApiV2.Data;
using SourceControlApiV2.Extensions;
using SourceControlApiV2.Models;

namespace SourceControlApiV2.Controllers
{
    [Route("api/commit")]
    [ApiController]
    [Authorize]
    public class CommitController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommitController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllCommits([FromQuery] string repositoryId, [FromQuery] string? search, [FromQuery] string? filterByAuthor)
        {
            var repositoryIdGuid = Guid.Parse(repositoryId);

            var repository = await _context.Repositories.Where(r => r.IsDeleted == false).FirstOrDefaultAsync(r => r.Id == repositoryIdGuid);

            if (repository == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByNameAsync(User.GetUsername());

            if (!repository.Contributors.Any(c => c.UserId == user.Id))
            {
                return Unauthorized();
            }

            try
            {
                var commits = await _context.Commits
                    .Where(c => c.RepositoryId == repositoryIdGuid)
                    .Where(c => search == null || c.Title.Contains(search))
                    .Where(c => filterByAuthor == null || c.Author.UserName == filterByAuthor)
                    .OrderBy(c => c.CreatedAt)
                    .Include(c => c.Author)
                    .Include(c => c.Modifications)
                    .ToListAsync();

                return Ok(commits);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCommit([FromQuery] string repositoryId, [FromBody] CommitDTO dto)
        {
            var repositoryIdGuid = Guid.Parse(repositoryId);

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var repository = await _context.Repositories.Where(r => r.IsDeleted==false).FirstOrDefaultAsync(r=>r.Id == repositoryIdGuid);

            if (repository == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByNameAsync(User.GetUsername());

            if(!repository.Contributors.Any(c=> c.UserId == user.Id))
            {
                return Unauthorized();
            }

            try
            {
                var commit = new Commit
                {
                    Title = dto.Title,
                    RepositoryId = repositoryIdGuid,
                    AuthorId = user.Id
            };

                commit.Modifications = dto.Modifications.Select(m => new Modification
                {
                    FileName = m.FileName,
                    FileDifferences = m.FileDifferences,
                    modificationType = m.modificationType,
                    Commit = commit
                }).ToList();

                await _context.Commits.AddAsync(commit);
                await _context.Modifications.AddRangeAsync(commit.Modifications);
                await _context.SaveChangesAsync();

                return Ok(commit);
            }
            catch
            (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
