using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;
using Keeper_ContentService.Services.ArticleService.Interfaces;
using Keeper_ContentService.Services.ArticleStatusService.Implementations;
using Keeper_ContentService.Services.ArticleStatusService.Interfaces;
using System.Security.Claims;

namespace Keeper_ContentService.Services.ArticleService.Implementations
{
    public class ReadyToPublishStrategy : IStatusChangeStrategy
    {
        private readonly IArticleStatusNamedProvider _articleStatusService;

        public ReadyToPublishStrategy(IArticleStatusNamedProvider articleStatusService)
        {
            _articleStatusService = articleStatusService;
        }


        public async Task<bool> CanHandle(string status)
        {
            ServiceResponse<ArticleStatusDTO?> readyForPublishStatus = await _articleStatusService.GetReadyToPublishStatusAsync();

            if (!readyForPublishStatus.IsSuccess)
                return false;

            return status == readyForPublishStatus.Data!.Name;
        }


        public async Task<ServiceResponse<Article?>> ChangeStatusAsync(Article article, ClaimsPrincipal User)
        {
            string? userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (!Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
                return ServiceResponse<Article?>.Fail(default, 401, "user unauthorized.");

            if (userRole == "user")
                return ServiceResponse<Article?>.Fail(default, 403, "You don't have required permissions.");

            ServiceResponse<ArticleStatusDTO?> reviewStatus = await _articleStatusService.GetReviewStatusAsync();

            if (!reviewStatus.IsSuccess)
                return ServiceResponse<Article?>.Fail(default, reviewStatus.Status, reviewStatus.Message);

            if (article.ArticleStatusId != reviewStatus.Data!.Id && userRole != "admin")
                return ServiceResponse<Article?>.Fail(default, 400, "Only reviewed articles can be allowed to publication.");

            ServiceResponse<ArticleStatusDTO?> readyForPublishStatus = await _articleStatusService.GetReadyToPublishStatusAsync();

            if (!readyForPublishStatus.IsSuccess)
                return ServiceResponse<Article?>.Success(default, readyForPublishStatus.Status, readyForPublishStatus.Message);

            article.ArticleStatusId = readyForPublishStatus.Data!.Id;
            article.UpdatedAt = DateTime.UtcNow;

            return ServiceResponse<Article?>.Success(article);
        }
    }
}
