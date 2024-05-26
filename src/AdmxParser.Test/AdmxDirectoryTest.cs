namespace AdmxParser.Test;

public class AdmxDirectoryTest
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
