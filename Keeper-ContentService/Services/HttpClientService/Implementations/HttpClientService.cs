using Keeper_ContentService.Models.Service;
using Keeper_ContentService.Services.HttpClientService.Interfaces;
using System.Text.Json;

namespace Keeper_ContentService.Services.HttpClientService.Implementations
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient _httpClient;

        public HttpClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<ServiceResponse<T?>> GetAsync<T>(string url)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                return await ProcessResponse<T>(response);
            }
            catch (Exception ex)
            {
                return ServiceResponse<T?>.Fail(default, 500, ex.Message);
            }
        }


        public async Task<ServiceResponse<TResponse?>> PostAsync<TRequest, TResponse>(string url, TRequest data)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync(url, data);
                return await ProcessResponse<TResponse>(response);
            }
            catch (Exception ex)
            {
                return ServiceResponse<TResponse?>.Fail(default, 500, ex.Message);
            }
        }


        public async Task<ServiceResponse<TResponse?>> PutAsync<TRequest, TResponse>(string url, TRequest data)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync(url, data);
                return await ProcessResponse<TResponse>(response);
            }
            catch (Exception ex)
            {
                return ServiceResponse<TResponse?>.Fail(default, 500, ex.Message);
            }
        }


        public async Task<ServiceResponse<T?>> DeleteAsync<T>(string url)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync(url);
                return await ProcessResponse<T>(response);
            }
            catch (Exception ex)
            {
                return ServiceResponse<T?>.Fail(default, 500, ex.Message);
            }
        }


        private async Task<ServiceResponse<T?>> ProcessResponse<T>(HttpResponseMessage response)
        {
            string rawJson = await response.Content.ReadAsStringAsync();

            try
            {
                if (!response.IsSuccessStatusCode)
                    return ServiceResponse<T?>.Fail(default, (int)response.StatusCode, rawJson);

                using JsonDocument doc = JsonDocument.Parse(rawJson);
                JsonElement root = doc.RootElement;

                string? message = root.TryGetProperty("message", out var msgProp) ? msgProp.GetString() : null;

                if (root.TryGetProperty("data", out var dataElement))
                {
                    T? data = JsonSerializer.Deserialize<T>(dataElement.GetRawText(), new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    });

                    return ServiceResponse<T?>.Success(data, (int)response.StatusCode, message);
                }

                T? fallbackData = JsonSerializer.Deserialize<T>(rawJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });

                return ServiceResponse<T?>.Success(fallbackData, (int)response.StatusCode, message ?? "Success");
            }
            catch (Exception ex)
            {
                return ServiceResponse<T?>.Fail(default, 500, $"Deserialization error: {ex.Message}");
            }
        }
    }
}
