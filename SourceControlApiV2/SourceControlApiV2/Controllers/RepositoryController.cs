using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SourceControlAPI.Constants;
using SourceControlApiV2.Data;
using SourceControlApiV2.DTOs.Repository;
using SourceControlApiV2.Extensions;
using SourceControlApiV2.Interfaces;
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
        private readonly IRepositoryRepository _repositoryRepository;

        public RepositoryController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IRepositoryRepository repository)
        {
            _context = context;
            _userManager = userManager;
            _repositoryRepository = repository;
        }

        [HttpGet("all")]
       public async Task<IActionResult> GetAllRepositories([FromQuery] string? search)
       {
            try
            {
                var user = await _userManager.FindByNameAsync(User.GetUsername());
                var repositories = await _repositoryRepository.GetRepositories(user.Id, search);

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
                RepositoryDTO repository = await _repositoryRepository.CreateRepository(repositoryDTO, user.Id);

                return Ok(repository);
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
            var user = await _userManager.FindByNameAsync(User.GetUsername());

            try 
            {
                var repoContributors = await _repositoryRepository.AddContributor(repositoryIdGuid, contributroIdGuid, user.Id);

                return Ok(repoContributors);
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
