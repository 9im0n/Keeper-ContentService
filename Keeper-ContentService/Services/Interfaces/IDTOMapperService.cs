using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.DTO;

namespace Keeper_ContentService.Services.Interfaces
{
    public interface IDTOMapperService
    {
        // Single
        public CategoryDTO Map(Category category);
        public ArticleStatusDTO Map(ArticleStatus status);
        public CommentDTO Map(Comment comment);
        public ArticleDTO Map(Article article);
        public LikedArticleDTO Map(LikedArticle likedArticle);
        public SavedArticleDTO Map(SavedArticle savedArticle);

        // Collection
        public ICollection<CategoryDTO> Map(ICollection<Category> categories);
        public ICollection<ArticleStatusDTO> Map(ICollection<ArticleStatus> articleStatuses);
        public ICollection<CommentDTO> Map(ICollection<Comment> comments);
        public ICollection<ArticleDTO> Map(ICollection<Article> articles);
        public ICollection<LikedArticleDTO> Map(ICollection<LikedArticle> likedArticles);
        public ICollection<SavedArticleDTO> Map(ICollection<SavedArticle> savedArticles);
    }
}
