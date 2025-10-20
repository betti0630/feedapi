
using AttrectoTest.Iam.Domain.Common;

namespace AttrectoTest.Iam.Application.Contracts.Persistence;

public interface IBaseRepository<T> : IGenericRepository<T> where T : BaseEntity
{

}
