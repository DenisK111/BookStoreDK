namespace BookStoreDK.Models.Models.Users
{
    public record UserInfo
    {
        public int UserId { get; init; }
        public string DisplayName { get; init; } = null!;
        public string UserName { get; init; } = null!;
        public string Email { get; init; } = null!;
        public string Password { get; set; } = null!;
        public DateTime CreatedDate { get; init; }
    }
}
