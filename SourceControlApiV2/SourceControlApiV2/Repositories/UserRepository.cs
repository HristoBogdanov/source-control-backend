using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SourceControlAPI.Constants;
using SourceControlApiV2.DTOs.User;
using SourceControlApiV2.Interfaces;
using SourceControlApiV2.Models;

namespace SourceControlApiV2.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly ITokenService _tokenService;
        public UserRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signinManager = signInManager;
            _tokenService = tokenService;
        }

        public async Task<NewUserDTO> Login(LoginDTO loginDto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

            if (user == null) throw new UnauthorizedAccessException(UserErrorMessages.InvalidUsernameOrPassword);

            var result = await _signinManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) throw new UnauthorizedAccessException(UserErrorMessages.InvalidUsernameOrPassword);

            return new NewUserDTO
            {
                UserName = user.UserName!,
                Email = user.Email!,
                Token = _tokenService.CreateToken(user)
            };
        }

        public async Task<NewUserDTO> Register(RegisterDTO registerDto)
        {
            var appUser = new ApplicationUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email
            };

            var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);

            if (createdUser.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                if (roleResult.Succeeded)
                {
                    return new NewUserDTO
                    {
                        UserName = appUser.UserName,
                        Email = appUser.Email,
                        Token = _tokenService.CreateToken(appUser)
                    };
                }
                else
                {
                    throw new Exception(string.Join("\n", roleResult.Errors.Select(e => e.Description)));
                }
            }
            else
            {
                throw new Exception(string.Join("\n", createdUser.Errors.Select(e => e.Description)));
            }
        }
    }
}
