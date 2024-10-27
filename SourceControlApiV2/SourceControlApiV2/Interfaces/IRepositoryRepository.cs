﻿using SourceControlApiV2.DTOs.Repository;

namespace SourceControlApiV2.Interfaces
{
    public interface IRepositoryRepository
    {
        Task<List<RepositoryDTO>> GetRepositories(Guid UserId, string? search);
        Task<RepositoryDTO> CreateRepository(RepositoryDTO repositoryDTO, Guid UserId);
        Task<RepositoryContributorsDTO> AddContributor(Guid repositoryId, Guid contributorId, Guid UserId);
        Task<RepositoryContributorsDTO> RemoveContributor(Guid repositoryId, Guid contributorId, Guid UserId);
        Task<RepositoryDTO> DeleteRepository(RepositoryDTO repositoryDTO, Guid UserId);
    }
}