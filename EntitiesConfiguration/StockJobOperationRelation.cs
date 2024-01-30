using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMTS.Entities;

namespace SMTS.EntitiesConfiguration
{
    public class StockJobOperationRelationConfiguration : IEntityTypeConfiguration<StockJobOperationRelation>
    {
        public void Configure(EntityTypeBuilder<StockJobOperationRelation> builder)
        {
            // Specify a composite key using Prefix and Number
            builder.HasKey(rn => new { rn.JobOperationId, rn.StockDtlKey });

            // Other configurations, if needed
            // builder.Property(rn => rn.Prefix).IsRequired();
            // builder.Property(rn => rn.Number).IsRequired();
        }
    }
}
