using FluentAssertions;
using Xunit;
using Ahatornn.TestGenerator;
using PPEManagement.Context.Tests;
using PPEManagement.Repositories.Contracts;
using PPEManagement.Entities;

namespace PPEManagement.Repositories.Tests;

/// <summary>
/// Тесты для <see cref="PPEStatementItemRepository"/>
/// </summary>
public class PPEStatementItemRepositoryTests : PPEManagementContextInMemory
{
    private readonly IPPEStatementItemRepository repository;

    /// <summary>
    /// Инициализирует новый экземпляр тестов репозитория строк ведомостей
    /// </summary>
    public PPEStatementItemRepositoryTests()
    {
        repository = new PPEStatementItemRepository(WriterContext, Context);
    }

    /// <summary>
    /// Должен вернуть пустую коллекцию, если в базе нет строк ведомостей
    /// </summary>
    [Fact]
    public async Task GetPPEStatementItemsShouldReturnEmpty()
    {
        // Act
        var items = await repository.GetPPEStatementItemsAsync(CancellationToken.None);

        // Assert
        items.Should().NotBeNull().And.BeEmpty();
    }

    /// <summary>
    /// Должен вернуть все неудаленные строки ведомостей, если они есть в базе
    /// </summary>
    [Fact]
    public async Task GetPPEStatementItemsShouldReturnAllNotDeletedItems()
    {
        // Arrange
        var firstItem = TestEntityProvider.Shared.Create<PPEStatementItem>(x => x.DeletedAt = null);
        var secondItem = TestEntityProvider.Shared.Create<PPEStatementItem>(x => x.DeletedAt = null);
        await Context.AddRangeAsync(firstItem, secondItem);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var items = await repository.GetPPEStatementItemsAsync(CancellationToken.None);

        // Assert
        items.Should().NotBeNull()
            .And.HaveCount(2)
            .And.Contain(x => x.Id == firstItem.Id)
            .And.Contain(x => x.Id == secondItem.Id);
    }

    /// <summary>
    /// Возвращает строки по ID родительской ведомости
    /// </summary>
    [Fact]
    public async Task GetItemsByStatementIdShouldReturnOnlyItemsForGivenStatement()
    {
        // Arrange
        var targetStatementId = Guid.NewGuid();
        var otherStatementId = Guid.NewGuid();

        var item1 = TestEntityProvider.Shared.Create<PPEStatementItem>(x => {
            x.DeletedAt = null;
            x.StatementId = targetStatementId;
        });
        var item2 = TestEntityProvider.Shared.Create<PPEStatementItem>(x => {
            x.DeletedAt = null;
            x.StatementId = targetStatementId;
        });
        var otherItem = TestEntityProvider.Shared.Create<PPEStatementItem>(x => {
            x.DeletedAt = null;
            x.StatementId = otherStatementId;
        });

        await Context.AddRangeAsync(item1, item2, otherItem);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var items = await repository.GetItemsByStatementIdAsync(targetStatementId, CancellationToken.None);

        // Assert
        items.Should().NotBeNull()
            .And.HaveCount(2)
            .And.Contain(x => x.Id == item1.Id)
            .And.Contain(x => x.Id == item2.Id)
            .And.NotContain(x => x.Id == otherItem.Id);
    }
}