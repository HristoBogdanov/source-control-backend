using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SourceControlApiV2.Models
{
    [Table("Commits")]
    public class Commit
    {
        [Key]
        [Comment("Unique identifier for the commit")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(50)]
        [Comment("Title of the commit")]
        public string Title { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [Comment("Repository that the commit belongs to")]
        public Guid RepositoryId { get; set; }

        [ForeignKey(nameof(RepositoryId))]
        public virtual Repository Repository { get; set; } = null!;

        [Required]
        [Comment("Author of the commit")]
        public Guid AuthorId { get; set; }

        [ForeignKey(nameof(AuthorId))]
        public virtual ApplicationUser Author { get; set; } = null!;

        [Comment("Flag to indicate if the commit is deleted")]
        public bool IsDeleted { get; set; } = false;

        public virtual ICollection<Modification> Modifications { get; set; } = new List<Modification>();

        [Comment("Pull request that the commit belongs to")]
        public Guid PullRequestId { get; set; }

        [ForeignKey(nameof(PullRequestId))]
        public virtual PullRequest PullRequest { get; set; } = null!;
    }
}