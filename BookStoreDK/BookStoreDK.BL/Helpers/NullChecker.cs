using System.Net;
using BookStoreDK.Models.Responses;

namespace BookStoreDK.BL.Helpers
{
    public static class NullChecker
    {
        public static TResponseModel CheckForNullObjectAndReturnResponse<TDbEntityModel, TResponseModel>(TDbEntityModel? result, string errorMessage = "")
            where TResponseModel : BaseResponse<TDbEntityModel>, new()
        {
            if (result == null)
            {
                return new TResponseModel()
                {
                    HttpStatusCode = HttpStatusCode.NotFound,
                    Message = errorMessage,
                };
            }

            return new TResponseModel()
            {
                Model = result,
                HttpStatusCode = HttpStatusCode.OK,
            };
        }
    }

}
