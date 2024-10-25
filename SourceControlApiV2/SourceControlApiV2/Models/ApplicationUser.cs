using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace SourceControlApiV2.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid();
        }

        public virtual ICollection<RepositoryContributor> Repositories { get; set; } = new List<RepositoryContributor>();

        [Comment("Flag to indicate if the user is deleted")]
        public bool isDeleted { get; set; } = false;
    }
}
