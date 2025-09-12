using AttrectoTest.Domain.Common;

namespace AttrectoTest.Application.Contracts.Persistence;

public interface IBaseRepository<T> : IGenericRepository<T> where T : BaseEntity
{

}
