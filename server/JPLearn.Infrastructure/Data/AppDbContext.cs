using Microsoft.EntityFrameworkCore;
using JPLearn.Core.Vocabulary.Entities;
using JPLearn.Core.Review.Entities;

namespace JPLearn.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<VocabularyList> VocabularyLists => Set<VocabularyList>();
    public DbSet<VocabularyItem> VocabularyItems => Set<VocabularyItem>();
    public DbSet<UserWordProgress> UserWordProgress => Set<UserWordProgress>();
    public DbSet<ReviewSession> ReviewSessions => Set<ReviewSession>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
