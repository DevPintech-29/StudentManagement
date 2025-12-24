using StudentManagement.Models;

namespace StudentManagement.Tests.Models;

public class EmployeeTests
{
    [Fact]
    public void Employee_Should_Initialize_With_Default_Values()
    {
        // Arrange & Act
        var employee = new Employee();

        // Assert
        Assert.Equal(0, employee.Id);
        Assert.Null(employee.Name);
        Assert.Null(employee.Sex);
        Assert.Null(employee.DistrictId);
        Assert.Null(employee.ThanaId);
        Assert.Null(employee.VillageId);
        Assert.Equal(default(DateTime), employee.JoiningDate);
        Assert.False(employee.IsActive);
    }

    [Fact]
    public void Employee_Should_Set_Properties_Correctly()
    {
        // Arrange
        var joiningDate = new DateTime(2024, 1, 1);
        var employee = new Employee();

        // Act
        employee.Id = 1;
        employee.Name = "John Doe";
        employee.Sex = "Male";
        employee.Email = "john@company.com";
        employee.Phone = "1234567890";
        employee.IsActive = true;
        employee.JoiningDate = joiningDate;
        employee.DistrictId = 10;
        employee.ThanaId = 20;
        employee.VillageId = 30;

        // Assert
        Assert.Equal(1, employee.Id);
        Assert.Equal("John Doe", employee.Name);
        Assert.Equal("Male", employee.Sex);
        Assert.Equal("john@company.com", employee.Email);
        Assert.Equal("1234567890", employee.Phone);
        Assert.True(employee.IsActive);
        Assert.Equal(joiningDate, employee.JoiningDate);
        Assert.Equal(10, employee.DistrictId);
        Assert.Equal(20, employee.ThanaId);
        Assert.Equal(30, employee.VillageId);
    }

    [Theory]
    [InlineData("Alice", "Female", "alice@company.com")]
    [InlineData("Bob", "Male", "bob@company.com")]
    [InlineData("Charlie", "Other", "charlie@company.com")]
    public void Employee_Should_Accept_Valid_Data(string name, string sex, string email)
    {
        // Arrange & Act
        var employee = new Employee
        {
            Name = name,
            Sex = sex,
            Email = email,
            IsActive = true
        };

        // Assert
        Assert.Equal(name, employee.Name);
        Assert.Equal(sex, employee.Sex);
        Assert.Equal(email, employee.Email);
        Assert.True(employee.IsActive);
    }

    [Fact]
    public void Employee_Should_Handle_Optional_LocationIds()
    {
        // Arrange & Act
        var employee = new Employee
        {
            Name = "Test Employee",
            DistrictId = null,
            ThanaId = null,
            VillageId = null
        };

        // Assert
        Assert.Null(employee.DistrictId);
        Assert.Null(employee.ThanaId);
        Assert.Null(employee.VillageId);
    }
}
