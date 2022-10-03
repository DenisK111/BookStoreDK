namespace BookStoreDK.Models.Requests
{
    public class UpdateBookRequest : AddBookRequest 
    {
        public int Id { get; init; }
    }
}
