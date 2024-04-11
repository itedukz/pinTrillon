using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ms.MainApi.Entity.Models.DbModels;
using ms.MainApi.Entity.Models.DbModels.Catalogs;
using ms.MainApi.Entity.Models.DbModels.Identity;
using ms.MainApi.Entity.Models.DbModels.Organizations;
using ms.MainApi.Entity.Models.DbModels.Products;
using ms.MainApi.Entity.Models.DbModels.Projects;
using ms.MainApi.Entity.Models.Dtos.Baskets;
using ms.MainApi.Entity.Models.Dtos.Catalogs;
using ms.MainApi.Entity.Models.Dtos.Favourites;
using ms.MainApi.Entity.Models.Dtos.Identities.Permissions;
using ms.MainApi.Entity.Models.Dtos.Identities.Roles;
using ms.MainApi.Entity.Models.Dtos.Identities.UserRoles;
using ms.MainApi.Entity.Models.Dtos.Identities.Users;
using ms.MainApi.Entity.Models.Dtos.Organizations;
using ms.MainApi.Entity.Models.Dtos.Organizations.Cities;
using ms.MainApi.Entity.Models.Dtos.Pictures;
using ms.MainApi.Entity.Models.Dtos.Products.Brands;
using ms.MainApi.Entity.Models.Dtos.Products.Materials;
using ms.MainApi.Entity.Models.Dtos.Products.ProductArticles;
using ms.MainApi.Entity.Models.Dtos.Projects;
using ms.MainApi.Entity.Models.Dtos.Projects.ProjectCatalogs;
using ms.MainApi.Entity.Models.Profiles.Baskets;
using ms.MainApi.Entity.Models.Profiles.Favourites;
using ms.MainApi.Entity.Models.Profiles.Products;
using ms.MainApi.Entity.Models.Profiles.Projects;

namespace ms.MainApi.Business.ServiceRegistrations;

public static class AutoMapServiceRegistrations
{
    public static IServiceCollection AddAutoMapServices
        (this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        services.AddSingleton(new MapperConfiguration(config =>
        {
            #region Identity
            config.CreateMap<UserDto, User>().ReverseMap();
            config.CreateMap<UserWithRolesDto, User>().ReverseMap();
            config.CreateMap<UserCreateDto, User>().ReverseMap();
            config.CreateMap<UserUpdateDto, User>().ReverseMap();

            config.CreateMap<RoleDto, Role>().ReverseMap();
            config.CreateMap<RoleShortDto, Role>().ReverseMap();
            config.CreateMap<RoleCreateDto, Role>().ReverseMap();
            config.CreateMap<RoleUpdateDto, Role>().ReverseMap();

            config.CreateMap<PermissionDto, Permission>().ReverseMap();

            config.CreateMap<UserRoleDto, UserRole>().ReverseMap();
            config.CreateMap<UserRolesDto, UserRole>().ReverseMap();
            config.CreateMap<UserRoleCreateDto, UserRole>().ReverseMap();

            #endregion


            #region Catalogs, Products, Brands, Materials, ProductArticles
            config.CreateMap<CatalogDto, Catalog>().ReverseMap();
            config.CreateMap<CatalogCreateDto, Catalog>().ReverseMap();

            config.AddProfile<ProductCreateProfile>();
            config.AddProfile<ProductUpdateProfile>();
            config.AddProfile<ProductDtoProfile>();
            config.AddProfile<ProductShortDtoProfile>();
            
            config.CreateMap<BrandDto, Brand>().ReverseMap();
            config.CreateMap<BrandCreateDto, Brand>().ReverseMap();
            
            config.CreateMap<MaterialDto, Material>().ReverseMap();
            config.CreateMap<MaterialCreateDto, Material>().ReverseMap();

            config.CreateMap<ProductArticleDto, ProductArticle>().ReverseMap();
            config.CreateMap<ProductArticleCreateDto, ProductArticle>().ReverseMap();
            config.CreateMap<ProductArticleUpdateDto, ProductArticle>().ReverseMap();

            config.CreateMap<PictureDto, ProductPicture>().ReverseMap();
            

            #endregion


            #region Organizations, Cities
            config.CreateMap<OrganizationDto, Organization>().ReverseMap();
            config.CreateMap<OrganizationCreateDto, Organization>().ReverseMap();
            config.CreateMap<OrganizationUpdateDto, Organization>().ReverseMap();

            config.CreateMap<CityDto, City>().ReverseMap();
            config.CreateMap<CityCreateDto, City>().ReverseMap();
            config.CreateMap<CityUpdateDto, City>().ReverseMap();

            config.CreateMap<PictureDto, OrganizationPicture>().ReverseMap();

            #endregion


            #region ProjectCatalogs, Projects
            config.CreateMap<ProjectCatalogDto, ProjectCatalog>().ReverseMap();
            config.CreateMap<ProjectCatalogCreateDto, ProjectCatalog>().ReverseMap();
            config.CreateMap<ProjectCatalogUpdateDto, ProjectCatalog>().ReverseMap();
            config.CreateMap<ProjectCatalog, ProjectCatalogCollectDto>();

            
            config.AddProfile<ProjectCreateProfile>();
            config.AddProfile<ProjectUpdateProfile>();
            config.AddProfile<ProjectDtoProfile>();
            config.AddProfile<ProjectShortDtoProfile>();

            config.CreateMap<PictureDto, ProjectPicture>().ReverseMap();
            config.CreateMap<ProjectLayout, ProjectLayoutDto>();

            #endregion


            #region Baskets, Favourites
            //config.AddProfile<BasketDtoProfile>();            
            //config.AddProfile<FavouriteDtoProfile>();

            #endregion






        }).CreateMapper());
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        return services;
    }
}