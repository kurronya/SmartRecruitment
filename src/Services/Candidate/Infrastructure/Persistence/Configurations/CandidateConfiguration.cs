using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartRecruitment.Domain.Entities;

namespace SmartRecruitment.Infrastructure.Persistence.Configurations
{
    public class CandidateConfiguration : IEntityTypeConfiguration<Candidate>
    {
        public void Configure(EntityTypeBuilder<Candidate> builder)
        {
            builder.ToTable("Candidates");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.FullName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(256);

            // Enforce email uniqueness at the database level, matching the business rule
            // already checked in CreateCandidateHandler
            builder.HasIndex(c => c.Email)
                .IsUnique();

            builder.Property(c => c.PhoneNumber)
                .HasMaxLength(20);

            // Store the enum as a readable string instead of an integer,
            // so the raw table data stays understandable when inspected in pgAdmin
            builder.Property(c => c.Status)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(c => c.StatusChangedAt)
                .IsRequired();

            builder.Property(c => c.CreatedAt)
                .IsRequired();
        }
    }
}