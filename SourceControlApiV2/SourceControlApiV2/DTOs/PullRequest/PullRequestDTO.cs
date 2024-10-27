using System.ComponentModel.DataAnnotations;
using SourceControlAPI.Constants;
using SourceControlApiV2.DTOs.Commit;

namespace SourceControlApiV2.DTOs.PullRequest
{
    public class PullRequestDTO
    {

        [Required(ErrorMessage = CommonErrorMessages.RequiredTitle)]
        [StringLength(50, MinimumLength = 3, ErrorMessage = CommonErrorMessages.TitleLength)]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = CommonErrorMessages.RequiredDescription)]
        [StringLength(500, MinimumLength = 3, ErrorMessage = CommonErrorMessages.LengthDescription)]
        public string Description { get; set; } = String.Empty;

        [Required(ErrorMessage = RepositoryErrorMessages.RepossitoryIdRequired)]
        public Guid RepositoryId { get; set; }

        [Required(ErrorMessage = UserErrorMessages.UserIdRequired)]
        public Guid CreatorId { get; set; }

        public ICollection<CommitDTO> Commits { get; set; } = new List<CommitDTO>();
    }
}
