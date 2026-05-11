using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repair.Models.Entity.Model;
using Repair.Persistence.Core;
using Repair.Persistence.Core.Abstraction;

namespace Repair.Persistence.Configuration;

public class OrderConfiguration : EntityConfiguration<Order>
{
    public OrderConfiguration(DatabaseType type) : base(type)
    {
    }

    public override void Configure(EntityTypeBuilder<Order> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.HandInWhat).IsRequired();
        builder.Property(x => x.RepairWhat).IsRequired();
        builder.Property(x => x.CustomerId).IsRequired();
        builder.Property(x => x.HandInWhen).IsRequired();
        builder.Property(x => x.ReturnedWhen);
        builder.Property(x => x.IsOrderComplete).IsRequired();
        builder.Property(x => x.HasBorrowedPhone).IsRequired();
    }
}
