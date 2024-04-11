using AutoMapper;
using MediatR;
using ms.MainApi.DataAccess.Products;
using ms.MainApi.DataAccess.Projects;
using ms.MainApi.Entity.Models.DbModels.Products;
using ms.MainApi.Entity.Models.DbModels.Projects;
using ms.MainApi.Entity.Models.Dtos.Pictures;
using ms.MainApi.Entity.Models.Dtos.Products;
using ms.MainApi.Entity.Models.Dtos.Projects.ProjectCatalogs;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Pages.MainPages;

namespace ms.MainApi.Business.Cqrs.MainPages;

public class MainPageGetListCommand : IRequest<IMainResponseDto>
{
    public class Handler : IRequestHandler<MainPageGetListCommand, IMainResponseDto>
    {
        #region DI
        private readonly IMapper _mapper;
        private readonly IProjectCatalogDal _catalogDal;
        private readonly IProjectDal _projectDal;
        private readonly IProjectPictureDal _projectPictureDal;
        private readonly IProductDal _productDal;
        private readonly IProductPictureDal _productPictureDal;

        public Handler(IMapper mapper, IProjectCatalogDal catalogDal, IProjectDal projectDal, IProjectPictureDal projectPictureDal, 
            IProductDal productDal, IProductPictureDal productPictureDal)
        {
            _mapper = mapper;
            _catalogDal = catalogDal;
            _projectDal = projectDal;
            _projectPictureDal = projectPictureDal;
            _productDal = productDal;
            _productPictureDal = productPictureDal;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(MainPageGetListCommand request, CancellationToken cancellationToken)
        {
            MainPage result = new MainPage
            {
                projectCatalogs = new List<ProjectCatalogCollectDto>(),
                products = new List<ProductShortDto>()
            };

            List<ProjectCatalog>? projectCatalogs = await _catalogDal.GetAllAsync();
            List<int> projectCatalogsId = projectCatalogs.Select(i => i.id).ToList();
            List<Project> projects = await _projectDal.GetAllAsync(i => projectCatalogsId.Contains(i.projectCatalogId));

            foreach (ProjectCatalog projectCatalog in projectCatalogs)
            {
                List<Project> CatalogProjects = projects.Where(i => i.projectCatalogId == projectCatalog.id).ToList();
                if (CatalogProjects == null || CatalogProjects.Count == 0)
                    continue;

                ProjectCatalogCollectDto? projectCatalogDto = _mapper.Map<ProjectCatalogCollectDto>(projectCatalog);
                //projectCatalogDto.amount = await _projectDal.CountAsync(i => i.projectCatalogId == projectCatalog.id);

                projectCatalogDto.amount = CatalogProjects.Count();

                int projectId = CatalogProjects.OrderByDescending(o => o.id).FirstOrDefault()!.id;
                ProjectPicture? sourcePictures = await _projectPictureDal.GetAsync(i => i.isMain && i.projectId == projectId);
                projectCatalogDto.picture = _mapper.Map<PictureDto>(sourcePictures);

                result.projectCatalogs.Add(projectCatalogDto);
            }

            #region products with main pictures
            int page = 0, pageSize = 7;
            List<Product> products = await _productDal.GetAllPaginationAsync(page, pageSize);
            result.products = products.Select(i => _mapper.Map<ProductShortDto>(i)).ToList();

            List<int> productsId = products.Select(i => i.id).ToList();
            List<ProductPicture> pictures = await _productPictureDal.GetAllAsync(i => productsId.Contains(i.productId) && i.isMain);

            foreach (ProductShortDto product in result.products)
            {
                ProductPicture? picture = pictures.FirstOrDefault(i => i.productId == product.id);
                product.picture = _mapper.Map<PictureDto>(picture);
            }
            #endregion

            return new MainResponseDto(result);
        }
    }
}