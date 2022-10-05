using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreDK.Models.Models;

namespace BookStoreDK.Models.Responses
{
    public class PersonCollectionResponse : BaseResponse
    {
        public IEnumerable<Person> People { get; init; }
    }
}
