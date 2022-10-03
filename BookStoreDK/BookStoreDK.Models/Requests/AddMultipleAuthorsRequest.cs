using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreDK.Models.Requests
{
    public class AddMultipleAuthorsRequest
    {
        public IEnumerable<AddAuthorRequest> AuthorRequests { get; set; }

        public string Reason { get; set; }
    }
}
