namespace BookStoreDK.Models.Requests
{
    public class AddAuthorRequest
    {
        public string Name { get; init; } = null!;

        public int Age { get; init; }

        public DateTime DateOfBirth { get; init; }

        public string? NickName { get; init; }
    }
}
