using Newtonsoft.Json;
using Repair.Abstractions.Entity.Model;

namespace Repair.Models.Entity.Model;

public class Customer : ICustomer
{
    private int id;

    /// <inheritdoc />
    public int Id
    {
        get => id;
        set => throw new InvalidOperationException(
            $"{nameof(Id)} cannot be changed after creation of {nameof(Customer)} entity");
    }

    /// <inheritdoc />
    public byte[] Version { get; set; } = [];

    /// <inheritdoc />
    public DateTime CreatedDateTime { get; set; }

    /// <inheritdoc />
    public DateTime UpdatedDateTime { get; set; }

    /// <inheritdoc />
    public string Name { get; set; } = string.Empty;

    /// <inheritdoc />
    public string Phone { get; set; } = string.Empty;

    /// <inheritdoc />
    public string Email { get; set; } = string.Empty;

    /// <inheritdoc />
    public ICollection<IOrder> Orders { get; set; } = new List<IOrder>();

    [JsonConstructor]
    private Customer(int id, List<Order> orders)
    {
        this.id = id;
        Orders = orders.Cast<IOrder>().ToList();
    }

    public Customer()
    {
    }
}
