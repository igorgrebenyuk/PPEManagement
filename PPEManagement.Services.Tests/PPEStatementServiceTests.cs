using Ahatornn.TestGenerator;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using PPEManagement.Context.Tests;
using PPEManagement.Repositories; 
using PPEManagement.Entities;
using PPEManagement.Repositories.Contracts;
using PPEManagement.Services.AutoMapper;
using PPEManagement.Services.Contracts.Exceptions;
using PPEManagement.Services.Contracts.Models.PPEStatement;
using Xunit;

namespace PPEManagement.Services.Tests;

/// <summary>
/// Тесты для <see cref="PPEStatementService"/>
/// </summary>
public class PPEStatementServiceTests : PPEManagementContextInMemory
{
    private readonly PPEStatementService ppeStatementService;
    private readonly IPPEStatementItemRepository itemRepository;

    /// <summary>
    /// ctor
    /// </summary>
    public PPEStatementServiceTests()
    {
        var ppeStatementRepository = new PPEStatementRepository(WriterContext, Context);
    
        // Создаем экземпляр профиля маппинга
        var profile = new ServiceProfile();
        var mapper = new MapperConfiguration(
                x => x.AddProfile(profile),
                NullLoggerFactory.Instance)
            .CreateMapper();

        var createValidator = new InlineValidator<PPEStatementCreateModel>();

        ppeStatementService = new PPEStatementService(
            ppeStatementRepository,
            itemRepository,
            UnitOfWork,
            mapper,
            createValidator);
    }

    /// <summary>
    /// Проверяет получение ведомостей из пустой базы данных
    /// </summary>
    [Fact]
    public async Task GetPPEStatementsAsyncShouldReturnEmpty()
    {
        // Act
        var items = await ppeStatementService.GetPPEStatementsAsync(CancellationToken.None);

        // Assert
        items.Should().NotBeNull()
            .And.BeEmpty();
    }

    /// <summary>
    /// Проверяет получение всех ведомостей из базы данных
    /// </summary>
    [Fact]
    public async Task GetPPEStatementsAsyncShouldReturnStatements()
    {
        // Arrange
        var item1 = TestEntityProvider.Shared.Create<PPEStatement>();
        var item2 = TestEntityProvider.Shared.Create<PPEStatement>();

        Context.AddRange(item1, item2);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var items = await ppeStatementService.GetPPEStatementsAsync(CancellationToken.None);

        // Assert
        items.Should().HaveCount(2)
            .And.ContainSingle(x => x.Id == item1.Id)
            .And.ContainSingle(x => x.Id == item2.Id);
    }

    /// <summary>
    /// Проверяет получение ведомости по идентификатору
    /// </summary>
    [Fact]
    public async Task GetPPEStatementByIdAsyncShouldReturnStatement()
    {
        // Arrange
        var entity = TestEntityProvider.Shared.Create<PPEStatement>();
        Context.Add(entity);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await ppeStatementService.GetPPEStatementByIdAsync(
            entity.Id,
            CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(entity.Id);
    }

    /// <summary>
    /// Проверяет получение ведомости по идентификатору, когда она не существует
    /// </summary>
    [Fact]
    public async Task GetPPEStatementByIdAsyncShouldThrowNotFoundException()
    {
        // Act
        var act = () => ppeStatementService.GetPPEStatementByIdAsync(
            Guid.NewGuid(),
            CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<PPEStatement>>();
    }

    /// <summary>
    /// Проверяет добавление ведомости и корректный автоматический расчет категорий СИЗ
    /// </summary>
    [Fact]
    public async Task AddPPEStatementAsyncShouldWorkAndCalculateTotals()
    {
        // Arrange
        var createModel = TestEntityProvider.Shared.Create<PPEStatementCreateModel>(x =>
        {
            x.Items = new List<PPEStatementItemCreateModel>
            {
                new() { PPEName = "Противогаз ГП-7" },
                new() { PPEName = "КИМГЗ аптечка" },
                new() { PPEName = "Респиратор Алина" }
            };
        });

        // Act
        var result = await ppeStatementService.AddPPEStatementAsync(
            createModel,
            CancellationToken.None);

        // Assert
        result.Should().NotBeNull();

        var savedEntity = await Context.Set<PPEStatement>()
            .FirstOrDefaultAsync(x => x.Id == result.Id);

        savedEntity.Should().NotBeNull();
        savedEntity!.TotalGasMasks.Should().Be(1);
        savedEntity.TotalKIMGZ.Should().Be(1);
        savedEntity.TotalOtherPPE.Should().Be(1);
    }

    /// <summary>
    /// Проверяет удаление ведомости
    /// </summary>
    [Fact]
    public async Task DeletePPEStatementAsyncShouldWork()
    {
        // Arrange
        var entity = TestEntityProvider.Shared.Create<PPEStatement>();
        Context.Add(entity);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        await ppeStatementService.DeletePPEStatementAsync(
            entity.Id,
            CancellationToken.None);

        // Assert
        var result = await Context.Set<PPEStatement>()
            .IgnoreQueryFilters()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == entity.Id);

        result.Should().NotBeNull();
        result.DeletedAt.Should().NotBeNull();
    }

    /// <summary>
    /// Проверяет удаление несуществующей ведомости
    /// </summary>
    [Fact]
    public async Task DeletePPEStatementAsyncShouldThrowNotFoundException()
    {
        // Act
        var act = () => ppeStatementService.DeletePPEStatementAsync(
            Guid.NewGuid(),
            CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<PPEStatement>>();
    }
}