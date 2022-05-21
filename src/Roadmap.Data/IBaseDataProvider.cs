namespace Roadmap.Data;

public interface IBaseDataProvider
{
    void Save();

    Task SaveAsync(CancellationToken cancellationToken = default);

    object MakeEntityDetached(object obj);

    void EnsureDeleted();

    bool IsInMemory();
}