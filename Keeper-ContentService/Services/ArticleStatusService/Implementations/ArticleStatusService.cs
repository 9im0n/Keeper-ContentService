﻿using Keeper_ContentService.Models.Db;
using Keeper_ContentService.Models.DTO;
using Keeper_ContentService.Models.Service;
using Keeper_ContentService.Repositories.ArticleStatusRepository.Interfaces;
using Keeper_ContentService.Services.ArticleStatusService.Interfaces;
using Keeper_ContentService.Services.DTOMapperService.Interfaces;

namespace Keeper_ContentService.Services.ArticleStatusService.Implementations
{
    public class ArticleStatusService : IArticlesStatusesService
    {
        private readonly IArticleStatusesRepository _repository;
        private readonly IDTOMapperService _mapper;

        public ArticleStatusService(IArticleStatusesRepository repository,
            IDTOMapperService mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public async Task<ServiceResponse<ICollection<ArticleStatusDTO>>> GetAllAsync()
        {
            ICollection<ArticleStatus> articleStatuses = await _repository.GetAllAsync();
            ICollection<ArticleStatusDTO> articleStatusDTOs = _mapper.Map(articleStatuses);
            return ServiceResponse<ICollection<ArticleStatusDTO>>.Success(articleStatusDTOs);
        }


        public async Task<ServiceResponse<ArticleStatusDTO?>> GetByIdAsync(Guid Id)
        {
            ArticleStatus? statuse = await _repository.GetByIdAsync(Id);

            if (statuse == null)
                return ServiceResponse<ArticleStatusDTO?>.Fail(default, 404, "Statuse doesn't exist");

            ArticleStatusDTO articleStatusDTO = _mapper.Map(statuse);

            return ServiceResponse<ArticleStatusDTO?>.Success(articleStatusDTO);
        }


        public async Task<ServiceResponse<ArticleStatusDTO?>> GetReviewStatusAsync()
        {
            ArticleStatus? statuse = await _repository.GetByNameAsync("review");

            if (statuse == null)
                return ServiceResponse<ArticleStatusDTO?>.Fail(default, 404, "Statuse doesn't exist");

            ArticleStatusDTO articleStatusDTO = _mapper.Map(statuse);

            return ServiceResponse<ArticleStatusDTO?>.Success(articleStatusDTO);
        }


        public async Task<ServiceResponse<ArticleStatusDTO?>> GetDraftStatusAsync()
        {
            ArticleStatus? statuse = await _repository.GetByNameAsync("draft");

            if (statuse == null)
                return ServiceResponse<ArticleStatusDTO?>.Fail(default, 404, "Statuse doesn't exist");

            ArticleStatusDTO articleStatusDTO = _mapper.Map(statuse);

            return ServiceResponse<ArticleStatusDTO?>.Success(articleStatusDTO);
        }


        public async Task<ServiceResponse<ArticleStatusDTO?>> GetPublishedStatusAsync()
        {
            ArticleStatus? statuse = await _repository.GetByNameAsync("published");

            if (statuse == null)
                return ServiceResponse<ArticleStatusDTO?>.Fail(default, 404, "Statuse doesn't exist");

            ArticleStatusDTO articleStatusDTO = _mapper.Map(statuse);

            return ServiceResponse<ArticleStatusDTO?>.Success(articleStatusDTO);
        }


        public async Task<ServiceResponse<ArticleStatusDTO?>> GetReadyToPublishStatusAsync()
        {
            ArticleStatus? statuse = await _repository.GetByNameAsync("readyToPublish");

            if (statuse == null)
                return ServiceResponse<ArticleStatusDTO?>.Fail(default, 404, "Statuse doesn't exist");

            ArticleStatusDTO articleStatusDTO = _mapper.Map(statuse);

            return ServiceResponse<ArticleStatusDTO?>.Success(articleStatusDTO);
        }
    }
}
