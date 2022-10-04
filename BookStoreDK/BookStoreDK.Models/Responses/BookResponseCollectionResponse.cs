using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreDK.Models.Models;

namespace BookStoreDK.Models.Responses
{
    public class BookResponseCollectionResponse : BaseResponse
    {
         public IEnumerable<Book> Books { get; init; }
    }
}
