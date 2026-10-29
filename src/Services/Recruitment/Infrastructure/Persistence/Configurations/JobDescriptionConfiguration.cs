using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartRecruitment.Domain.Entities;

namespace SmartRecruitment.Infrastructure.Persistence.Configurations
{
    public class JobDescriptionConfiguration : IEntityTypeConfiguration<JobDescription>
    {
        public void Configure(EntityTypeBuilder<JobDescription> builder)
        {
            builder.ToTable("JobDescriptions");

            builder.HasKey(j => j.Id);

            // CompanyId is a plain foreign key value, not a navigation property
            // the Domain layer intentionally does not reference the Company entity directly
            builder.Property(j => j.CompanyId)
                .IsRequired();

            builder.Property(j => j.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(j => j.Description)
                .IsRequired();

            builder.Property(j => j.Requirements)
                .IsRequired();

            builder.Property(j => j.Status)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(j => j.RejectionReason)
                .HasMaxLength(500);

            // Npgsql maps float[] directly to PostgreSQL's real[] array type no extra configuration needed
            builder.Property(j => j.Embedding);

            builder.Property(j => j.PublishedAt);

            builder.Property(j => j.CreatedAt)
                .IsRequired();
        }
    }
}