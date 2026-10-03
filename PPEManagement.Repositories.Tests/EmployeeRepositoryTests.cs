using FluentAssertions;
using Xunit;
using Ahatornn.TestGenerator;
using PPEManagement.Context.Tests;
using PPEManagement.Repositories.Contracts;
using PPEManagement.Entities;

namespace PPEManagement.Repositories.Tests;

/// <summary>
/// Тесты для <see cref="EmployeeRepository"/>
/// </summary>
public class EmployeeRepositoryTests : PPEManagementContextInMemory
{
    private readonly IEmployeeRepository repository;

    /// <summary>
    /// Инициализирует новый экземпляр тестов репозитория сотрудников 
    /// </summary>
    public EmployeeRepositoryTests()
    {
        repository = new EmployeeRepository(WriterContext, Context);
    }

    /// <summary>
    /// Должен вернуть пустую коллекцию, если в базе нет сотрудников
    /// </summary>
    [Fact]
    public async Task GetEmployeesShouldReturnEmpty()
    {
        // Act
        var items = await repository.GetEmployeesAsync(CancellationToken.None);

        // Assert
        items.Should().NotBeNull().And.BeEmpty();
    }

    /// <summary>
    /// Должен вернуть всех неудаленных сотрудников, если они есть в базе
    /// </summary>
    [Fact]
    public async Task GetEmployeesShouldReturnAllNotDeletedItems()
    {
        // Arrange
        var firstEmployee = TestEntityProvider.Shared.Create<Employee>(x => x.DeletedAt = null);
        var secondEmployee = TestEntityProvider.Shared.Create<Employee>(x => x.DeletedAt = null);
        await Context.AddRangeAsync(firstEmployee, secondEmployee);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var items = await repository.GetEmployeesAsync(CancellationToken.None);

        // Assert
        items.Should().NotBeNull()
            .And.HaveCount(2)
            .And.Contain(x => x.Id == firstEmployee.Id)
            .And.Contain(x => x.Id == secondEmployee.Id);
    }

    /// <summary>
    /// Не должен возвращать сотрудников, помеченных как удаленные
    /// </summary>
    [Fact]
    public async Task GetEmployeesShouldNotReturnDeletedItems()
    {
        // Arrange
        var activeEmployee = TestEntityProvider.Shared.Create<Employee>(x => x.DeletedAt = null);
        var deletedEmployee = TestEntityProvider.Shared.Create<Employee>(x => x.DeletedAt = DateTimeOffset.UtcNow);
        await Context.AddRangeAsync(activeEmployee, deletedEmployee);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var items = await repository.GetEmployeesAsync(CancellationToken.None);

        // Assert
        items.Should().NotBeNull()
            .And.HaveCount(1)
            .And.OnlyContain(x => x.Id == activeEmployee.Id);
    }

    /// <summary>
    /// Возвращает сотрудника по идентификатору, если он существует и не удален
    /// </summary>
    [Fact]
    public async Task GetEmployeeByIdShouldReturnEntityWhenExistsAndNotDeleted()
    {
        // Arrange
        var targetEmployee = TestEntityProvider.Shared.Create<Employee>(x => x.DeletedAt = null);
        await Context.AddAsync(targetEmployee);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetEmployeeByIdAsync(targetEmployee.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(targetEmployee.Id);
    }

    /// <summary>
    /// Возвращает null, если сотрудник помечен как удаленный
    /// </summary>
    [Fact]
    public async Task GetEmployeeByIdShouldReturnNullWhenEntityIsDeleted()
    {
        // Arrange
        var deletedEmployee = TestEntityProvider.Shared.Create<Employee>(x => x.DeletedAt = DateTimeOffset.UtcNow);
        await Context.AddAsync(deletedEmployee);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetEmployeeByIdAsync(deletedEmployee.Id, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Возвращает null, если сотрудник с указанным идентификатором не найден
    /// </summary>
    [Fact]
    public async Task GetEmployeeByIdShouldReturnNullWhenEntityDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await repository.GetEmployeeByIdAsync(nonExistentId, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Возвращает сотрудника по табельному номеру, если он существует и не удален
    /// </summary>
    [Fact]
    public async Task GetEmployeeByPersonnelNumberShouldReturnThisEmployeeWhenExistsAndNotDeleted()
    {
        // Arrange
        var targetEmployee = TestEntityProvider.Shared.Create<Employee>(x => {
            x.DeletedAt = null;
            x.PersonnelNumber = "EMP-001";
        });

        var deletedEmployee = TestEntityProvider.Shared.Create<Employee>(x => {
            x.DeletedAt = DateTimeOffset.UtcNow;
            x.PersonnelNumber = "EMP-001";
        });

        var otherEmployee = TestEntityProvider.Shared.Create<Employee>(x => {
            x.DeletedAt = null;
            x.PersonnelNumber = "EMP-002";
        });

        await Context.AddRangeAsync(targetEmployee, deletedEmployee, otherEmployee);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetEmployeeByPersonnelNumberAsync(targetEmployee.PersonnelNumber, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(targetEmployee.Id);
        result.DeletedAt.Should().BeNull();
    }

    /// <summary>
    /// Возвращает null, если сотрудник с указанным табельным номером не найден
    /// </summary>
    [Fact]
    public async Task GetEmployeeByPersonnelNumberShouldReturnNullWhenEntityDoesNotExist()
    {
        // Arrange
        var otherEmployee = TestEntityProvider.Shared.Create<Employee>(x => {
            x.DeletedAt = null;
            x.PersonnelNumber = "EMP-002";
        });
        await Context.AddAsync(otherEmployee);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetEmployeeByPersonnelNumberAsync("EMP-001", CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Должен возвращать сотрудника независимо от регистра символов в табельном номере
    /// </summary>
    [Fact]
    public async Task GetEmployeeByPersonnelNumberShouldBeCaseInsensitive()
    {
        // Arrange
        var targetEmployee = TestEntityProvider.Shared.Create<Employee>(x => {
            x.DeletedAt = null;
            x.PersonnelNumber = "EMP-001";
        });
        await Context.AddAsync(targetEmployee);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetEmployeeByPersonnelNumberAsync("emp-001", CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(targetEmployee.Id);
    }
}