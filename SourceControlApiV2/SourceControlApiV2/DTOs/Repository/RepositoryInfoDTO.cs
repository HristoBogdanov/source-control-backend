namespace SourceControlApiV2.DTOs.Repository
{
    public class RepositoryInfoDTO
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsPublic { get; set; }
        public string OwnerName { get; set; } = null!;
    }
}
