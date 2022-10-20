using BookStoreDK.BL.HttpClientProviders.Contracts;
using BookStoreDK.Models.Configurations;
using Microsoft.Extensions.Options;

namespace BookStoreDK.BL.HttpClientProviders.Implementations
{
    public class AdditionalInfoProvider : IAdditionalInfoProvider
    {
        private readonly HttpClient _httpClient;
        private readonly IOptionsMonitor<AdditionalInfoProviderSettings> _settings;
        public AdditionalInfoProvider(HttpClient httpClient, IOptionsMonitor<AdditionalInfoProviderSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings;
        }

        public async Task<string> GetAdditionalInfo(int id)
        {
            var additionalInfoResponse = await _httpClient
               .GetAsync($"{_settings.CurrentValue.BaseUrl}GetByAuthorId?authorId={id}");

            additionalInfoResponse.EnsureSuccessStatusCode();
            var authorInfo = await additionalInfoResponse.Content.ReadAsStringAsync();
            return authorInfo;
        }
    }
}
