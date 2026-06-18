using System.Reflection;
using Repair.Models.Entity.Model;

namespace Repair.Tests.Shared.Factory;

internal static class EntityFactory
{
    public static Customer CreateCustomer(string name, string phone, string email)
    {
        return new Customer
        {
            Name = name,
            Phone = phone,
            Email = email,
        };
    }

    public static Order CreateOrder(string handInWhat, string repairWhat, Customer customer)
    {
        return new Order
        {
            HandInWhat = handInWhat,
            RepairWhat = repairWhat,
            Customer = customer,
        };
    }

    internal static TEntity WithId<TEntity>(this TEntity entity, int id)
    {
        FieldInfo field = typeof(TEntity).GetField("id", BindingFlags.Instance | BindingFlags.NonPublic) ??
                          throw new InvalidOperationException(
                              $"Unable to find private field 'id' on {typeof(TEntity).Name}");

        field.SetValue(entity, id);

        return entity;
    }

    public static Order CreateDefaultOrder()
    {
        Customer customer = CreateCustomer("André", "12345678", "andre@example.com").WithId(12);
        Order order = CreateOrder("iPhone 13", "Screen replacement", customer).WithId(42);

        order.CustomerId = customer.Id;
        order.HandInWhen = new DateTime(2026, 06, 18, 10, 30, 00);
        order.ReturnedWhen = new DateTime(2026, 06, 19, 11, 45, 00);
        order.IsOrderComplete = true;
        order.BorrowedPhone = "Samsung A52";

        return order;
    }
}
