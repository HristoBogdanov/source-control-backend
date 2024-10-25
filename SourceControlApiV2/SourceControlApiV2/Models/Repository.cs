using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SourceControlApiV2.Models
{
    [Table("Repositories")]
    public class Repository
    {
        [Key]
        [Comment("Unique identifier for the repository")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(50)]
        [Comment("Name of the repository")]
        public string Name { get; set; } = null!;

        [MaxLength(300)]
        [Comment("Description of the repository")]
        public string Description { get; set; } = String.Empty;

        [Required]
        [Comment("Flag to indicate if the repository is public or private")]
        public bool IsPublic { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        [Comment("Owner of the repository")]
        public Guid OwnerId { get; set; }

        [ForeignKey(nameof(OwnerId))]
        public virtual ApplicationUser Owner { get; set; } = null!;

        public virtual ICollection<RepositoryContributor> Contributors { get; set; } = new List<RepositoryContributor>();
        public virtual ICollection<Issue> Issues { get; set; } = new List<Issue>();
        public virtual ICollection<Commit> Commits { get; set; } = new List<Commit>();
        public virtual ICollection<PullRequest> PullRequests { get; set; } = new List<PullRequest>();

        [Comment("Flag to indicate if the repository is deleted")]
        public bool IsDeleted { get; set; } = false;
    }
}