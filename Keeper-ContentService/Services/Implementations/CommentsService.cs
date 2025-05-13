using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;
using Keeper_ContentService.Repositories.Interfaces;
using Keeper_ContentService.Services.Interfaces;

namespace Keeper_ContentService.Services.Implementations
{
    public class CommentsService : ICommentsService
    {
        private readonly ICommentsRepository _repository;

        public CommentsService(ICommentsRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResponse<PagedResultDTO<CommentDTO>>> GetPagedAsync(Guid articleId, PagedRequestDTO<CommentsFilterDTO> pagedRequestDTO)
        {
            PagedResultDTO<CommentDTO> commentDTOs = await _repository.GetPagedCommentsAsync(articleId, pagedRequestDTO);
            return ServiceResponse<PagedResultDTO<CommentDTO>>.Success(commentDTOs);
        }
    }
}
