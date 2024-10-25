using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static SourceControlAPI.Constants.DataTypes;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SourceControlAPI.Constants;

namespace SourceControlApiV2.DTOs.Issue
{
    public class IssueDTO
    {
        [Required(ErrorMessage = ErrorMessages.requiredField)]
        [StringLength(50, MinimumLength = 3, ErrorMessage = ErrorMessages.lengthField)]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = ErrorMessages.requiredField)]
        [StringLength(300, MinimumLength = 10, ErrorMessage = ErrorMessages.lengthField)]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = ErrorMessages.requiredField)]
        [StringLength(150, ErrorMessage = ErrorMessages.lengthField)]
        public string Tags { get; set; } = null!;

        [Required(ErrorMessage = ErrorMessages.requiredField)]
        [EnumDataType(typeof(IssueStatusType))]
        public IssueStatusType Status { get; set; }

        [Required(ErrorMessage = ErrorMessages.requiredField)]
        public Guid RepositoryId { get; set; }
    }
}
