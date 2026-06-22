namespace Cleverence.TestTask.Tests;

public class RepositorySmokeTests
{
    [Fact]
    public void CoreAssemblyMarker_IsAccessible()
    {
        Assert.NotNull(typeof(Cleverence.TestTask.Core.AssemblyMarker));
    }
}
