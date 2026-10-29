using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartRecruitment.Domain.Entities;

namespace SmartRecruitment.Infrastructure.Persistence.Configurations
{
    public class InterviewConfiguration : IEntityTypeConfiguration<Interview>
    {
        public void Configure(EntityTypeBuilder<Interview> builder)
        {
            builder.ToTable("Interviews");

            builder.HasKey(i => i.Id);

            // Association entity - CandidateId and JobDescriptionId are plain foreign key values
            builder.Property(i => i.CandidateId)
                .IsRequired();

            builder.Property(i => i.JobDescriptionId)
                .IsRequired();

            builder.Property(i => i.ScheduledAt)
                .IsRequired();

            builder.Property(i => i.Status)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(i => i.CompletedAt);

            builder.Property(i => i.CancellationReason)
                .HasMaxLength(500);

            builder.Property(i => i.CreatedAt)
                .IsRequired();

            // Speeds up GetActiveInterviewForCandidateAsync, which filters by CandidateId
            builder.HasIndex(i => i.CandidateId);
        }
    }
}