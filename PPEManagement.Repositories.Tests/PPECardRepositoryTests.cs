using FluentAssertions;
using Xunit;
using Ahatornn.TestGenerator;
using PPEManagement.Context.Tests;
using PPEManagement.Repositories.Contracts;
using PPEManagement.Entities;

namespace PPEManagement.Repositories.Tests;

/// <summary>
/// Тесты для <see cref="PPECardRepository"/>
/// </summary>
public class PPECardRepositoryTests : PPEManagementContextInMemory
{
    private readonly IPPECardRepository repository;

    /// <summary>
    /// Инициализирует новый экземпляр тестов репозитория карточек СИЗ
    /// </summary>
    public PPECardRepositoryTests()
    {
        repository = new PPECardRepository(WriterContext, Context);
    }

    /// <summary>
    /// Должен вернуть пустую коллекцию, если в базе нет карточек СИЗ
    /// </summary>
    [Fact]
    public async Task GetPPECardsShouldReturnEmpty()
    {
        // Act
        var items = await repository.GetPPECardsAsync(CancellationToken.None);

        // Assert
        items.Should().NotBeNull().And.BeEmpty();
    }

    /// <summary>
    /// Должен вернуть все неудаленные карточки СИЗ, если они есть в базе
    /// </summary>
    [Fact]
    public async Task GetPPECardsShouldReturnAllNotDeletedItems()
    {
        // Arrange
        var firstCard = TestEntityProvider.Shared.Create<PPECard>(x => x.DeletedAt = null);
        var secondCard = TestEntityProvider.Shared.Create<PPECard>(x => x.DeletedAt = null);
        await Context.AddRangeAsync(firstCard, secondCard);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var items = await repository.GetPPECardsAsync(CancellationToken.None);

        // Assert
        items.Should().NotBeNull()
            .And.HaveCount(2)
            .And.Contain(x => x.Id == firstCard.Id)
            .And.Contain(x => x.Id == secondCard.Id);
    }

    /// <summary>
    /// Не должен возвращать карточки СИЗ, помеченные как удаленные
    /// </summary>
    [Fact]
    public async Task GetPPECardsShouldNotReturnDeletedItems()
    {
        // Arrange
        var activeCard = TestEntityProvider.Shared.Create<PPECard>(x => x.DeletedAt = null);
        var deletedCard = TestEntityProvider.Shared.Create<PPECard>(x => x.DeletedAt = DateTimeOffset.UtcNow);
        await Context.AddRangeAsync(activeCard, deletedCard);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var items = await repository.GetPPECardsAsync(CancellationToken.None);

        // Assert
        items.Should().NotBeNull()
            .And.HaveCount(1)
            .And.OnlyContain(x => x.Id == activeCard.Id);
    }

    /// <summary>
    /// Возвращает карточку СИЗ по идентификатору, если она существует и не удалена
    /// </summary>
    [Fact]
    public async Task GetPPECardByIdShouldReturnEntityWhenExistsAndNotDeleted()
    {
        // Arrange
        var targetCard = TestEntityProvider.Shared.Create<PPECard>(x => x.DeletedAt = null);
        await Context.AddAsync(targetCard);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetPPECardByIdAsync(targetCard.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(targetCard.Id);
    }

    /// <summary>
    /// Возвращает null, если карточка СИЗ помечена как удаленная
    /// </summary>
    [Fact]
    public async Task GetPPECardByIdShouldReturnNullWhenEntityIsDeleted()
    {
        // Arrange
        var deletedCard = TestEntityProvider.Shared.Create<PPECard>(x => x.DeletedAt = DateTimeOffset.UtcNow);
        await Context.AddAsync(deletedCard);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetPPECardByIdAsync(deletedCard.Id, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Возвращает null, если карточка СИЗ с указанным идентификатором не найдена
    /// </summary>
    [Fact]
    public async Task GetPPECardByIdShouldReturnNullWhenEntityDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await repository.GetPPECardByIdAsync(nonExistentId, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Возвращает карточку СИЗ по наименованию и номеру партии, если она существует и не удалена
    /// </summary>
    [Fact]
    public async Task GetPPECardByNameAndBatchShouldReturnThisCardWhenExistsAndNotDeleted()
    {
        // Arrange
        var targetCard = TestEntityProvider.Shared.Create<PPECard>(x => {
            x.DeletedAt = null;
            x.Name = "Перчатки";
            x.BatchNumber = "BATCH-001";
        });

        var deletedCard = TestEntityProvider.Shared.Create<PPECard>(x => {
            x.DeletedAt = DateTimeOffset.UtcNow;
            x.Name = "Перчатки";
            x.BatchNumber = "BATCH-001";
        });

        var otherCard = TestEntityProvider.Shared.Create<PPECard>(x => {
            x.DeletedAt = null;
            x.Name = "Каска";
            x.BatchNumber = "BATCH-002";
        });

        await Context.AddRangeAsync(targetCard, deletedCard, otherCard);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetPPECardByNameAndBatchAsync(targetCard.Name, targetCard.BatchNumber, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(targetCard.Id);
        result.DeletedAt.Should().BeNull();
    }

    /// <summary>
    /// Должен возвращать карточку СИЗ независимо от регистра наименования и партии
    /// </summary>
    [Fact]
    public async Task GetPPECardByNameAndBatchShouldBeCaseInsensitive()
    {
        // Arrange
        var targetCard = TestEntityProvider.Shared.Create<PPECard>(x => {
            x.DeletedAt = null;
            x.Name = "Перчатки";
            x.BatchNumber = "BATCH-001";
        });
        await Context.AddAsync(targetCard);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetPPECardByNameAndBatchAsync("перчатки", "batch-001", CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(targetCard.Id);
    }
}