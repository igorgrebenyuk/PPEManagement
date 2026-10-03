using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using PPEManagement.Dal.Contracts.Repositories;

namespace PPEManagement.Context.Tests;

public class PPEManagementContextInMemory : IAsyncDisposable
{
    /// <summary>
    /// Контекст <see cref="FinalExerciseContext"/>
    /// </summary>
    protected PPEManagementContext Context { get; }

    /// <inheritdoc cref="IUnitOfWork"/>
    protected IUnitOfWork UnitOfWork => Context;

    /// <inheritdoc cref="IDbWriterContext"/>
    protected IDbWriterContext WriterContext => new TestWriterContext(Context);

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="FinalExerciseContextInMemory"/>
    /// </summary>
    protected PPEManagementContextInMemory()
    {
        var optionsBuilder = new DbContextOptionsBuilder<PPEManagementContext>()
            .UseInMemoryDatabase($"FinalExerciseContextTests{Guid.NewGuid()}")
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
        Context = new PPEManagementContext(optionsBuilder.Options);
    }

    /// <inheritdoc cref="IAsyncDisposable"/>
    public async ValueTask DisposeAsync()
    {
        await Context.Database.EnsureDeletedAsync();
        await Context.DisposeAsync();
    }
}