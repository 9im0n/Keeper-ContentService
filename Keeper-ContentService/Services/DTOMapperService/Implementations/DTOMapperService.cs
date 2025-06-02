using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Services.DTOMapperService.Interfaces;

namespace Keeper_ContentService.Services.DTOMapperService.Implementations
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

        public CommentDTO Map(Comment comment, ProfileDTO profileDTO)
        {
            return new CommentDTO()
            {
                Id = comment.Id,
                ArticleId = comment.ArticleId,
                Author = profileDTO,
                ParentCommentId = comment.ParentCommentId,
                Text = comment.Text,
            };
        }

        public ArticleDTO Map(Article article, bool isLiked, bool isSaved, ProfileDTO profileDTO)
        {
            return new ArticleDTO()
            {
                Id = article.Id,
                Category = Map(article.Category),
                Author = profileDTO,
                Status = Map(article.Status),
                Title = article.Title,
                Content = article.Content,
                PublicationDate = article.PublicationDate,
                CreatedAt = article.CreatedAt,
                UpdatedAt = article.UpdatedAt,
                IsLiked = isLiked,
                IsSaved = isSaved,
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
        
        public ICollection<CommentDTO> Map(ICollection<Comment> comments, ICollection<ProfileDTO> profileDTOs)
        {
            Dictionary<Guid, ProfileDTO> profilesMap = profileDTOs.ToDictionary(p => p.Id);

            List<CommentDTO> commentDTOs = new List<CommentDTO>();

            foreach (Comment comment in comments)
            {
                commentDTOs.Add(Map(comment, profilesMap[comment.AuthorId]));
            }

            return commentDTOs;
        }


        public ICollection<ArticleDTO> Map(
            ICollection<Article> articles,
            ICollection<LikedArticle>? likedArticles,
            ICollection<SavedArticle>? savedArticles,
            ICollection<ProfileDTO> profileDTOs)
        {
            HashSet<Guid> likedIds = likedArticles?.Select(l => l.ArticleId).ToHashSet() ?? new HashSet<Guid>();
            HashSet<Guid> savedIds = savedArticles?.Select(s => s.ArticleId).ToHashSet() ?? new HashSet<Guid>();
            Dictionary<Guid, ProfileDTO> profilesMap = profileDTOs.ToDictionary(p => p.Id);

            List<ArticleDTO> articleDTOs = new List<ArticleDTO>();

            foreach (Article article in articles)
            {
                articleDTOs.Add(Map(
                    article,
                    likedIds.Contains(article.Id),
                    savedIds.Contains(article.Id),
                    profilesMap[article.AuthorId]
                ));
            }

            return articleDTOs;
        }


        public ICollection<LikedArticleDTO> Map(ICollection<LikedArticle> likedArticles) => likedArticles.Select(Map).ToList();
        
        public ICollection<SavedArticleDTO> Map(ICollection<SavedArticle> savedArticles) => savedArticles.Select(Map).ToList();
    }
}
