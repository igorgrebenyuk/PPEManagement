using FluentAssertions;
using Xunit;
using Ahatornn.TestGenerator;
using PPEManagement.Context.Tests;
using PPEManagement.Repositories.Contracts;
using PPEManagement.Entities;

namespace PPEManagement.Repositories.Tests;

/// <summary>
/// Тесты для <see cref="PPEStatementRepository"/>
/// </summary>
public class PPEStatementRepositoryTests : PPEManagementContextInMemory
{
    private readonly IPPEStatementRepository repository;

    /// <summary>
    /// Инициализирует новый экземпляр тестов репозитория ведомостей СИЗ
    /// </summary>
    public PPEStatementRepositoryTests()
    {
        repository = new PPEStatementRepository(WriterContext, Context);
    }

    /// <summary>
    /// Должен вернуть пустую коллекцию, если в базе нет ведомостей
    /// </summary>
    [Fact]
    public async Task GetPPEStatementsShouldReturnEmpty()
    {
        // Act
        var items = await repository.GetPPEStatementsAsync(CancellationToken.None);

        // Assert
        items.Should().NotBeNull().And.BeEmpty();
    }

    /// <summary>
    /// Должен вернуть все неудаленные ведомости, если они есть в базе
    /// </summary>
    [Fact]
    public async Task GetPPEStatementsShouldReturnAllNotDeletedItems()
    {
        // Arrange
        var firstStatement = TestEntityProvider.Shared.Create<PPEStatement>(x => x.DeletedAt = null);
        var secondStatement = TestEntityProvider.Shared.Create<PPEStatement>(x => x.DeletedAt = null);
        await Context.AddRangeAsync(firstStatement, secondStatement);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var items = await repository.GetPPEStatementsAsync(CancellationToken.None);

        // Assert
        items.Should().NotBeNull()
            .And.HaveCount(2)
            .And.Contain(x => x.Id == firstStatement.Id)
            .And.Contain(x => x.Id == secondStatement.Id);
    }

    /// <summary>
    /// Не должен возвращать ведомости, помеченные как удаленные
    /// </summary>
    [Fact]
    public async Task GetPPEStatementsShouldNotReturnDeletedItems()
    {
        // Arrange
        var activeStatement = TestEntityProvider.Shared.Create<PPEStatement>(x => x.DeletedAt = null);
        var deletedStatement = TestEntityProvider.Shared.Create<PPEStatement>(x => x.DeletedAt = DateTimeOffset.UtcNow);
        await Context.AddRangeAsync(activeStatement, deletedStatement);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var items = await repository.GetPPEStatementsAsync(CancellationToken.None);

        // Assert
        items.Should().NotBeNull()
            .And.HaveCount(1)
            .And.OnlyContain(x => x.Id == activeStatement.Id);
    }

    /// <summary>
    /// Возвращает ведомость по идентификатору, если она существует и не удалена
    /// </summary>
    [Fact]
    public async Task GetPPEStatementByIdShouldReturnEntityWhenExistsAndNotDeleted()
    {
        // Arrange
        var targetStatement = TestEntityProvider.Shared.Create<PPEStatement>(x => x.DeletedAt = null);
        await Context.AddAsync(targetStatement);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetPPEStatementByIdAsync(targetStatement.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(targetStatement.Id);
    }

    /// <summary>
    /// Возвращает null, если ведомость помечена как удаленная
    /// </summary>
    [Fact]
    public async Task GetPPEStatementByIdShouldReturnNullWhenEntityIsDeleted()
    {
        // Arrange
        var deletedStatement = TestEntityProvider.Shared.Create<PPEStatement>(x => x.DeletedAt = DateTimeOffset.UtcNow);
        await Context.AddAsync(deletedStatement);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetPPEStatementByIdAsync(deletedStatement.Id, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Возвращает ведомость по номеру, если она существует и не удалена
    /// </summary>
    [Fact]
    public async Task GetPPEStatementByNumberShouldReturnThisStatementWhenExistsAndNotDeleted()
    {
        // Arrange
        var targetStatement = TestEntityProvider.Shared.Create<PPEStatement>(x => {
            x.DeletedAt = null;
            x.StatementNumber = "ST-2026-001";
        });

        var deletedStatement = TestEntityProvider.Shared.Create<PPEStatement>(x => {
            x.DeletedAt = DateTimeOffset.UtcNow;
            x.StatementNumber = "ST-2026-001";
        });

        await Context.AddRangeAsync(targetStatement, deletedStatement);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetPPEStatementByNumberAsync(targetStatement.StatementNumber, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(targetStatement.Id);
        result.DeletedAt.Should().BeNull();
    }
}