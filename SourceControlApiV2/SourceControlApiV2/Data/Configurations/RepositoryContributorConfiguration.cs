using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SourceControlApiV2.Models;

namespace SourceControlApiV2.Data.Configurations
{
    public class RepositoryContributorConfiguration : IEntityTypeConfiguration<RepositoryContributor>
    {
        public void Configure(EntityTypeBuilder<RepositoryContributor> builder)
        {
            builder.HasKey(rc => new { rc.UserId, rc.RepositoryId });

            builder.HasOne(rc => rc.User)
                .WithMany(u => u.Repositories)
                .HasForeignKey(rc => rc.UserId);

            builder.HasOne(rc => rc.Repository)
                .WithMany(r => r.Contributors)
                .HasForeignKey(rc => rc.RepositoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
