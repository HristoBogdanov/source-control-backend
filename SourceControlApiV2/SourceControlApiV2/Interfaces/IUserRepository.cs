using SourceControlApiV2.DTOs.User;

namespace SourceControlApiV2.Interfaces
{
    public interface IUserRepository
    {
        Task<NewUserDTO> Login(LoginDTO loginDto);
        Task<NewUserDTO> Register(RegisterDTO registerDto);
    }
}
