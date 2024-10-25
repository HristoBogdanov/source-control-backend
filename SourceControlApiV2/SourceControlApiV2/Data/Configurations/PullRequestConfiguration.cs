using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SourceControlApiV2.Models;

namespace SourceControlApiV2.Data.Configurations
{
    public class PullRequestConfiguration : IEntityTypeConfiguration<PullRequest>
    {
        public void Configure(EntityTypeBuilder<PullRequest> builder)
        {
            builder.HasOne(pr => pr.SourceRepository)
                .WithMany(r => r.PullRequests)
                .HasForeignKey(i => i.RepositoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
