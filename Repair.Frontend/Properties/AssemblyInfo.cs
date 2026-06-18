using System.Runtime.CompilerServices;
using Repair.Frontend.Properties;

[assembly: InternalsVisibleTo(AssemblyInfo.FRONTEND_TESTS)]
[assembly: InternalsVisibleTo(AssemblyInfo.PROXY_GEN)]

namespace Repair.Frontend.Properties;

internal static class AssemblyInfo
{
    internal const string FRONTEND_TESTS = "Repair.Tests.Frontend";
    internal const string PROXY_GEN = "DynamicProxyGenAssembly2";
}
