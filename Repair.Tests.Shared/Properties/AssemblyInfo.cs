using System.Runtime.CompilerServices;
using Repair.Tests.Shared.Properties;

[assembly: InternalsVisibleTo(AssemblyInfo.FRONTEND_TESTS)]
[assembly: InternalsVisibleTo(AssemblyInfo.TESTS)]

namespace Repair.Tests.Shared.Properties;

internal static class AssemblyInfo
{
    internal const string FRONTEND_TESTS = "Repair.Tests.Frontend";
    internal const string TESTS = "Repair.Tests";
}
