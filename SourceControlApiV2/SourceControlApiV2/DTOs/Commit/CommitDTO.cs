using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using SourceControlAPI.DTOs.Modification;

namespace SourceControlApiV2.DTOs.Commit
{
    public class CommitDTO
    {

        [Required]
        [MaxLength(50)]
        [Comment("Title of the commit")]
        public string Title { get; set; } = null!;

        [Required]
        [Comment("Repository that the commit belongs to")]
        public Guid RepositoryId { get; set; }

        [Required]
        [Comment("Author of the commit")]
        public string AuthorId { get; set; } = null!;

        public ICollection<ModificationDTO> Modifications { get; set; } = new List<ModificationDTO>();
    }
}
