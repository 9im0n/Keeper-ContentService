using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;
using Keeper_ContentService.Repositories.Interfaces;
using Keeper_ContentService.Services.Interfaces;
using System.Security.Claims;

namespace Keeper_ContentService.Services.Implementations
{
    public class CommentsService : ICommentsService
    {
        private readonly ICommentsRepository _repository;
        private readonly IArticleService _articleService;
        private readonly IDTOMapperService _mapper;

        public CommentsService(
            ICommentsRepository repository, 
            IArticleService articleService,
            IDTOMapperService mapper)
        {
            _repository = repository;
            _articleService = articleService;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<PagedResultDTO<CommentDTO>?>> GetPagedAsync(
            Guid articleId, 
            PagedRequestDTO<CommentsFilterDTO> pagedRequestDTO)
        {
            ServiceResponse<ArticleDTO?> serviceResponse = await _articleService.GetByIdAsync(articleId);

            if (!serviceResponse.IsSuccess)
                return ServiceResponse<PagedResultDTO<CommentDTO>?>.Fail(default, serviceResponse.Status, serviceResponse.Message);

            PagedResultDTO <CommentDTO> commentDTOs = await _repository.GetPagedCommentsAsync(articleId, pagedRequestDTO);
            return ServiceResponse<PagedResultDTO<CommentDTO>?>.Success(commentDTOs);
        }


        public async Task<ServiceResponse<CommentDTO?>> CreateAsync(
            Guid articleId, 
            CreateCommentDTO createCommentDTO, 
            ClaimsPrincipal User)
        {
            if (!Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
                return ServiceResponse<CommentDTO?>.Fail(default, 401, "User Unauthorized.");

            ServiceResponse<ArticleDTO?> articleServiceResponse = await _articleService.GetByIdAsync(articleId);

            if (!articleServiceResponse.IsSuccess)
                return ServiceResponse<CommentDTO?>.Fail(default, 
                    articleServiceResponse.Status, articleServiceResponse.Message);

            Comment? newComment = new Comment()
            {
                Text = createCommentDTO.Text,
                AuthorId = userId,
                ArticleId = articleId,
                ParentCommentId = createCommentDTO.ParentCommentId
            };

            newComment = await _repository.CreateAsync(newComment);

            CommentDTO commentDTO = _mapper.Map(newComment);

            return ServiceResponse<CommentDTO?>.Success(commentDTO);
        }


        public async Task<ServiceResponse<object?>> DeleteAsync(Guid articleId, Guid commentId, ClaimsPrincipal User)
        {
            if (!Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
                return ServiceResponse<object?>.Fail(default, 401, "User Unauthorized.");

            ServiceResponse<ArticleDTO?> articleServiceResponse = await _articleService.GetByIdAsync(articleId);

            if (!articleServiceResponse.IsSuccess)
                return ServiceResponse<object?>.Fail(default,
                    articleServiceResponse.Status, articleServiceResponse.Message);

            Comment? comment = await _repository.GetByIdAsync(articleId);

            if (comment == null)
                return ServiceResponse<object?>.Fail(default, 404, "Comment doesn't exist");

            if (comment.AuthorId != userId)
                return ServiceResponse<object?>.Fail(default, 403, "You don't have permmision to delete this comment.");
            
            await _repository.DeleteAsync(commentId);

            return ServiceResponse<object?>.Success(default);
        }
    }
}
