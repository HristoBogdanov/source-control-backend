using SourceControlAPI.Constants;
using System.ComponentModel.DataAnnotations;

namespace SourceControlApiV2.DTOs.Modification
{
    public class ModificationDTO
    {
        [Required(ErrorMessage = CommonErrorMessages.RequiredName)]
        [StringLength(50, MinimumLength = 5, ErrorMessage = CommonErrorMessages.NameLength)]
        public string FileName { get; set; } = null!;

        [Required(ErrorMessage = ModificationErrorMessages.DifferencesRequired)]
        [StringLength(1500, MinimumLength = 1, ErrorMessage = ModificationErrorMessages.DifferencesLength)]
        public string FileDifferences { get; set; } = null!;

        [Required(ErrorMessage = ModificationErrorMessages.TypeModificationRequired)]
        public string modificationType { get; set; } = null!;

        [Required(ErrorMessage = CommitErrorMessages.CommitIdRequired)]
        public Guid CommitId { get; set; }
    }
}
