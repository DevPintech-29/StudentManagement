using Microsoft.AspNetCore.Mvc;
using Moq;
using StudentManagement.Controllers;
using StudentManagement.Data;
using StudentManagement.Models;

namespace StudentManagement.Tests.Controllers;

public class StudentControllerTests
{
    private readonly Mock<IGenericRepository<Student>> _mockRepository;
    private readonly StudentController _controller;

    public StudentControllerTests()
    {
        _mockRepository = new Mock<IGenericRepository<Student>>();
        _controller = new StudentController(_mockRepository.Object);
    }

    [Fact]
    public async Task Index_Should_Return_ViewResult_With_Students()
    {
        // Arrange
        var students = new List<Student>
        {
            new Student { StudentId = 1, Name = "John", Age = 20, Email = "john@test.com" },
            new Student { StudentId = 2, Name = "Jane", Age = 22, Email = "jane@test.com" }
        };
        _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(students);

        // Act
        var result = await _controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Student>>(viewResult.Model);
        Assert.Equal(2, model.Count());
    }

    [Fact]
    public async Task Details_Should_Return_ViewResult_When_Student_Exists()
    {
        // Arrange
        var student = new Student { StudentId = 1, Name = "John", Age = 20, Email = "john@test.com" };
        _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(student);

        // Act
        var result = await _controller.Details(1);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<Student>(viewResult.Model);
        Assert.Equal(1, model.StudentId);
        Assert.Equal("John", model.Name);
    }

    [Fact]
    public async Task Details_Should_Return_NotFound_When_Student_Does_Not_Exist()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.GetByIdAsync(999)).ReturnsAsync((Student?)null);

        // Act
        var result = await _controller.Details(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Create_Get_Should_Return_ViewResult()
    {
        // Act
        var result = _controller.Create();

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task Create_Post_Should_Add_Student_And_Redirect_When_Valid()
    {
        // Arrange
        var student = new Student { Name = "John", Age = 20, Email = "john@test.com" };
        _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Student>())).ReturnsAsync(1);

        // Act
        var result = await _controller.Create(student);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        _mockRepository.Verify(repo => repo.AddAsync(student), Times.Once);
    }

    [Fact]
    public async Task Create_Post_Should_Return_View_When_ModelState_Invalid()
    {
        // Arrange
        var student = new Student();
        _controller.ModelState.AddModelError("Name", "Required");

        // Act
        var result = await _controller.Create(student);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(student, viewResult.Model);
        _mockRepository.Verify(repo => repo.AddAsync(It.IsAny<Student>()), Times.Never);
    }

    [Fact]
    public async Task Edit_Get_Should_Return_ViewResult_When_Student_Exists()
    {
        // Arrange
        var student = new Student { StudentId = 1, Name = "John", Age = 20, Email = "john@test.com" };
        _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(student);

        // Act
        var result = await _controller.Edit(1);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<Student>(viewResult.Model);
        Assert.Equal(1, model.StudentId);
    }

    [Fact]
    public async Task Edit_Get_Should_Return_NotFound_When_Student_Does_Not_Exist()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.GetByIdAsync(999)).ReturnsAsync((Student?)null);

        // Act
        var result = await _controller.Edit(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Edit_Post_Should_Update_Student_And_Redirect_When_Valid()
    {
        // Arrange
        var student = new Student { StudentId = 1, Name = "John Updated", Age = 21, Email = "john@test.com" };
        _mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Student>())).ReturnsAsync(1);

        // Act
        var result = await _controller.Edit(student);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        _mockRepository.Verify(repo => repo.UpdateAsync(student), Times.Once);
    }

    [Fact]
    public async Task Edit_Post_Should_Return_View_When_ModelState_Invalid()
    {
        // Arrange
        var student = new Student { StudentId = 1 };
        _controller.ModelState.AddModelError("Name", "Required");

        // Act
        var result = await _controller.Edit(student);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(student, viewResult.Model);
        _mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Student>()), Times.Never);
    }

    [Fact]
    public async Task Delete_Get_Should_Return_ViewResult_When_Student_Exists()
    {
        // Arrange
        var student = new Student { StudentId = 1, Name = "John", Age = 20, Email = "john@test.com" };
        _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(student);

        // Act
        var result = await _controller.Delete(1);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<Student>(viewResult.Model);
        Assert.Equal(1, model.StudentId);
    }

    [Fact]
    public async Task Delete_Get_Should_Return_NotFound_When_Student_Does_Not_Exist()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.GetByIdAsync(999)).ReturnsAsync((Student?)null);

        // Act
        var result = await _controller.Delete(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteConfirmed_Should_Delete_Student_And_Redirect()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.DeleteAsync(1)).ReturnsAsync(1);

        // Act
        var result = await _controller.DeleteConfirmed(1);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        _mockRepository.Verify(repo => repo.DeleteAsync(1), Times.Once);
    }
}
