namespace BookStoreDK.Models.Models
{
    public record Person : BaseIdModel<int>
    {
        public string Name { get; init; } = null!;

        public int Age { get; init; }

        public DateTime DateOfBirth { get; init; }
    }
}
