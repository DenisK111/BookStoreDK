namespace BookStoreDK.Models.Requests
{
    public class AddPersonRequest
    {
        public string Name { get; init; } = null!;

        public int Age { get; init; }

        public DateTime DateOfBirth { get; init; }
    }
}
