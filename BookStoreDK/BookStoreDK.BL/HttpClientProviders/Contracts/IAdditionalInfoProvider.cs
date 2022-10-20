namespace BookStoreDK.BL.HttpClientProviders.Contracts
{
    public interface IAdditionalInfoProvider
    {
        Task<string> GetAdditionalInfo(int id);
    }
}
