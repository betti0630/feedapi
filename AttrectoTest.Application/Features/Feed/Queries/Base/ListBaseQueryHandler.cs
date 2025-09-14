using AttrectoTest.Application.Exceptions;
using AttrectoTest.Application.Features.Feed.Queries.ListComments;
using AttrectoTest.Application.Models;
using AttrectoTest.Domain;
using AttrectoTest.Domain.Common;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttrectoTest.Application.Features.Feed.Queries.Base;

internal abstract class ListBaseQueryHandler<T> where T : ListBaseQuery
{
    protected IQueryable<EntityType> AddPaging<EntityType>(IQueryable<EntityType> query, T request) where EntityType : BaseEntity
    {
        if (request.Page.HasValue && request.PageSize.HasValue)
        {
            var skip = (request.Page.Value - 1) * request.PageSize.Value;
            query = query.Skip(skip).Take(request.PageSize.Value);
        }
        return query;
    }


}
