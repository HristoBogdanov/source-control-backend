using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SourceControlApiV2.Data;
using SourceControlApiV2.DTOs.Commit;
using SourceControlApiV2.DTOs.PullRequest;
using SourceControlApiV2.Extensions;
using SourceControlApiV2.Models;

namespace SourceControlApiV2.Controllers
{
    [Route("api/pull-request")]
    [ApiController]
    [Authorize]
    public class PullRequestController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PullRequestController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllPullRequests()
        {
            var user = await _userManager.FindByNameAsync(User.GetUsername());

            try
            {
                var pullRequests = await _context.PullRequests
                    .Where(p => p.IsDeleted == false)
                    .Where(p => p.SourceRepository.Owner.Id == user.Id)
                    .Where(p => p.IsResolved == false)
                    .OrderBy(p => p.CreatedAt)
                    .Select(pr => new PullRequestDTO()
                    {
                        Title = pr.Title,
                        Description = pr.Description,
                        RepositoryId = pr.RepositoryId,
                        CreatorId = pr.CreatorId,
                        Commits = pr.Commits.Select(c => new CommitDTO()
                        {
                            Title = c.Title,
                            AuthorId = c.AuthorId,
                            RepositoryId = c.RepositoryId
                        }).ToList()
                    }).ToListAsync();

                return Ok(pullRequests);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> AddPullRequestToRepository([FromQuery] string repositoryId, [FromBody] PullRequestDTO pullRequestDTO)
        {
            var repositoryIdGuid = Guid.Parse(repositoryId);

            var repository = await _context.Repositories.Where(r => r.IsDeleted == false).FirstOrDefaultAsync(r => r.Id == repositoryIdGuid);

            if (repository == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByNameAsync(User.GetUsername());

            try
            {
                var pullRequest = new PullRequest
                {
                    Title = pullRequestDTO.Title,
                    Description = pullRequestDTO.Description,
                    RepositoryId = Guid.Parse(repositoryId),
                    CreatorId = user.Id
            };

                pullRequest.Commits = pullRequestDTO.Commits.Select(c => new Commit
                {
                    Title = c.Title,
                    AuthorId = user.Id,
                    RepositoryId = repositoryIdGuid
                }).ToList();

                await _context.PullRequests.AddAsync(pullRequest);
                await _context.SaveChangesAsync();

                return Ok(pullRequest);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("accept/{pullRequestId}")]
        public async Task<IActionResult> ResolvePullRequest([FromRoute] string pullRequestId)
        {
            var pullRequestIdGuid = Guid.Parse(pullRequestId);

            var pullRequest = await _context.PullRequests.Where(p => p.IsDeleted == false).FirstOrDefaultAsync(p => p.Id == pullRequestIdGuid);

            if (pullRequest == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByNameAsync(User.GetUsername());

            if (pullRequest.SourceRepository.Owner.Id != user.Id)
            {
                return Unauthorized();
            }

            pullRequest.IsResolved = true;

            var repository = await _context.Repositories.Where(r => r.IsDeleted == false)
                .FirstOrDefaultAsync(r => r.Id == pullRequest.SourceRepository.Id);

            foreach (var commit in pullRequest.Commits)
            {
                repository!.Commits.Add(commit);
            }

            await _context.SaveChangesAsync();
            return Ok(pullRequest);
        }

        [HttpPost("reject/{pullRequestId}")]
        public async Task<IActionResult> RejectPullRequest([FromRoute] string pullRequestId)
        {
            var pullRequestIdGuid = Guid.Parse(pullRequestId);

            var pullRequest = await _context.PullRequests.Where(p => p.IsDeleted == false).FirstOrDefaultAsync(p => p.Id == pullRequestIdGuid);

            if (pullRequest == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByNameAsync(User.GetUsername());

            if (pullRequest.SourceRepository.Owner.Id != user.Id)
            {
                return Unauthorized();
            }

            pullRequest.IsResolved = false;

            await _context.SaveChangesAsync();
            return Ok($"Pull request: ${pullRequest} was rejected");
        }
    }
}
