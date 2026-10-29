using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartRecruitment.Domain.Entities;

namespace SmartRecruitment.Infrastructure.Persistence.Configurations
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("Companies");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(256);

            builder.HasIndex(c => c.Email)
                .IsUnique();

            builder.Property(c => c.BusinessRegistrationNumber)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(c => c.BusinessRegistrationNumber)
                .IsUnique();

            builder.Property(c => c.VerificationStatus)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(c => c.RejectionReason)
                .HasMaxLength(500);

            builder.Property(c => c.CreatedAt)
                .IsRequired();
        }
    }
}