namespace BookStoreDK.Models.Models
{
    public record Author : Person
    {
       
        public string NickName { get; init; } = null!;
    }

}
