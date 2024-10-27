using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SourceControlApiV2.Data;
using SourceControlApiV2.DTOs.Issue;
using SourceControlApiV2.Extensions;
using SourceControlApiV2.Models;

namespace SourceControlApiV2.Controllers
{
    [Route("api/issue")]
    [ApiController]
    [Authorize]
    public class IssueController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public IssueController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllIssues([FromQuery] string repositoryId, [FromQuery] string? search, [FromQuery] string? filterByStatus)
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
                var issues = await _context.Issues
                    .Where(i => i.RepositoryId == repositoryIdGuid)
                    .Where(i => search == null || i.Title.Contains(search))
                    .Where(i => filterByStatus == null || i.Status.ToString() == filterByStatus)
                    .OrderBy(i => i.CreatedAt)
                    .Include(i => i.Creator)
                    .ToListAsync();

                return Ok(issues);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> GetAllIssues([FromQuery] string repositoryId, [FromBody] IssueDTO issueDTO)
        {
            var repositoryIdGuid = Guid.Parse(repositoryId);

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByNameAsync(User.GetUsername());

            if(user == null)
            {
                return NotFound();
            }

            var repository = await _context.Repositories.Where(r => r.IsDeleted==false).FirstOrDefaultAsync(r=>r.Id == repositoryIdGuid);

            if(repository == null)
            {
                return NotFound();
            }


            if(repository.IsPublic==false && (!repository.Contributors.Any(c => c.UserId == user.Id))){
                return Unauthorized();
            }

            try
            {
                var issue = new Issue
                {
                    Title = issueDTO.Title,
                    Description = issueDTO.Description,
                    Tags = issueDTO.Tags,
                    Status = issueDTO.Status,
                    RepositoryId = repositoryIdGuid,
                    CreatorId = user.Id
                };

                await _context.Issues.AddAsync(issue);
                await _context.SaveChangesAsync();
                return Ok(issue);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("update/{updateIssueID}")]
        public async Task<IActionResult> UpdateIssue([FromRoute] string updateIssueID, [FromBody] IssueDTO issueDTO) {
            var updateIssueId = Guid.Parse(updateIssueID);

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var issue = await _context.Issues.Where(i => i.IsDeleted == false).FirstOrDefaultAsync(i => i.Id == updateIssueId);

            if(issue == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByNameAsync(User.GetUsername());

            if(issue.Creator.Id != user.Id || (!issue.Repository.Contributors.Any(c => c.UserId == user.Id)))
            {
                return Unauthorized();
            }

            issue.Title = issueDTO.Title;
            issue.Description = issueDTO.Description;
            issue.Tags = issueDTO.Tags;
            issue.Status = issueDTO.Status;

            await _context.SaveChangesAsync();
            return Ok(issue);
        }

        [HttpPost("delete/{deleteIssueID}")]
        public async Task<IActionResult> DeleteIssue([FromRoute] string deleteIssueID)
        {
            var deleteIssueId = Guid.Parse(deleteIssueID);

            var issue = await _context.Issues.Where(i => i.IsDeleted == false).FirstOrDefaultAsync(i => i.Id == deleteIssueId);

            if(issue == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByNameAsync(User.GetUsername());

            if(issue.Creator.Id != user.Id)
            {
                return Unauthorized();
            }

            issue.IsDeleted = true;

            await _context.SaveChangesAsync();
            return Ok(issue);
        }
    }
}
