using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SourceControlAPI.DTOs.Commit;
using SourceControlAPI.Constants;

namespace SourceControlApiV2.DTOs.PullRequest
{
    public class PullRequestDTO
    {

        [Required(ErrorMessage =ErrorMessages.requiredField)]
        [MaxLength(50)]
        public string Title { get; set; } = null!;

        [MaxLength(300)]
        [Required(ErrorMessage = ErrorMessages.requiredField)]
        public string Description { get; set; } = String.Empty;

        [Required(ErrorMessage = ErrorMessages.requiredField)]
        public Guid RepositoryId { get; set; }

        [Required(ErrorMessage = ErrorMessages.requiredField)]
        public string CreatorId { get; set; } = null!;

        public ICollection<CommitDTO> Commits { get; set; } = new List<CommitDTO>();
    }
}
