using Keeper_ContentService.Models.Service;

namespace Keeper_ContentService.Services.HttpClientService.Interfaces
{
    public interface IHttpClientService
    {
        Task<ServiceResponse<T?>> GetAsync<T>(string url);
        Task<ServiceResponse<TResponse?>> PostAsync<TRequest, TResponse>(string url, TRequest data);
        Task<ServiceResponse<TResponse?>> PutAsync<TRequest, TResponse>(string url, TRequest data);
        Task<ServiceResponse<T?>> DeleteAsync<T>(string url);
    }
}
