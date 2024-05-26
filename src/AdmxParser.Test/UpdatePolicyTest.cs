using System.ComponentModel;

namespace AdmxParser.Test;

public class UpdatePolicyTest
{
    [Fact]
    public void Test_UpdatePolicy_PerformFullPolicyUpdate_Forcing()
    {
        // Act
        var result = UpdatePolicy.PerformFullPolicyUpdate(true, out Win32Exception e1, out Win32Exception e2);

        // Assert
        Assert.Null(e1);
        Assert.Null(e2);
        Assert.True(result);
    }

    [Fact]
    public void Test_UpdatePolicy_PerformFullPolicyUpdate()
    {
        // Act
        var result = UpdatePolicy.PerformFullPolicyUpdate(false, out Win32Exception e1, out Win32Exception e2);

        // Assert
        Assert.Null(e1);
        Assert.Null(e2);
        Assert.True(result);
    }

    [Fact]
    public void Test_UpdatePolicy_PerformMachinePolicyUpdate_Forcing()
    {
        // Act
        var result = UpdatePolicy.PerformMachinePolicyUpdate(true, out Win32Exception e);

        // Assert
        Assert.Null(e);
        Assert.True(result);
    }

    [Fact]
    public void Test_UpdatePolicy_PerformMachinePolicyUpdate()
    {
        // Act
        var result = UpdatePolicy.PerformMachinePolicyUpdate(false, out Win32Exception e);

        // Assert
        Assert.Null(e);
        Assert.True(result);
    }

    [Fact]
    public void Test_UpdatePolicy_PerformCurrentUserPolicyUpdate_Forcing()
    {
        // Act
        var result = UpdatePolicy.PerformCurrentUserPolicyUpdate(true, out Win32Exception e);

        // Assert
        Assert.Null(e);
        Assert.True(result);
    }

    [Fact]
    public void Test_UpdatePolicy_PerformCurrentUserPolicyUpdate()
    {
        // Act
        var result = UpdatePolicy.PerformCurrentUserPolicyUpdate(false, out Win32Exception e);

        // Assert
        Assert.Null(e);
        Assert.True(result);
    }
}
