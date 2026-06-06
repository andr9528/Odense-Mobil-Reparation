using Repair.Abstractions.Entity.Model;

namespace Repair.Abstractions.Services;

public interface IReportService
{
    Task<string> CreateReport(IOrder order);
}
