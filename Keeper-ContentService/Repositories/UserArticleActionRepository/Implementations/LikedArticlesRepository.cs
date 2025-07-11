﻿using Keeper_ContentService.DB;
using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Repositories.BaseRepository.Implementations;
using Keeper_ContentService.Repositories.UserArticleActionRepository.Interfaces;
using Keeper_ContentService.Services.DTOMapperService.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Keeper_ContentService.Repositories.UserArticleActionRepository.Implementations
{
    public class LikedArticlesRepository : 
        BaseRepository<LikedArticle>, 
        ILikedArticlesRepository
    {
        public LikedArticlesRepository(AppDbContext context) : base(context) { }


        public async Task<PagedResultDTO<LikedArticle>>
            GetPagedAsync(PagedRequestDTO<LikedArticlesFillterDTO> request)
        {
            IQueryable<LikedArticle> query = _appDbContext.Likes;

            query = query.Include(l => l.Article).ThenInclude(a => a.Status)
                .Include(l => l.Article).ThenInclude(a => a.Category);

            if (request.Filter?.UserId != null)
                query = query.Where(l => l.UserId == request.Filter.UserId);

            int totalCount = await query.CountAsync();

            bool isDescending = request.Direction.ToLower() == "desc";

            query = request.Sort.ToLower() switch
            {
                "id" => isDescending ? query.OrderByDescending(l => l.Id) : query.OrderBy(l => l.Id),
                "createdat" => isDescending ? query.OrderByDescending(l => l.CreatedAt) : query.OrderBy(l => l.CreatedAt),
                _ => isDescending ? query.OrderByDescending(l => l.Id) : query.OrderBy(l => l.Id)
            };

            query = query.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize);

            List<LikedArticle> likedArticles = await query.ToListAsync();

            return new PagedResultDTO<LikedArticle>()
            {
                Items = likedArticles,
                TotalCount = totalCount
            };
        }


        public async Task<LikedArticle?> GetByUserAndArticleIdAsync(Guid userId, Guid articleId)
        {
            return await _appDbContext.Likes.FirstOrDefaultAsync(l => l.UserId == userId && l.ArticleId == articleId);
        }

        public async Task<ICollection<LikedArticle>> GetBatchedByUserAndArticleId(ICollection<Guid> articleId, Guid userId)
        {
            return await _appDbContext.Likes.Where(l => articleId.Contains(l.ArticleId) && l.UserId == userId).ToListAsync();
        }
    }
}
