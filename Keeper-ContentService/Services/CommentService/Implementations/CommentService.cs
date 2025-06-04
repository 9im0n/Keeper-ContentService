using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;
using Keeper_ContentService.Repositories.CommentRepository.Interfaces;
using Keeper_ContentService.Services.CommentService.Interfaces;
using Keeper_ContentService.Services.ArticleService.Interfaces;
using System.Security.Claims;
using Keeper_ContentService.Services.DTOMapperService.Interfaces;
using Keeper_ContentService.Services.ProfileService.Interfaces;

namespace Keeper_ContentService.Services.CommentService.Implementations
{
    public class CommentService : ICommentService
    {
        private readonly ICommentsRepository _repository;
        private readonly IArticleService _articleService;
        private readonly IProfileService _profileService;
        private readonly IDTOMapperService _mapper;

        public CommentService(
            ICommentsRepository repository,
            IArticleService articleService,
            IProfileService profileService,
            IDTOMapperService mapper)
        {
            _repository = repository;
            _articleService = articleService;
            _profileService = profileService;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<PagedResultDTO<CommentDTO>?>> GetPagedAsync(
            Guid articleId,
            PagedRequestDTO<CommentsFilterDTO> pagedRequestDTO)
        {
            ServiceResponse<ArticleDTO?> serviceResponse = await _articleService.GetByIdAsync(articleId);

            if (!serviceResponse.IsSuccess)
                return ServiceResponse<PagedResultDTO<CommentDTO>?>.Fail(default, serviceResponse.Status, serviceResponse.Message);

            PagedResultDTO<Comment> comments = await _repository.GetPagedCommentsAsync(articleId, pagedRequestDTO);

            BatchedProfileRequestDTO profileRequest = new BatchedProfileRequestDTO()
            {
                profileIds = comments.Items.Select(c => c.AuthorId).Distinct().ToList()
            };

            ServiceResponse<ICollection<ProfileDTO>?> profilesResponse = await _profileService
                .GetProfilesBatchAsync(profileRequest);

            if (!profilesResponse.IsSuccess)
                return ServiceResponse<PagedResultDTO<CommentDTO>?>.Fail(default, profilesResponse.Status, profilesResponse.Message);

            ICollection<CommentDTO> commentDTOs = _mapper.Map(comments.Items, profilesResponse.Data!);

            PagedResultDTO<CommentDTO> result = new PagedResultDTO<CommentDTO>()
            {
                Items = commentDTOs.ToList(),
                TotalCount = comments.TotalCount,
            };

            return ServiceResponse<PagedResultDTO<CommentDTO>?>.Success(result);
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

            ServiceResponse<ProfileDTO?> profileResponse = await _profileService.GetProfileByIdAsync(userId);

            if (!profileResponse.IsSuccess)
                return ServiceResponse<CommentDTO?>.Fail(default, profileResponse.Status, profileResponse.Message);

            CommentDTO commentDTO = _mapper.Map(newComment, profileResponse.Data!);

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

            Comment? comment = await _repository.GetByIdAsync(commentId);

            if (comment == null)
                return ServiceResponse<object?>.Fail(default, 404, "Comment doesn't exist");

            if (comment.AuthorId != userId)
                return ServiceResponse<object?>.Fail(default, 403, "You don't have permmision to delete this comment.");

            await _repository.DeleteAsync(commentId);

            return ServiceResponse<object?>.Success(default);
        }
    }
}
