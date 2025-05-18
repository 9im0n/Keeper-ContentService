using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.DTO;

namespace Keeper_ContentService.Services.DTOMapperService.Interfaces
{
    public interface IDTOMapperService
    {
        // Single
        public CategoryDTO Map(Category category);
        public ArticleStatusDTO Map(ArticleStatus status);
        public CommentDTO Map(Comment comment);
        public ArticleDTO Map(Article article, bool isLiked, bool isSaved, ProfileDTO profileDTO);
        public LikedArticleDTO Map(LikedArticle likedArticle);
        public SavedArticleDTO Map(SavedArticle savedArticle);

        // Collection
        public ICollection<CategoryDTO> Map(ICollection<Category> categories);
        public ICollection<ArticleStatusDTO> Map(ICollection<ArticleStatus> articleStatuses);
        public ICollection<CommentDTO> Map(ICollection<Comment> comments);
        public ICollection<ArticleDTO> Map(ICollection<Article> articles, ICollection<LikedArticle>? likedArticles,
            ICollection<SavedArticle>? savedArticles, ICollection<ProfileDTO> profileDTOs);
        public ICollection<LikedArticleDTO> Map(ICollection<LikedArticle> likedArticles);
        public ICollection<SavedArticleDTO> Map(ICollection<SavedArticle> savedArticles);
    }
}
