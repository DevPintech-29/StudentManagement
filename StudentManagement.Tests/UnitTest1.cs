using NPOI.SS.Formula.Functions;
using Xunit;

namespace StudentManagement.Tests;

public class UnitTest1
{
    [Fact]
    public void Application_Should_Have_Correct_Assembly_Name()
    {
        // Arrange
        var assemblyName = typeof(Program).Assembly.GetName().Name;

        // Assert
        Assert.Equal("StudentManagement", assemblyName);
    }

    [Fact]
    public void Test_Framework_Should_Work()
    {
        // Arrange
        var expected = 4;

        // Act
        var actual = 2 + 2;

        // Assert
        Assert.Equal(expected, actual);
    }
}
