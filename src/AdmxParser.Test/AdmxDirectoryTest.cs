#pragma warning disable CS9113

using Xunit.Abstractions;

namespace AdmxParser.Test;

public class AdmxDirectoryTest(
    ITestOutputHelper Output)
{
    [Fact]
    public async Task Test_AdmxDirectoryTest()
    {
        // Arrange
        var directory = AdmxDirectory.GetSystemPolicyDefinitions();
        
        // Act
        var result = await directory.LoadAsync(true);
        
        // Assert
        Assert.True(result);
        Assert.True(directory.Loaded);
        Assert.NotEmpty(directory.LoadedAdmxContents);
        Assert.NotEmpty(directory.AvailableLanguages);
    }
}

#pragma warning restore CS9113