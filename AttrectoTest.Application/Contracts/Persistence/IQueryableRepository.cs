using AttrectoTest.Domain.Common;

namespace AttrectoTest.Application.Contracts.Persistence;

public interface IQueryableRepository<T>: IGenericRepository<T>, IDisposable where T : AppEntity
{
    IQueryable<T> List();
}
