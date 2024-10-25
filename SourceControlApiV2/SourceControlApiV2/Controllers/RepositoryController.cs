using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SourceControlApiV2.Data;
using SourceControlApiV2.Extensions;
using SourceControlApiV2.Models;

namespace SourceControlApiV2.Controllers
{
    [Route("api/repository")]
    [ApiController]
    [Authorize]
    public class RepositoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RepositoryController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("all")]
       public async Task<IActionResult> GetAllRepositories([FromQuery] string? search)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.GetUsername());

                var repositories = await _context.Repositories
                    .Where(r => r.IsDeleted == false)
                    .Where(r => r.IsPublic == true 
                    || r.OwnerId == user.Id
                    || r.Contributors.Any(c => c.UserId == user.Id))
                    .Where(r => search == null || r.Name.Contains(search))
                    .OrderBy(r => r.CreatedAt)
                    .ToListAsync();
                return Ok(repositories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
       }

        [HttpPost("create")]
        public async Task<IActionResult> CreateRepository([FromBody] RepositoryDTO repositoryDTO)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await _userManager.FindByNameAsync(User.GetUsername());

                var repository = new Repository
                {
                    Name = repositoryDTO.Name,
                    Description = repositoryDTO.Description,
                    IsPublic = repositoryDTO.IsPublic,
                    OwnerId = user.Id
                };

                await _context.Repositories.AddAsync(repository);
                await _context.SaveChangesAsync();

                return Ok(repositoryDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("add-contributor/{repositoryId}")]
        public async Task<IActionResult> AddContributorToRepository([FromBody] string contributorId, string repositoryId)
        {
            var repositoryIdGuid = Guid.Parse(repositoryId);

            var contributroIdGuid = Guid.Parse(contributorId);

            if (contributorId == null)
            {
                return BadRequest(ErrorMessages.ContributerIdRequired);
            }

            var contributor = await _context.Users.FirstOrDefaultAsync(u => u.Id == contributroIdGuid);
            var repository = await _context.Repositories.Where(r => r.IsDeleted == false).FirstOrDefaultAsync(r => r.Id == repositoryIdGuid);

            if(contributor == null || repository == null)
            {
                return BadRequest(ErrorMessages.InvalidData);
            }

            try
            {
                var contribution = new RepositoryContributor
                {
                    UserId = contributroIdGuid,
                    RepositoryId = repositoryIdGuid
                };

                await _context.RepositoryContributors.AddAsync(contribution);
                await _context.SaveChangesAsync();

                return Ok(contribution);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("remove-contributor/{repositoryId}")]
        public async Task<IActionResult> RemoveContributorFromRepository([FromRoute] string repositoryId, [FromBody] string contributorId)
        {
            var repositoryIdGuid = Guid.Parse(repositoryId);

            var contributorIdGuid = Guid.Parse(contributorId);

            if (contributorId == null)
            {
                return BadRequest(ErrorMessages.ContributerIdRequired);
            }

            var contributor = await _context.Users.FirstOrDefaultAsync(u => u.Id == contributorIdGuid);
            var repository = await _context.Repositories.Where(r => r.IsDeleted == false).FirstOrDefaultAsync(r => r.Id == repositoryIdGuid);

            if (contributor == null || repository == null)
            {
                return BadRequest(ErrorMessages.InvalidData);
            }

            try
            {
                var contribution = _context.RepositoryContributors.FirstOrDefault(c => c.UserId == contributorIdGuid && c.RepositoryId == repositoryIdGuid);

                if(contribution == null)
                {
                    return BadRequest(ErrorMessages.InvalidData);
                }

                _context.RepositoryContributors.Remove(contribution);
                await _context.SaveChangesAsync();

                return Ok(contribution);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("delete/{deleteRepositoryId}")]
        public async Task<IActionResult> DeleteRepository([FromRoute] string deleteRepositoryId)
        {
            var deleteRepositoryIdGuid = Guid.Parse(deleteRepositoryId);

            var repository = await _context.Repositories.Where(r => r.IsDeleted == false).FirstOrDefaultAsync(r => r.Id == deleteRepositoryIdGuid);

            if (repository == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByNameAsync(User.GetUsername());

            if (repository.Owner.Id != user.Id)
            {
                return Unauthorized();
            }

            repository.IsDeleted = true;

            await _context.SaveChangesAsync();
            return Ok(repository);
        }

    }
}
