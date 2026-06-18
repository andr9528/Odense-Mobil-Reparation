using Moq;
using Repair.Abstractions.Persistence;
using Repair.Abstractions.Services;
using Repair.Frontend.Abstraction;
using Repair.Models.Entity.Model;
using Repair.Models.Entity.Searchable;
using Path = System.IO.Path;

namespace Repair.Tests.Frontend.Core;

internal static class MockFactory
{
    internal static Mock<IEntityQueryService<Order, SearchableOrder>> CreateOrderQueryService(Order order)
    {
        Mock<IEntityQueryService<Order, SearchableOrder>> mock = new();

        mock.Setup(x => x.GetEntity(It.Is<SearchableOrder>(searchable => searchable.Id == order.Id)))
            .ReturnsAsync(order);

        mock.Setup(x => x.GetEntities(It.IsAny<SearchableOrder>())).ReturnsAsync([order,]);

        mock.Setup(x => x.UpdateEntity(It.IsAny<Order>())).Returns(Task.CompletedTask);

        mock.Setup(x => x.AddEntity(It.IsAny<Order>())).Returns(Task.CompletedTask);

        mock.Setup(x => x.DeleteEntityById(It.IsAny<int>())).Returns(Task.CompletedTask);

        return mock;
    }

    internal static Mock<INavigationService> CreateNavigationService()
    {
        return new Mock<INavigationService>();
    }

    internal static Mock<IReportService> CreateReportService()
    {
        Mock<IReportService> mock = new();

        mock.Setup(x => x.CreateReport(It.IsAny<Order>())).ReturnsAsync(Path.Combine("Temp", "TestReport.pdf"));

        return mock;
    }

    internal static Mock<IUiDispatcher> CreateUiDispatcher()
    {
        Mock<IUiDispatcher> mock = new();

        mock.Setup(x => x.TryEnqueue(It.IsAny<Action>())).Callback<Action>(action => action()).Returns(true);

        return mock;
    }
}
