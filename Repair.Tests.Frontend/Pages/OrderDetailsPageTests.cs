using FluentAssertions;
using Repair.Models.Entity.Model;
using Repair.Tests.Frontend.Harness;
using Repair.Tests.Shared.Factory;

namespace Repair.Tests.Frontend.Pages;

public class OrderDetailsPageTests
{
    public class RefreshOrder
    {
        [Test]
        public async Task WithExistingOrder_AppliesOrderToEditorAndSelectsCustomer()
        {
            Order order = EntityFactory.CreateDefaultOrder();
            var harness = OrderDetailsPageHarness.Create(order);

            await harness.Logic.RefreshOrder();

            harness.ViewModel.OrderEditor.ViewModel.CustomerId.Should().Be(order.CustomerId);
            harness.ViewModel.OrderEditor.ViewModel.CustomersGrid.ViewModel.SelectedCustomerId.Should()
                .Be(order.CustomerId);
            harness.ViewModel.HasChanges.Should().BeFalse();
        }
    }

    public class OrderEditorEvents
    {
        [Test]
        public async Task WhenHandInWhatChanges_HasChangesBecomesTrue()
        {
            Order order = EntityFactory.CreateDefaultOrder();
            var harness = OrderDetailsPageHarness.Create(order);

            await harness.Logic.RefreshOrder();

            harness.ViewModel.HasChanges.Should().BeFalse();

            harness.ViewModel.OrderEditor.ViewModel.HandInWhat = "Changed repair item";

            harness.ViewModel.HasChanges.Should().BeTrue();
        }
    }
}
