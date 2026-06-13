using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Repair.Abstractions.Entity.Model;
using Repair.Abstractions.Services;
using Repair.Models.Entity.Model;
using Repair.Models.Settings;

namespace Repair.Services;

public class ReportService(
    ConfigurationService configurationService,
    ILogger<ReportService> logger,
    IOptions<ReportDataSettings> reportOptions) : IReportService
{
    private const string REPORT_FOLDER_NAME = "Reports";

    /// <inheritdoc />
    public Task<string> CreateReport(IOrder order)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(order);

            QuestPDF.Settings.License = LicenseType.Community;

            string reportFolder = Path.Combine(configurationService.GetApplicationDataPath(), REPORT_FOLDER_NAME);

            Directory.CreateDirectory(reportFolder);

            string filePath = Path.Combine(reportFolder, CreateReportFileName(order));

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(40);
                    page.DefaultTextStyle(x => x.FontSize(11));

                    page.Content().Column(column =>
                    {
                        column.Spacing(25);

                        AddCompanyDetails(column);
                        AddCustomerDetails(column, order.Customer);
                        AddOrderDetails(column, order);
                    });
                });
            }).GeneratePdf(filePath);

            logger.LogInformation("Created report for order '{OrderId}' at {ReportPath}", order.Id, filePath);

            return Task.FromResult(filePath);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Caught exception while trying to create a Report for Order '{OrderId}'.", order.Id);
            throw;
        }
    }

    private string CreateReportFileName(IOrder order)
    {
        string customerName = SanitizeFileNamePart(order.Customer.Name);
        var handInWhen = order.HandInWhen.ToString("yyyy-MM-dd_HH-mm-ss");
        string handInWhat = SanitizeFileNamePart(order.HandInWhat);

        return $"{customerName} - {handInWhen} - {handInWhat}.pdf";
    }

    private string SanitizeFileNamePart(string value)
    {
        foreach (char invalidCharacter in Path.GetInvalidFileNameChars())
        {
            value = value.Replace(invalidCharacter, '-');
        }

        return value.Trim();
    }

    private void AddCompanyDetails(ColumnDescriptor column)
    {
        column.Item().Row(row =>
        {
            row.RelativeItem().Element(AddCompanyLogo);

            row.RelativeItem().AlignRight().Column(company =>
            {
                company.Item().Text(GetTextValue(reportOptions.Value.CompanyWebsiteUrl, "Missing Company Website URL"));

                company.Item().Text(GetTextValue(reportOptions.Value.CompanyAddress, "Missing Company Address"));

                company.Item()
                    .Text(GetTextValue(reportOptions.Value.CompanyPhoneNumber, "Missing Company Phone Number"));

                company.Item().Text(GetTextValue(reportOptions.Value.CompanyEmail, "Missing Company Email"));
            });
        });
    }

    private void AddCompanyLogo(IContainer container)
    {
        string? logoPath = GetCompanyLogoPath();

        if (logoPath is null)
        {
            container.Text("Missing Company Logo Path").FontSize(18).Bold();
            return;
        }

        container.Height(80).Image(logoPath).FitArea();
    }

    private string? GetCompanyLogoPath()
    {
        string logoPath = Path.Combine(configurationService.GetApplicationDataPath(),
            reportOptions.Value.CompanyLogoPath);

        if (!File.Exists(logoPath))
        {
            return null;
        }

        string extension = Path.GetExtension(logoPath);

        return extension.ToLowerInvariant() switch
        {
            ".png" or ".jpg" or ".jpeg" => logoPath,
            var _ => null,
        };
    }

    private string GetTextValue(string? value, string fallback)
    {
        return value ?? fallback;
    }

    private void AddCustomerDetails(ColumnDescriptor column, ICustomer customer)
    {
        column.Item().Column(customerColumn =>
        {
            customerColumn.Spacing(5);

            customerColumn.Item().Text("Customer Details").FontSize(15).Bold();

            customerColumn.Item().Text($"Name: {customer.Name}");
            customerColumn.Item().Text($"Phone: {customer.Phone}");
            customerColumn.Item().Text($"Email: {customer.Email}");
        });
    }

    private void AddOrderDetails(ColumnDescriptor column, IOrder order)
    {
        column.Item().Column(orderColumn =>
        {
            orderColumn.Spacing(5);

            orderColumn.Item().Text("Order Details").FontSize(15).Bold();

            orderColumn.Item().Text($"Handed In: {FormatDateTime(order.HandInWhen)}");
            orderColumn.Item().Text($"Returned: {FormatNullableDateTime(order.ReturnedWhen)}");
            orderColumn.Item().Text($"Borrowed Phone: {GetTextValue(order.BorrowedPhone, "No Borrowed Phone")}");

            orderColumn.Item().PaddingTop(10).Text("Hand In What").Bold();
            orderColumn.Item().Text(order.HandInWhat);

            orderColumn.Item().PaddingTop(10).Text("Repair Notes").Bold();
            orderColumn.Item().Text(order.RepairWhat);
        });
    }

    private string FormatDateTime(DateTime dateTime)
    {
        return dateTime.ToString("dd-MM-yyyy HH:mm");
    }

    private string FormatNullableDateTime(DateTime? dateTime)
    {
        return dateTime.HasValue ? FormatDateTime(dateTime.Value) : "No Date/Time Set";
    }
}
