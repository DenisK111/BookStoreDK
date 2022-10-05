using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreDK.Models.Models;

namespace BookStoreDK.Models.Responses
{
    public class AuthorResponse : BaseResponse
    {
        public Author? Author { get; init; }
    }
}
