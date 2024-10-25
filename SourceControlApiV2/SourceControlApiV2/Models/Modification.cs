using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SourceControlApiV2.Models
{
    [Table("Modifications")]
    public class Modification
    {
        [Key]
        [Comment("Unique identifier for the modification")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(50)]
        [Comment("Name of the file")]
        public string FileName { get; set; } = null!;

        [Required]
        [MaxLength(1500)]
        [Comment("Differences in the file")]
        public string FileDifferences { get; set; } = null!;

        [Required]
        [Comment("Type of modification")]
        public string modificationType { get; set; } = "Modified"; // "Added", "Deleted", "Modified

        [Required]
        [Comment("Commit that the modification belongs to")]
        public Guid CommitId { get; set; }

        [ForeignKey(nameof(CommitId))]
        public virtual Commit Commit { get; set; } = null!;

        [Comment("Flag to indicate if the modification is deleted")]
        public bool IsDeleted { get; set; } = false;
    }
}