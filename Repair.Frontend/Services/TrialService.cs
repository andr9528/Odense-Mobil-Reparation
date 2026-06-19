using Repair.Frontend.Extensions;
using Repair.Frontend.Presentation.Factory;

namespace Repair.Frontend.Services;

public class TrialService
{
    private const bool USE_TRIAL_SERVICE = true;

    private static readonly DateTime TrialEndDate = new(2026, 7, 4);
    private static readonly DateTime TrialExpirationDate = TrialEndDate.AddDays(1);

    private bool hasShownTrialCountdown;

    internal NavigationResult GetNavigationElementOrDefault(UIElement requestedElement, string requestedName)
    {
        if (!USE_TRIAL_SERVICE)
        {
            return new NavigationResult(requestedElement, requestedName);
        }

        if (IsTrialExpired())
        {
            return new NavigationResult(CreateTrialExpiredElement(), "Trial Expired");
        }

        if (hasShownTrialCountdown)
        {
            return new NavigationResult(requestedElement, requestedName);
        }

        hasShownTrialCountdown = true;

        return new NavigationResult(CreateTrialCountdownElement(), "Trial Countdown");
    }

    private bool IsTrialExpired()
    {
        return USE_TRIAL_SERVICE && DateTime.Now >= TrialExpirationDate;
    }

    private int DaysRemaining()
    {
        return Math.Max(0, (TrialExpirationDate.Date - DateTime.Now.Date).Days);
    }

    private UIElement CreateTrialCountdownElement()
    {
        return CreateCenteredText("Welcome to the Trial Version",
            $"This application can be used until {TrialEndDate:dd-MM-yyyy}.",
            $"{DaysRemaining()} day(s) remaining in the trial period.");
    }

    private UIElement CreateTrialExpiredElement()
    {
        return CreateCenteredText("Trial Period Expired", $"The trial period ended on {TrialEndDate:dd-MM-yyyy}.",
            "Please contact the developer to continue using the application.");
    }

    private UIElement CreateCenteredText(string header, string lineOne, string lineTwo)
    {
        Grid grid = GridFactory.CreateDefaultGrid().DefineRows(GridLength.Auto, GridLength.Auto, GridLength.Auto);

        grid.Children.Add(TextBlockFactory.CreateHeader(header).SetRow(0));

        TextBlock lineOneElement = TextBlockFactory.CreateBlackText(lineOne).SetRow(1);
        lineOneElement.HorizontalAlignment = HorizontalAlignment.Center;
        grid.Children.Add(lineOneElement);

        TextBlock lineTwoElement = TextBlockFactory.CreateBlackText(lineTwo).SetRow(2);
        lineTwoElement.HorizontalAlignment = HorizontalAlignment.Center;
        grid.Children.Add(lineTwoElement);

        grid.VerticalAlignment = VerticalAlignment.Center;
        grid.HorizontalAlignment = HorizontalAlignment.Center;

        return grid;
    }

    internal record NavigationResult(UIElement Element, string DisplayName);
}
