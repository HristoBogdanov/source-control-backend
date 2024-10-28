using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SourceControlAPI.Constants;
using SourceControlApiV2.Data;
using SourceControlApiV2.DTOs.Repository;
using SourceControlApiV2.Interfaces;
using SourceControlApiV2.Models;

namespace SourceControlApiV2.Repositories
{
    public class RepositoryRepository : IRepositoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RepositoryRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<RepositoryInfoDTO>> GetRepositories(Guid UserId, string? search)
        {
            try
            {
                var repositories = await _context.Repositories
                    .Include(r => r.Owner)
                    .Where(r => r.IsDeleted == false)
                    .Where(r => r.IsPublic == true
                    || r.OwnerId == UserId
                    || r.Contributors.Any(c => c.UserId == UserId))
                    .Where(r => search == null || r.Name.Contains(search))
                    .OrderBy(r => r.CreatedAt)
                    .Select(r => new RepositoryInfoDTO()
                    {
                        Name = r.Name,
                        Description = r.Description,
                        IsPublic = r.IsPublic,
                        OwnerName = r.Owner.UserName
                    })
                    .AsNoTracking()
                    .ToListAsync();

                return repositories;
            }
            catch (Exception ex)
            {
                throw new Exception(RepositoryErrorMessages.UserGettingRepositories + ex.Message);
            }

        }

        public async Task<RepositoryDTO> CreateRepository(RepositoryDTO repositoryDTO, Guid UserId)
        {
            try
            {
                var repository = new Repository
                {
                    Name = repositoryDTO.Name,
                    Description = repositoryDTO.Description,
                    IsPublic = repositoryDTO.IsPublic,
                    OwnerId = UserId
                };

                await _context.Repositories.AddAsync(repository);
                await _context.SaveChangesAsync();

                return repositoryDTO;
            }
            catch (Exception ex)
            {
                throw new Exception(RepositoryErrorMessages.ErrorCreatingRepository + ex.Message);
            }
        }

        public async Task<RepositoryContributorsDTO> AddContributor(Guid repositoryId, Guid contributorId, Guid UserId)
        {

            var contributor = await _context.Users.FirstOrDefaultAsync(u => u.Id == contributorId);
            var repository = await _context.Repositories.Where(r => r.IsDeleted == false).FirstOrDefaultAsync(r => r.Id == repositoryId);

            if (contributor == null)
            {
                throw new Exception(UserErrorMessages.InvalidUserId);
            }

            if (repository == null)
            {
                throw new Exception(RepositoryErrorMessages.InvalidRepositoryId);
            }

            try
            {
                if(repository.OwnerId != UserId)
                {
                    throw new Exception(UserErrorMessages.UserAddingContributors);
                }

                var contribution = new RepositoryContributor
                {
                    UserId = contributorId,
                    RepositoryId = repositoryId
                };

                await _context.RepositoryContributors.AddAsync(contribution);
                await _context.SaveChangesAsync();

                return new RepositoryContributorsDTO
                {
                    Name = repository.Name,
                    Description = repository.Description,
                    IsPublic = repository.IsPublic,
                    Contributors = repository.Contributors.Select(c => c.UserId.ToString()).ToList()
                };
            }
            catch (Exception ex)
            {
                throw new Exception(RepositoryErrorMessages.ErrorCreatingRepository + ex.Message);
            }
        }

        public async Task<RepositoryContributorsDTO> RemoveContributor(Guid repositoryId, Guid contributorId, Guid UserId)
        {

            var contributor = await _context.Users.FirstOrDefaultAsync(u => u.Id == contributorId);
            var repository = await _context.Repositories.Where(r => r.IsDeleted == false).FirstOrDefaultAsync(r => r.Id == repositoryId);

            if (contributor == null)
            {
                throw new Exception(UserErrorMessages.InvalidUserId);
            }

            if (repository == null)
            {
                throw new Exception(RepositoryErrorMessages.InvalidRepositoryId);
            }

            try
            {
                var contribution = _context.RepositoryContributors.FirstOrDefault(c => c.UserId == contributorId && c.RepositoryId == repositoryId);

                if (contribution == null)
                {
                    throw new Exception(UserErrorMessages.UserIsNotContributor);
                }

                _context.RepositoryContributors.Remove(contribution);
                await _context.SaveChangesAsync();

                return new RepositoryContributorsDTO()
                {
                    Name = repository.Name,
                    Description = repository.Description,
                    IsPublic = repository.IsPublic,
                    Contributors = repository.Contributors.Select(c => c.UserId.ToString()).ToList()
                };
            }
            catch (Exception ex)
            {
                throw new Exception(RepositoryErrorMessages.ErrorRemovingContributor + ex.Message);
            }
        }

        public async Task<RepositoryDTO> DeleteRepository(Guid deleteRepositoryId, Guid UserId)
        {
            var repository = await _context.Repositories.Where(r => r.IsDeleted == false).FirstOrDefaultAsync(r => r.Id == deleteRepositoryId);

            if (repository == null)
            {
                throw new Exception(RepositoryErrorMessages.InvalidRepositoryId);
            }

            if (repository.Owner.Id != UserId)
            {
                throw new Exception(UserErrorMessages.UserDeletingRepository);
            }

            repository.IsDeleted = true;

            await _context.SaveChangesAsync();
            return new RepositoryDTO
            {
                Name = repository.Name,
                Description = repository.Description,
                IsPublic = repository.IsPublic
            };
        }
    }
}
