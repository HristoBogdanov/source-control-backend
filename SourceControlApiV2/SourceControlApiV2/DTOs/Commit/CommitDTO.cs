using SourceControlAPI.Constants;
using SourceControlApiV2.DTOs.Modification;
using System.ComponentModel.DataAnnotations;

namespace SourceControlApiV2.DTOs.Commit
{
    public class CommitDTO
    {
        [Required(ErrorMessage = CommonErrorMessages.RequiredTitle)]
        [StringLength(50, MinimumLength = 3, ErrorMessage = CommonErrorMessages.TitleLength)]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = RepositoryErrorMessages.RepossitoryIdRequired)]
        public Guid RepositoryId { get; set; }

        [Required(ErrorMessage = UserErrorMessages.UserIdRequired)]
        public Guid AuthorId { get; set; }

        public ICollection<ModificationDTO> Modifications { get; set; } = new List<ModificationDTO>();
    }
}
