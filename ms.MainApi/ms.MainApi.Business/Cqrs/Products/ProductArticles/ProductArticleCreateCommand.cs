﻿using AutoMapper;
using FluentValidation;
using MediatR;
using ms.MainApi.Business.Services;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Products;
using ms.MainApi.Entity.Models.DbModels.Products;
using ms.MainApi.Entity.Models.Dtos.Products.ProductArticles;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using System.Net;

namespace ms.MainApi.Business.Cqrs.Products.ProductArticles;

public class ProductArticleCreateCommand : IRequest<IMainResponseDto>
{
    public ProductArticleCreateDto form { get; }

    public ProductArticleCreateCommand(ProductArticleCreateDto _form)
    {
        form = _form;
    }

    public class Handler : IRequestHandler<ProductArticleCreateCommand, IMainResponseDto>
    {
        #region DI
        private readonly IMediator _mediator;
        private readonly IMessagesRepository _messagesRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<ProductArticleCreateCommand> _validator;
        private readonly IProductArticleDal _entityDal;
        private readonly IPermissionCheck _checkPermission;
        

        public Handler(IMediator mediator, IMessagesRepository messagesRepository, IMapper mapper, IValidator<ProductArticleCreateCommand> validator, 
            IProductArticleDal entityDal, IPermissionCheck checkPermission)
        {
            _mediator = mediator;
            _messagesRepository = messagesRepository;
            _mapper = mapper;
            _validator = validator;
            _entityDal = entityDal;
            _checkPermission = checkPermission;
            
        }
        #endregion

        public async Task<IMainResponseDto> Handle(ProductArticleCreateCommand request, CancellationToken cancellationToken)
        {
            #region Validation
            var validation = await _validator.ValidateAsync(request);
            if (!validation.IsValid)
                return new MainResponseDto(_messagesRepository.FormValidation(),
                    validation.Errors.Select(e => e.ErrorMessage).ToList(), HttpStatusCode.BadRequest);
            #endregion

            #region checkPermission
            List<PermissionAction> actions = new List<PermissionAction>() {
                PermissionAction.create
            };
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.productSettings, actions);

            if (!permission.isSuccess)
                return new MainResponseDto(permission.message, HttpStatusCode.Forbidden);
            #endregion

            ProductArticle entity = _mapper.Map<ProductArticle>(request.form);
            await _entityDal.AddAsync(entity);

            ProductArticleDto? entityDto = await _mediator.Send(new getProductArticleCommand(entity.id));

            return new MainResponseDto(entityDto, permission.permittedActions);
        }
    }
}