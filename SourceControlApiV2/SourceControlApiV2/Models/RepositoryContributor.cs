using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SourceControlApiV2.Models
{
    [Table("RepositoryContributors")]
    public class RepositoryContributor
    {
        [Required]
        [Comment("Unique identifier for the repository contributor")]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; } = null!;

        [Required]
        [Comment("Unique identifier for the repository")]
        public Guid RepositoryId { get; set; }

        [ForeignKey(nameof(RepositoryId))]
        public virtual Repository Repository { get; set; } = null!;
    }
}