using System.ComponentModel.DataAnnotations;
using SourceControlAPI.Constants;

namespace SourceControlApiV2.DTOs.Repository
{
    public class RepositoryDTO
    {
        [Required(ErrorMessage = ErrorMessages.requiredField)]
        [StringLength(50,MinimumLength =3, ErrorMessage = ErrorMessages.lengthField)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = ErrorMessages.requiredField)]
        [StringLength(300, MinimumLength = 20, ErrorMessage = ErrorMessages.lengthField)]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = ErrorMessages.requiredField)]
        public bool IsPublic { get; set; }
    }
}
