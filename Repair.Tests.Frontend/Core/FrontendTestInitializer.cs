using TUnit.Core;

namespace Repair.Tests.Frontend.Core;

public static class FrontendTestInitializer
{
    [Before(TestSession)]
    public static void InitializeApp()
    {
        try
        {
            if (Application.Current is not null)
            {
                return;
            }

            _ = new Repair.Frontend.App();
        }
        catch (Exception exception)
        {
            throw new InvalidOperationException("Failed to initialize Repair.Frontend.App for frontend tests",
                exception);
        }
    }
}
