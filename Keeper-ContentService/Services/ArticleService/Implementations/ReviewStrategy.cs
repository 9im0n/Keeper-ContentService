using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;
using Keeper_ContentService.Services.ArticleService.Interfaces;
using Keeper_ContentService.Services.ArticleStatusService.Interfaces;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;

namespace Keeper_ContentService.Services.ArticleService.Implementations
{
    public class ReviewStrategy : IStatusChangeStrategy
    {
        private readonly IArticleStatusNamedProvider _articleStatusService;

        public ReviewStrategy(IArticleStatusNamedProvider articleStatusService)
        {
            _articleStatusService = articleStatusService;
        }


        public async Task<bool> CanHandle(string status)
        {
            ServiceResponse<ArticleStatusDTO?> reviewStatus = await _articleStatusService.GetReviewStatusAsync();

            if (!reviewStatus.IsSuccess) 
                return false;

            return status == reviewStatus.Data!.Name;
        }


        public async Task<ServiceResponse<Article?>> ChangeStatusAsync(Article article, ClaimsPrincipal User)
        {
            string? userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (!Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
                return ServiceResponse<Article?>.Fail(default, 401, "user unauthorized.");

            if (userRole != "admin" && article.AuthorId != userId)
                return ServiceResponse<Article?>.Fail(default, 403, "You don't have required permissions.");

            ServiceResponse<ArticleStatusDTO?> draftStatus = await _articleStatusService.GetDraftStatusAsync();

            if (!draftStatus.IsSuccess)
                return ServiceResponse<Article?>.Fail(default, draftStatus.Status, draftStatus.Message);

            if (article.ArticleStatusId != draftStatus.Data!.Id && userRole != "admin")
                return ServiceResponse<Article?>.Fail(default, 400, "Only drafts can be sent to review.");

            ServiceResponse<ArticleStatusDTO?> reviewStatus = await _articleStatusService.GetReviewStatusAsync();

            if (!reviewStatus.IsSuccess)
                return ServiceResponse<Article?>.Success(default, reviewStatus.Status, reviewStatus.Message);

            article.ArticleStatusId = reviewStatus.Data!.Id;
            article.UpdatedAt = DateTime.UtcNow;

            return ServiceResponse<Article?>.Success(article);
        }
    }
}
