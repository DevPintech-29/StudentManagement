using StudentManagement.Models;
using Xunit;

namespace StudentManagement.Tests.Models;

public class StudentTests
{
    [Fact]
    public void Student_Should_Initialize_With_Default_Values()
    {
        // Arrange & Act
        var student = new Student();

        // Assert
        Assert.Equal(0, student.StudentId);
        Assert.Equal(string.Empty, student.Name);
        Assert.Equal(0, student.Age);
        Assert.Equal(string.Empty, student.Email);
    }

    [Fact]
    public void Student_Should_Set_Properties_Correctly()
    {
        // Arrange
        var student = new Student();

        // Act
        student.StudentId = 1;
        student.Name = "John Doe";
        student.Age = 20;
        student.Email = "john@example.com";

        // Assert
        Assert.Equal(1, student.StudentId);
        Assert.Equal("John Doe", student.Name);
        Assert.Equal(20, student.Age);
        Assert.Equal("john@example.com", student.Email);
    }

    [Theory]
    [InlineData("Alice", 18, "alice@test.com")]
    [InlineData("Bob", 25, "bob@test.com")]
    [InlineData("Charlie", 30, "charlie@test.com")]
    public void Student_Should_Accept_Valid_Data(string name, int age, string email)
    {
        // Arrange & Act
        var student = new Student
        {
            Name = name,
            Age = age,
            Email = email
        };

        // Assert
        Assert.Equal(name, student.Name);
        Assert.Equal(age, student.Age);
        Assert.Equal(email, student.Email);
    }
}
