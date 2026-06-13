using Newtonsoft.Json;
using Repair.Abstractions.Entity.Model;

namespace Repair.Models.Entity.Model;

public class Order : IOrder
{
    private int id;

    /// <inheritdoc />
    public int Id
    {
        get => id;
        set => throw new InvalidOperationException(
            $"{nameof(Id)} cannot be changed after creation of {nameof(Order)} entity");
    }

    /// <inheritdoc />
    public byte[] Version { get; set; } = [];

    /// <inheritdoc />
    public DateTime CreatedDateTime { get; set; }

    /// <inheritdoc />
    public DateTime UpdatedDateTime { get; set; }

    /// <inheritdoc />
    public string HandInWhat { get; set; } = string.Empty;

    /// <inheritdoc />
    public string RepairWhat { get; set; } = string.Empty;

    /// <inheritdoc />
    public int CustomerId { get; set; }

    /// <inheritdoc />
    public string? BorrowedPhone { get; set; }

    /// <inheritdoc />
    public DateTime HandInWhen { get; set; }

    /// <inheritdoc />
    public DateTime? ReturnedWhen { get; set; }

    /// <inheritdoc />
    public bool IsOrderComplete { get; set; }

    /// <inheritdoc />
    public ICustomer Customer { get; set; } = null!;

    [JsonConstructor]
    private Order(int id, Customer customer)
    {
        this.id = id;
        Customer = customer;
    }

    public Order()
    {
    }
}
