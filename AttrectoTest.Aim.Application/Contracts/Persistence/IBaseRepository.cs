
using AttrectoTest.Aim.Domain.Common;

namespace AttrectoTest.Aim.Application.Contracts.Persistence;

public interface IBaseRepository<T> : IGenericRepository<T> where T : BaseEntity
{

}
