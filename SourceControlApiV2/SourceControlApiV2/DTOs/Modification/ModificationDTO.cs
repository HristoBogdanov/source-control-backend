using Microsoft.EntityFrameworkCore;
using static SourceControlAPI.Constants.DataTypes;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SourceControlAPI.Constants;

namespace SourceControlApiV2.DTOs.Modification
{
    public class ModificationDTO
    {
        [Required(ErrorMessage =ErrorMessages.requiredField)]
        [StringLength(50, MinimumLength = 5, ErrorMessage = ErrorMessages.lengthField)]
        public string FileName { get; set; } = null!;

        [Required(ErrorMessage = ErrorMessages.requiredField)]

        [StringLength(1500, MinimumLength = 1, ErrorMessage = ErrorMessages.lengthField)]
        public string FileDifferences { get; set; } = null!;

        [Required(ErrorMessage = ErrorMessages.requiredField)]

        [EnumDataType(typeof(ModificationType))]
        public ModificationType modificationType { get; set; }

        [Required(ErrorMessage = ErrorMessages.requiredField)]
        public Guid CommitId { get; set; }
    }
}
