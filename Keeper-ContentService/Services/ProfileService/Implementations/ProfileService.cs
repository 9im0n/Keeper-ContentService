using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;
using Keeper_ContentService.Services.HttpClientService.Interfaces;
using Keeper_ContentService.Services.ProfileService.Interfaces;
using Microsoft.Extensions.Options;

namespace Keeper_ContentService.Services.ProfileService.Implementations
{
    public class ProfileService : IProfileService
    {
        private readonly IHttpClientService _httpClient;
        private readonly ServiceUrlsDTO _urls;

        public ProfileService(IHttpClientService httpClient,
            IOptions<ServiceUrlsDTO> urls,
            ILogger<ProfileService> logger)
        {
            _httpClient = httpClient;
            _urls = urls.Value;
        }


        public async Task<ServiceResponse<ProfileDTO?>> GetProfileByIdAsync(Guid Id)
        {
            return await _httpClient.GetAsync<ProfileDTO>($"{_urls.UserService}/profiles/{Id}");
        }


        public async Task<ServiceResponse<ICollection<ProfileDTO>?>> GetProfilesBatchAsync(BatchedProfileRequestDTO Ids)
        {
            return await _httpClient.PostAsync<BatchedProfileRequestDTO, ICollection<ProfileDTO>>($"{_urls.UserService}/profiles/batch", Ids);
        }
    }
}
