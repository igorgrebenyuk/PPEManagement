using Ahatornn.TestGenerator;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using PPEManagement.Context.Tests;
using PPEManagement.Repositories; 
using PPEManagement.Entities;
using PPEManagement.Services.AutoMapper;
using PPEManagement.Services.Contracts.Exceptions;
using PPEManagement.Services.Contracts.Models.PPECard;
using Xunit;

namespace PPEManagement.Services.Tests;

    /// <summary>
    /// Тесты для <see cref="PPECardService"/>
    /// </summary>
    public class PPECardServiceTests : PPEManagementContextInMemory
    {
        private readonly PPECardService ppeCardService;

        /// <summary>
        /// ctor
        /// </summary>
        public PPECardServiceTests()
        {
            var ppeCardRepository = new PPECardRepository(WriterContext, Context);
            
            var profile = new ServiceProfile(); // Используйте ваш AutoMapper Profile
            var mapper = new MapperConfiguration(
                x => x.AddProfile(profile),
                NullLoggerFactory.Instance)
                .CreateMapper();

            var createValidator = new InlineValidator<PPECardCreateModel>();
            var updateValidator = new InlineValidator<PPECardUpdateModel>();

            ppeCardService = new PPECardService(
                ppeCardRepository,
                UnitOfWork,
                mapper,
                createValidator,
                updateValidator);
        }

        /// <summary>
        /// Тест на получение всех карточек СИЗ, когда их нет
        /// </summary>
        [Fact]
        public async Task GetPPECardsAsyncShouldReturnEmpty()
        {
            // Act
            var items = await ppeCardService.GetPPECardsAsync(CancellationToken.None);

            // Assert
            items.Should().NotBeNull()
                .And.BeEmpty();
        }

        /// <summary>
        /// Тест на получение всех карточек СИЗ, когда они есть
        /// </summary>
        [Fact]
        public async Task GetPPECardsAsyncShouldReturnPPECards()
        {
            // Arrange
            var item1 = TestEntityProvider.Shared.Create<PPECard>();
            var item2 = TestEntityProvider.Shared.Create<PPECard>();
            var item3 = TestEntityProvider.Shared.Create<PPECard>();

            Context.AddRange(item1, item2, item3);
            await UnitOfWork.SaveChangesAsync(CancellationToken.None);

            // Act
            var items = await ppeCardService.GetPPECardsAsync(CancellationToken.None);

            // Assert
            items.Should().HaveCount(3)
                .And.Contain(x => x.Id == item1.Id)
                .And.Contain(x => x.Id == item2.Id)
                .And.Contain(x => x.Id == item3.Id);
        }

        /// <summary>
        /// Тест на получение карточки СИЗ по Id
        /// </summary>
        [Fact]
        public async Task GetPPECardByIdAsyncShouldReturnPPECard()
        {
            // Arrange
            var item = TestEntityProvider.Shared.Create<PPECard>();
            Context.Add(item);
            await UnitOfWork.SaveChangesAsync(CancellationToken.None);

            // Act
            var result = await ppeCardService.GetPPECardByIdAsync(
                item.Id,
                CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(item.Id);
        }

        /// <summary>
        /// Тест на получение карточки СИЗ по Id, когда ее нет
        /// </summary>
        [Fact]
        public async Task GetPPECardByIdAsyncShouldThrowNotFoundException()
        {
            // Act
            var act = () => ppeCardService.GetPPECardByIdAsync(
                Guid.NewGuid(),
                CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<EntityNotFoundException<PPECard>>();
        }

        /// <summary>
        /// Тест на создание карточки СИЗ
        /// </summary>
        [Fact]
        public async Task AddPPECardAsyncShouldWork()
        {
            // Arrange
            var model = TestEntityProvider.Shared.Create<PPECardCreateModel>();

            // Act
            var result = await ppeCardService.AddPPECardAsync(
                model,
                CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().NotBeEmpty();

            Context.Set<PPECard>()
                .Should()
                .ContainSingle(x => x.Id == result.Id);
        }

        /// <summary>
        /// Тест на обновление карточки СИЗ
        /// </summary>
        [Fact]
        public async Task UpdatePPECardAsyncShouldWork()
        {
            // Arrange
            var item = TestEntityProvider.Shared.Create<PPECard>();
            Context.Add(item);
            await UnitOfWork.SaveChangesAsync(CancellationToken.None);

            var updateModel = TestEntityProvider.Shared.Create<PPECardUpdateModel>();
            updateModel.Id = item.Id;

            // Act
            await ppeCardService.UpdatePPECardAsync(
                updateModel,
                CancellationToken.None);

            // Assert
            var updatedItem = await Context.Set<PPECard>()
                .FindAsync(item.Id);

            updatedItem.Should().NotBeNull();
            updatedItem.Should().BeEquivalentTo(updateModel, options => options.ExcludingMissingMembers());

        }

        /// <summary>
        /// Тест на обновление карточки СИЗ, когда ее нет
        /// </summary>
        [Fact]
        public async Task UpdatePPECardAsyncShouldThrowNotFoundException()
        {
            // Arrange
            var updateModel = TestEntityProvider.Shared.Create<PPECardUpdateModel>();

            // Act
            var act = () => ppeCardService.UpdatePPECardAsync(
                updateModel,
                CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<EntityNotFoundException<PPECard>>();
        }

        /// <summary>
        /// Тест на удаление карточки СИЗ
        /// </summary>
        [Fact]
        public async Task DeletePPECardAsyncShouldWork()
        {
            // Arrange
            var item = TestEntityProvider.Shared.Create<PPECard>();
            Context.Add(item);
            await UnitOfWork.SaveChangesAsync(CancellationToken.None);

            // Act
            await ppeCardService.DeletePPECardAsync(
                item.Id,
                CancellationToken.None);

            // Assert
            var result = await Context.Set<PPECard>()
                .IgnoreQueryFilters()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == item.Id);

            result.Should().NotBeNull();
            result.DeletedAt.Should().NotBeNull();
        }

        /// <summary>
        /// Тест на удаление карточки СИЗ, когда ее нет
        /// </summary>
        [Fact]
        public async Task DeletePPECardAsyncShouldThrowNotFoundException()
        {
            // Act
            var act = () => ppeCardService.DeletePPECardAsync(
                Guid.NewGuid(),
                CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<EntityNotFoundException<PPECard>>();
        }
    }
