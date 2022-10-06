using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreDK.Models.Responses;
using MediatR;

namespace BookStoreDK.Models.MediatR.Commands.AuthorCommands
{
    public record GetAllAuthorsCommand : IRequest<AuthorsCollectionResponse>
    {
    }
}
