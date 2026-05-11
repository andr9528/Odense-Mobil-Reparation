using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repair.Models.Entity.Model;
using Repair.Persistence.Core;
using Repair.Persistence.Core.Abstraction;

namespace Repair.Persistence.Configuration;

public class CustomerConfiguration : EntityConfiguration<Customer>
{
    public CustomerConfiguration(DatabaseType type) : base(type)
    {
    }

    public override void Configure(EntityTypeBuilder<Customer> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.Phone).IsRequired();
        builder.Property(x => x.Email).IsRequired();

        builder.HasIndex(x => new {x.Name, x.Email,}).IsUnique();

        builder.HasIndex(x => new {x.Name, x.Phone,}).IsUnique();

        builder
            .HasMany(x => (IEnumerable<Order>)x.Orders)
            .WithOne(x => (Customer)x.Customer)
            .HasForeignKey(x => x.CustomerId);
    }
}
