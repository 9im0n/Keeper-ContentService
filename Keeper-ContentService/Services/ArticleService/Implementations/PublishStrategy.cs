using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;
using Keeper_ContentService.Services.ArticleService.Interfaces;
using Keeper_ContentService.Services.ArticleStatusService.Interfaces;
using System.Security.Claims;

namespace Keeper_ContentService.Services.ArticleService.Implementations
{
    public class PublishStrategy : IStatusChangeStrategy
    {
        private readonly IArticleStatusNamedProvider _articleStatusService;

        public PublishStrategy(IArticleStatusNamedProvider articleStatusService)
        {
            _articleStatusService = articleStatusService;
        }

        public async Task<bool> CanHandle(string status)
        {
            ServiceResponse<ArticleStatusDTO?> statusResponse = await _articleStatusService.GetPublishedStatusAsync();
            
            if (!statusResponse.IsSuccess)
                return false;

            return status == statusResponse.Data!.Name;
        }

        public async Task<ServiceResponse<Article?>> ChangeStatusAsync(Article article, ClaimsPrincipal User)
        {
            string? userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (!Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
                return ServiceResponse<Article?>.Fail(default, 401, "user unauthorized.");

            if (userRole != "admin" && article.AuthorId != userId)
                return ServiceResponse<Article?>.Fail(default, 403, "You don't have required permissions.");

            ServiceResponse<ArticleStatusDTO?> readyForPublishStatus = await _articleStatusService.GetReadyToPublishStatusAsync();

            if (!readyForPublishStatus.IsSuccess)
                return ServiceResponse<Article?>.Fail(default, readyForPublishStatus.Status, readyForPublishStatus.Message);

            if (article.ArticleStatusId != readyForPublishStatus.Data!.Id && userRole != "admin")
                return ServiceResponse<Article?>.Fail(default, 400, "You cannot publish article, that doesn't ready for publication.");
            
            ServiceResponse<ArticleStatusDTO?> publishedStatus = await _articleStatusService.GetPublishedStatusAsync();

            if (!publishedStatus.IsSuccess)
                return ServiceResponse<Article?>.Fail(default, publishedStatus.Status, publishedStatus.Message);

            article.ArticleStatusId = publishedStatus.Data!.Id;
            article.UpdatedAt = DateTime.UtcNow;
            article.PublicationDate = DateTime.UtcNow;

            return ServiceResponse<Article?>.Success(article);
        }
    }
}
