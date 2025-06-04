using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.Service;
using System.Security.Claims;


namespace Keeper_ContentService.Services.ArticleService.Interfaces
{
    public interface IStatusChangeStrategy
    {
        public Task<bool> CanHandle(string status);
        public Task<ServiceResponse<Article?>> ChangeStatusAsync(Article article, ClaimsPrincipal User);
    }
}
