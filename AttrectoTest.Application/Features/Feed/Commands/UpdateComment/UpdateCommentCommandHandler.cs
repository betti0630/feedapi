using AttrectoTest.Application.Features.Feed.Dtos;

using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttrectoTest.Application.Features.Feed.Commands.UpdateComment;

internal class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, CommentDto>
{
    public Task<CommentDto> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
    {
        //comment author
        throw new NotImplementedException();
    }
}
