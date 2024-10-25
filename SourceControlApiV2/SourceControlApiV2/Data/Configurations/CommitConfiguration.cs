using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SourceControlApiV2.Models;

namespace SourceControlApiV2.Data.Configurations
{
    public class CommitConfiguration : IEntityTypeConfiguration<Commit>
    {
        public void Configure(EntityTypeBuilder<Commit> builder)
        {
            builder.HasKey(c => c.Id);

            builder.HasOne(c => c.PullRequest)
                .WithMany(pr => pr.Commits)
                .HasForeignKey(c => c.PullRequestId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Repository)
                .WithMany(r => r.Commits)
                .HasForeignKey(c => c.RepositoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
