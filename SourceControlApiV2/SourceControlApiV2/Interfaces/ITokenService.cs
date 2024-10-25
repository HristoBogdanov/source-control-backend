using SourceControlApiV2.Models;

namespace SourceControlApiV2.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(ApplicationUser user);
    }
}
