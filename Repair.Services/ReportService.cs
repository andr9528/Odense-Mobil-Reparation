using System.Diagnostics;
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
    private const string MISSING_COMPANY_WEBSITE_URL = "Mangler virksomhedens hjemmeside";
    private const string MISSING_COMPANY_ADDRESS = "Mangler virksomhedens adresse";
    private const string MISSING_COMPANY_PHONE_NUMBER = "Mangler virksomhedens telefonnummer";
    private const string MISSING_COMPANY_EMAIL = "Mangler virksomhedens email";
    private const string MISSING_COMPANY_LOGO_PATH = "Mangler sti til virksomhedens logo";

    private const string CUSTOMER_DETAILS_HEADER = "Kundeoplysninger";
    private const string CUSTOMER_NAME_LABEL = "Navn";
    private const string CUSTOMER_PHONE_LABEL = "Telefon";
    private const string CUSTOMER_EMAIL_LABEL = "Email";

    private const string ORDER_DETAILS_HEADER = "Ordreoplysninger";
    private const string HANDED_IN_LABEL = "Indleveret";
    private const string RETURNED_LABEL = "Udleveret";
    private const string BORROWED_PHONE_HEADER = "Lånetelefon";
    private const string NO_BORROWED_PHONE = "Ingen lånetelefon";
    private const string HAND_IN_WHAT_HEADER = "Indleveret";
    private const string REPAIR_NOTES_HEADER = "Reparationsnoter";
    private const string NO_DATE_TIME_SET = "Ingen dato/tid angivet";

    private const string CUSTOMER_ORDERS_HEADER = "Kundens ordrer";
    private const string IS_ORDER_COMPLETE_LABEL = "Ordre færdig";
    private const string YES_TEXT = "Ja";
    private const string NO_TEXT = "Nej";
    private const string NO_ORDERS_TEXT = "Kunden har ingen ordrer";

    public Task<string> CreateReport(ICustomer customer)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(customer);

            QuestPDF.Settings.License = LicenseType.Community;

            string reportFolder = Path.Combine(configurationService.GetApplicationDataPath(), REPORT_FOLDER_NAME);

            Directory.CreateDirectory(reportFolder);

            string filePath = Path.Combine(reportFolder, CreateReportFileName(customer));

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
                        AddCustomerDetails(column, customer);
                        AddCustomerOrders(column, customer);
                    });
                });
            }).GeneratePdf(filePath);
            OpenReportInDefaultViewer(filePath);

            logger.LogInformation("Created report for customer '{CustomerId}' at {ReportPath}", customer.Id, filePath);

            return Task.FromResult(filePath);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Caught exception while trying to create a Report for Customer '{CustomerId}'.",
                customer.Id);
            throw;
        }
    }

    private void OpenReportInDefaultViewer(string filePath)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = filePath,
            UseShellExecute = true,
        });

        logger.LogInformation("Opened report '{ReportPath}'.", filePath);
    }

    private void AddCustomerOrders(ColumnDescriptor column, ICustomer customer)
    {
        column.Item().Column(ordersColumn =>
        {
            ordersColumn.Spacing(15);

            ordersColumn.Item().Text(CUSTOMER_ORDERS_HEADER).FontSize(15).Bold();

            if (customer.Orders.Count == 0)
            {
                ordersColumn.Item().Text(NO_ORDERS_TEXT);
                return;
            }

            foreach (IOrder order in customer.Orders.OrderByDescending(x => x.HandInWhen))
                AddOrderDetails(ordersColumn, order);
        });
    }

    private string CreateReportFileName(ICustomer customer)
    {
        string customerName = SanitizeFileNamePart(customer.Name);
        var createdAt = DateTime.Now.ToString("yyyy-MM-dd_HH-mm");

        return $"{customerName} - {createdAt} - Ordrer.pdf";
    }

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
            OpenReportInDefaultViewer(filePath);

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
                company.Item().Text(GetTextValue(reportOptions.Value.CompanyWebsiteUrl, MISSING_COMPANY_WEBSITE_URL));
                company.Item().Text(GetTextValue(reportOptions.Value.CompanyAddress, MISSING_COMPANY_ADDRESS));
                company.Item().Text(GetTextValue(reportOptions.Value.CompanyPhoneNumber, MISSING_COMPANY_PHONE_NUMBER));
                company.Item().Text(GetTextValue(reportOptions.Value.CompanyEmail, MISSING_COMPANY_EMAIL));
            });
        });
    }

    private void AddCompanyLogo(IContainer container)
    {
        string? logoPath = GetCompanyLogoPath();

        if (logoPath is null)
        {
            container.Text(MISSING_COMPANY_LOGO_PATH).FontSize(18).Bold();
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

            customerColumn.Item().Text(CUSTOMER_DETAILS_HEADER).FontSize(15).Bold();

            customerColumn.Item().Text($"{CUSTOMER_NAME_LABEL}: {customer.Name}");
            customerColumn.Item().Text($"{CUSTOMER_PHONE_LABEL}: {customer.Phone}");
            customerColumn.Item().Text($"{CUSTOMER_EMAIL_LABEL}: {customer.Email}");
        });
    }

    private void AddOrderDetails(ColumnDescriptor column, IOrder order)
    {
        column.Item().Column(orderColumn =>
        {
            orderColumn.Spacing(5);

            orderColumn.Item().Text(ORDER_DETAILS_HEADER).FontSize(15).Bold();

            orderColumn.Item().Text($"{HANDED_IN_LABEL}: {FormatDateTime(order.HandInWhen)}");
            orderColumn.Item().Text($"{RETURNED_LABEL}: {FormatNullableDateTime(order.ReturnedWhen)}");
            orderColumn.Item().Text($"{IS_ORDER_COMPLETE_LABEL}: {FormatBoolean(order.IsOrderComplete)}");

            orderColumn.Item().PaddingTop(10).Text(BORROWED_PHONE_HEADER).Bold();
            orderColumn.Item().Text(GetTextValue(order.BorrowedPhone, NO_BORROWED_PHONE));

            orderColumn.Item().PaddingTop(10).Text(HAND_IN_WHAT_HEADER).Bold();
            orderColumn.Item().Text(order.HandInWhat);

            orderColumn.Item().PaddingTop(10).Text(REPAIR_NOTES_HEADER).Bold();
            orderColumn.Item().Text(order.RepairWhat);
        });
    }

    private string FormatDateTime(DateTime dateTime)
    {
        return dateTime.ToString("dd-MM-yyyy HH:mm");
    }

    private string FormatNullableDateTime(DateTime? dateTime)
    {
        return dateTime.HasValue ? FormatDateTime(dateTime.Value) : NO_DATE_TIME_SET;
    }

    private string FormatBoolean(bool value)
    {
        return value ? YES_TEXT : NO_TEXT;
    }
}
