using SourceControlAPI.Constants;
using System.ComponentModel.DataAnnotations;

namespace SourceControlApiV2.DTOs.User
{
    public class LoginDTO
    {
        [Required(ErrorMessage = UserErrorMessages.UsernameRequired)]
        [StringLength(150, MinimumLength = 3, ErrorMessage = UserErrorMessages.InvalidUsernameLength)]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = UserErrorMessages.PasswordRequired)]
        [StringLength(150, MinimumLength = 12, ErrorMessage = UserErrorMessages.InvalidPasswordLength)]
        public string Password { get; set; } = null!;
    }
}
