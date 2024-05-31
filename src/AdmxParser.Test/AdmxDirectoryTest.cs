namespace AdmxParser.Test;

public class AdmxDirectoryTest
{
#if WINDOWS
    [Fact]
#else
    [Fact(Skip = "This test requires Windows.")]
#endif
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

#if WINDOWS
    [Fact]
#else
    [Fact(Skip = "This test requires Windows.")]
#endif
    public async Task Test_AdmlResource_GetString()
    {
        // Arrange
        var directory = AdmxDirectory.GetSystemPolicyDefinitions();
        var admxLoadResult = await directory.LoadAsync(true);
        var firstAdmxContents = directory.LoadedAdmxContents.First();
        var firstAdmlResource = firstAdmxContents.LoadedAdmlResources.Where(x => x.Value.StringKeys.Any()).First();
        var firstAdmlKey = firstAdmlResource.Value.StringKeys.First();

        // Act
        var str = firstAdmlResource.Value.GetString(firstAdmlKey);
        var str2 = firstAdmxContents.GetString(firstAdmlKey, firstAdmlResource.Key, false);

        // Assert
        Assert.True(admxLoadResult);
        Assert.True(directory.Loaded);
        Assert.NotEmpty(directory.LoadedAdmxContents);
        Assert.NotEmpty(directory.AvailableLanguages);

        Assert.NotNull(str);
        Assert.NotEmpty(str);
        Assert.NotNull(str2);
        Assert.NotEmpty(str2);
        Assert.Equal(str, str2);
    }
}
