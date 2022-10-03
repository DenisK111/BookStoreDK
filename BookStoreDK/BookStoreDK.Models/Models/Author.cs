namespace BookStoreDK.Models.Models
{
    public record Author : BaseIdModel<int>
    {
        public string Name { get; init; } = null!;

        public int Age { get; init; }

        public DateTime DateOfBirth { get; init; }

        public string NickName { get; init; } = null!;
    }

}
