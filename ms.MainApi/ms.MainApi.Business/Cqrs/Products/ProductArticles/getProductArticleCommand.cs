using AutoMapper;
using FluentValidation;
using MediatR;
using ms.MainApi.Business.Services;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess;
using ms.MainApi.DataAccess.Products;
using ms.MainApi.Entity.Models.DbModels.Products;
using ms.MainApi.Entity.Models.Dtos.Catalogs;
using ms.MainApi.Entity.Models.Dtos.Products.ProductArticles;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using System.Net;

namespace ms.MainApi.Business.Cqrs.Products.ProductArticles;

public class getProductArticleCommand : IRequest<ProductArticleDto>
{
    public int id { get; }

    public getProductArticleCommand(int _id)
    {
        id = _id;
    }

    public class Handler : IRequestHandler<getProductArticleCommand, ProductArticleDto>
    {
        #region DI
        private readonly IMapper _mapper;
        private readonly IProductArticleDal _entityDal;
        private readonly ICatalogDal _catalogDal;

        public Handler(IMapper mapper, IProductArticleDal entityDal, ICatalogDal catalogDal)
        {
            _mapper = mapper;
            _entityDal = entityDal;
            _catalogDal = catalogDal;
        }
        #endregion

        public async Task<ProductArticleDto?> Handle(getProductArticleCommand request, CancellationToken cancellationToken)
        {
            ProductArticle? entity = await _entityDal.GetAsync(i => i.id == request.id);
            if (entity == null)
                return null;

            ProductArticleDto entityDto = _mapper.Map<ProductArticleDto>(entity);
            entityDto.catalog = _mapper.Map<CatalogDto>(await _catalogDal.GetAsync(i => i.id == entity.catalogId));

            return entityDto;
        }
    }
}