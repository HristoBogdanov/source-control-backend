using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SourceControlApiV2.Models;

namespace SourceControlApiV2.Data.Configurations
{
    public class IssueConfiguration : IEntityTypeConfiguration<Issue>
    {
        public void Configure(EntityTypeBuilder<Issue> builder)
        {
            builder.HasOne(i => i.Repository)
                .WithMany(r => r.Issues)
                .HasForeignKey(i => i.RepositoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
