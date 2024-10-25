using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SourceControlApiV2.Models
{
    [Table("Issues")]
    public class Issue
    {
        [Key]
        [Comment("Unique identifier for the issue")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(50)]
        [Comment("Title of the issue")]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(300)]
        [Comment("Description of the issue")]
        public string Description { get; set; } = null!;

        [Required]
        [MaxLength(150)]
        [Comment("Comma separated list of the tags of the issue")]
        public string Tags { get; set; } = null!;

        [Required]
        [Comment("Status of the issue")]
        public string Status { get; set; } = "Open"; // "Open", "Closed", "On Hold"

        [Required]
        [Comment("Repository that the issue belongs to")]
        public Guid RepositoryId { get; set; }

        [ForeignKey(nameof(RepositoryId))]
        public virtual Repository Repository { get; set; } = null!;

        [Required]
        [Comment("Creator of the issue")]
        public Guid CreatorId { get; set; }

        [ForeignKey(nameof(CreatorId))]
        public virtual ApplicationUser Creator { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        [Comment("Flag to indicate if the issue is deleted")]
        public bool IsDeleted { get; set; } = false;
    }
}