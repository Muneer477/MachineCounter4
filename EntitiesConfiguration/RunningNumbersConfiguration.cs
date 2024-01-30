using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SMTS.Entities;

namespace SMTS.EntitiesConfiguration
{
    public class RunningNumbersConfiguration : IEntityTypeConfiguration<RunningNumbers>
    {
        public void Configure(EntityTypeBuilder<RunningNumbers> builder)
        {
            // Specify a composite key using Prefix and Number
            builder.HasKey(rn => new { rn.Prefix, rn.Number });

            // Other configurations, if needed
            // builder.Property(rn => rn.Prefix).IsRequired();
            // builder.Property(rn => rn.Number).IsRequired();
        }
    }

}
