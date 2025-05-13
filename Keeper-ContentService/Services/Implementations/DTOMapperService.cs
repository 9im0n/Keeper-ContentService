using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Services.Interfaces;

namespace Keeper_ContentService.Services.Implementations
{
    public class DTOMapperService : IDTOMapperService
    {
        public CategoryDTO Map(Category category)
        {
            return new CategoryDTO()
            {
                Id = category.Id,
                Name = category.Name,
            };
        }

        public ArticleStatusDTO Map(ArticleStatus status)
        {
            return new ArticleStatusDTO()
            {
                Id = status.Id,
                Name = status.Name,
            };
        }

        public CommentDTO Map(Comment comment)
        {
            return new CommentDTO()
            {
                Id = comment.Id,
                ArticleId = comment.ArticleId,
                AuthorId = comment.AuthorId,
                ParentCommentId = comment.ParentCommentId,
                Text = comment.Text,
            };
        }

        public ArticleDTO Map(Article article)
        {
            return new ArticleDTO()
            {
                Id = article.Id,
                Category = Map(article.Category),
                AuthorId = article.AuthorId,
                Satus = Map(article.Status),
                Title = article.Title,
                Content = article.Content,
                PublicationDate = article.PublicationDate,
                CreatedAt = article.CreatedAt,
                UpdatedAt = article.UpdatedAt
            };
        }

        public LikedArticleDTO Map(LikedArticle likedArticle)
        {
            return new LikedArticleDTO()
            {
                Id = likedArticle.Id,
                UserId = likedArticle.UserId,
                ArticleId = likedArticle.ArticleId
            };
        }

        public SavedArticleDTO Map(SavedArticle savedArticle)
        {
            return new SavedArticleDTO()
            {
                Id = savedArticle.Id,
                UserId = savedArticle.UserId,
                ArticleId = savedArticle.ArticleId
            };
        }

        public ICollection<CategoryDTO> Map(ICollection<Category> categories) => categories.Select(Map).ToList();
        public ICollection<ArticleStatusDTO> Map(ICollection<ArticleStatus> articleStatuses) => articleStatuses.Select(Map).ToList();
        public ICollection<CommentDTO> Map(ICollection<Comment> comments) => comments.Select(Map).ToList();
        public ICollection<ArticleDTO> Map(ICollection<Article> articles) => articles.Select(Map).ToList();
        public ICollection<LikedArticleDTO> Map(ICollection<LikedArticle> likedArticles) => likedArticles.Select(Map).ToList();
        public ICollection<SavedArticleDTO> Map(ICollection<SavedArticle> savedArticles) => savedArticles.Select(Map).ToList();
    }
}
