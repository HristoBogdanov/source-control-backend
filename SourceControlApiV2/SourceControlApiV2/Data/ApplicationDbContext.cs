using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SourceControlApiV2.Models;
using System.Reflection;

namespace SourceControlApiV2.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<Commit> Commits { get; set; } = null!;
        public DbSet<Issue> Issues { get; set; } = null!;
        public DbSet<Modification> Modifications { get; set; } = null!;
        public DbSet<PullRequest> PullRequests { get; set; } = null!;
        public DbSet<Repository> Repositories { get; set; } = null!;
        public DbSet<RepositoryContributor> RepositoryContributors { get; set; } = null!;


        // Enable lazy loading for all virtual properties
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Automatically apply all configurations in the project
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
