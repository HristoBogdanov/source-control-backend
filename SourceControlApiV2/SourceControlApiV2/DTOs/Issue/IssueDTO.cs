using System.ComponentModel.DataAnnotations;
using SourceControlAPI.Constants;

namespace SourceControlApiV2.DTOs.Issue
{
    public class IssueDTO
    {
        [Required(ErrorMessage = CommonErrorMessages.RequiredTitle)]
        [StringLength(50, MinimumLength = 3, ErrorMessage = CommonErrorMessages.TitleLength)]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = CommonErrorMessages.RequiredDescription)]
        [StringLength(300, MinimumLength = 10, ErrorMessage = CommonErrorMessages.LengthDescription)]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = IssueErrorMessages.TagsRequired)]
        [StringLength(150, ErrorMessage = IssueErrorMessages.TagsLength)]
        public string Tags { get; set; } = null!;

        [Required(ErrorMessage = IssueErrorMessages.StatusRequired)]
        public string Status { get; set; } = null!;

        [Required(ErrorMessage = RepositoryErrorMessages.RepossitoryIdRequired)]
        public Guid RepositoryId { get; set; }
    }
}
