using Bogus;
using Repair.Abstractions.Entity.Model;
using Repair.Models.Entity.Model;

namespace Repair.Services;

public class BogusService
{
    private readonly string[] handInItems =
    [
        "iPhone 13",
        "iPhone 14",
        "Samsung S23",
        "Samsung S24",
        "iPad Air",
        "MacBook Air",
        "OnePlus 12",
        "Google Pixel 9",
    ];

    private readonly string[] shortRepairTypes =
    [
        "Screen replacement",
        "Battery replacement",
        "Charging port repair",
        "Water damage",
        "Speaker repair",
        "Microphone repair",
        "Camera replacement",
        "Software issue",
        "Power button repair",
        "Diagnostic check",
    ];

    private readonly string[] longRepairTypes =
    [
        "Customer reports intermittent charging issues when cable is moved slightly",
        "Screen replacement required due to multiple cracks across the entire display",
        "Battery drains from one hundred percent to empty within a few hours of normal use",
        "Device shuts down unexpectedly when battery level reaches approximately thirty percent",
        "Customer reports that the phone becomes extremely hot during charging and gaming",
        "Front and rear camera modules require replacement due to moisture damage",
        "Phone was dropped into salt water and requires a complete internal cleaning and inspection",
        "Touch screen responds inconsistently and occasionally registers presses in incorrect locations",
        "Charging port is physically damaged and no longer maintains a stable connection to the cable",
        "Customer requests replacement of battery, charging port and cracked rear glass during same repair",
        "Customer reports that the device powers on normally but becomes unresponsive after several minutes of use, requiring a forced restart before it can be used again",
        "Phone was handed in after being run over by a vehicle and requires a complete diagnostic assessment to determine whether repair is economically viable",
        "Customer states that notifications, calls and text messages are delayed by several hours despite the device showing a stable mobile data connection",
        "Device suffered extensive liquid damage after being left outside during heavy rain and now exhibits charging issues, display artefacts and intermittent audio problems",
        "Customer requests a full inspection because the device has experienced random shutdowns, poor battery life, overheating and occasional touch screen freezes over the last several months",
    ];

    public IReadOnlyList<Customer> CreateCustomers(
        int customerCount, int minimumOrdersPerCustomer = 0, int maximumOrdersPerCustomer = 5)
    {
        Faker faker = new();

        List<Customer> customers = CreateCustomerFaker().Generate(customerCount);

        foreach (Customer customer in customers)
        {
            int orderCount = faker.Random.Int(minimumOrdersPerCustomer, maximumOrdersPerCustomer);
            customer.Orders = CreateOrders(orderCount, customer).ToList<IOrder>();
        }

        return customers;
    }

    private Faker<Customer> CreateCustomerFaker()
    {
        return new Faker<Customer>().RuleFor(x => x.Name, x => x.Name.FullName())
            .RuleFor(x => x.Phone, x => x.Phone.PhoneNumber("########")).RuleFor(x => x.Email,
                (x, customer) => x.Internet.Email(customer.Name.Replace(' ', '.').ToLowerInvariant()));
    }

    private IReadOnlyList<Order> CreateOrders(int orderCount, Customer customer)
    {
        return CreateOrderFaker(customer).Generate(orderCount);
    }

    private Faker<Order> CreateOrderFaker(Customer customer)
    {
        return new Faker<Order>().RuleFor(x => x.HandInWhat, x => x.PickRandom(handInItems))
            .RuleFor(x => x.RepairWhat,
                x => x.Random.Bool(0.3f) ? x.PickRandom(longRepairTypes) : x.PickRandom(shortRepairTypes))
            .RuleFor(x => x.HandInWhen, x => x.Date.Between(DateTime.Today.AddMonths(-6), DateTime.Today))
            .RuleFor(x => x.IsOrderComplete, x => x.Random.Bool(0.60f))
            .RuleFor(x => x.BorrowedPhone, x => x.Random.Bool(0.15f) ? x.PickRandom(handInItems) : null)
            .RuleFor(x => x.ReturnedWhen, GetReturnedWhen).RuleFor(x => x.Customer, customer);
    }

    private DateTime? GetReturnedWhen(Faker faker, Order order)
    {
        if (!order.IsOrderComplete)
        {
            return null;
        }

        return faker.Date.Between(order.HandInWhen, order.HandInWhen.AddDays(30));
    }
}
