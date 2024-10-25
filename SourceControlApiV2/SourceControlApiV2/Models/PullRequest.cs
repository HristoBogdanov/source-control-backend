using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SourceControlApiV2.Models
{
    [Table("PullRequests")]
    public class PullRequest
    {
        [Key]
        [Comment("Unique identifier for the pull request")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(50)]
        [Comment("Title of the pull request")]
        public string Title { get; set; } = null!;

        [MaxLength(300)]
        public string Description { get; set; } = String.Empty;

        [Comment("Flag to indicate if the pull request is resolved")]
        public bool IsResolved { get; set; } = false;

        public DateTime CreatedAt = DateTime.UtcNow;

        [Required]
        [Comment("Repository that the pull request belongs to")]
        public Guid RepositoryId { get; set; }

        [ForeignKey(nameof(RepositoryId))]
        public virtual Repository SourceRepository { get; set; } = null!;

        [Required]
        [Comment("The creator of the pull request")]
        public Guid CreatorId { get; set; }

        [ForeignKey(nameof(CreatorId))]
        public virtual ApplicationUser Creator { get; set; } = null!;

        public virtual ICollection<Commit> Commits { get; set; } = new List<Commit>();

        [Comment("Flag to indicate if the pull request is deleted")]
        public bool IsDeleted { get; set; } = false;
    }
}
