using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartRecruitment.Domain.Entities;

namespace SmartRecruitment.Infrastructure.Persistence.Configurations
{
    public class CVConfiguration : IEntityTypeConfiguration<CV>
    {
        public void Configure(EntityTypeBuilder<CV> builder)
        {
            builder.ToTable("CVs");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.CandidateId)
                .IsRequired();

            builder.Property(c => c.Content)
                .IsRequired();

            builder.Property(c => c.Status)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(c => c.Source)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            // Npgsql maps float[] directly to PostgreSQL's real[] array type
            builder.Property(c => c.Embedding);

            builder.Property(c => c.FinalizedAt);

            builder.Property(c => c.CreatedAt)
                .IsRequired();

            // Speeds up GetAllFinalizedAsync, which filters by Status
            builder.HasIndex(c => c.Status);
        }
    }
}