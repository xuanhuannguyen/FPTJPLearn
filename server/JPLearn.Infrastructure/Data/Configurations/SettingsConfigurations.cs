using JPLearn.Core.Settings.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JPLearn.Infrastructure.Data.Configurations;

public class AppSettingConfiguration : IEntityTypeConfiguration<AppSetting>
{
    public void Configure(EntityTypeBuilder<AppSetting> builder)
    {
        builder.ToTable("AppSettings");
        builder.HasKey(setting => setting.Id);
        builder.Property(setting => setting.Key).IsRequired().HasMaxLength(120);
        builder.Property(setting => setting.Value).IsRequired().HasMaxLength(500);
        builder.Property(setting => setting.Description).HasMaxLength(500);
        builder.HasIndex(setting => setting.Key).IsUnique();
    }
}
