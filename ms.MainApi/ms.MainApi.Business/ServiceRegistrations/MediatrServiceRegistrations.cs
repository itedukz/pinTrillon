using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ms.MainApi.Business.Cqrs.Baskets;
using ms.MainApi.Business.Cqrs.Catalogs;
using ms.MainApi.Business.Cqrs.Favourites;
using ms.MainApi.Business.Cqrs.Identities;
using ms.MainApi.Business.Cqrs.Identities.Permissions;
using ms.MainApi.Business.Cqrs.Identities.Roles;
using ms.MainApi.Business.Cqrs.Identities.UserRoles;
using ms.MainApi.Business.Cqrs.Identities.Users;
using ms.MainApi.Business.Cqrs.MainPages;
using ms.MainApi.Business.Cqrs.Organizations;
using ms.MainApi.Business.Cqrs.Organizations.Cities;
using ms.MainApi.Business.Cqrs.Organizations.OrganizationPictures;
using ms.MainApi.Business.Cqrs.Products;
using ms.MainApi.Business.Cqrs.Products.Brands;
using ms.MainApi.Business.Cqrs.Products.Colors;
using ms.MainApi.Business.Cqrs.Products.Materials;
using ms.MainApi.Business.Cqrs.Products.ProductArticles;
using ms.MainApi.Business.Cqrs.Products.ProductPictures;
using ms.MainApi.Business.Cqrs.Projects;
using ms.MainApi.Business.Cqrs.Projects.ProjectCatalogs;
using ms.MainApi.Business.Cqrs.Projects.ProjectPictures;

namespace ms.MainApi.Business.ServiceRegistrations;

public static class MediatrServiceRegistrations
{
    public static IServiceCollection AddMediatrServices
        (this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        #region Identities

        #region Users
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(UserGetCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(UserGetListCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(UserGetCurrentCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(UserCreateCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(UserUpdateCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(UserDeleteCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(UserBanCommand.Handler).Assembly));

        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(UserUpdatePasswordCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(UserResetPasswordCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(UserAvatarUploadCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(UserAvatarDeleteCommand.Handler).Assembly));

        #endregion

        #region Roles
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(RoleGetCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(RoleGetListCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(RoleCreateCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(RoleUpdateCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(RoleDeleteCommand.Handler).Assembly));

        #endregion

        #region UserRoles
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(UserRoleCreateCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(UserRoleDeleteCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(UserRoleGetRolesCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(UserRoleGetUsersCommand.Handler).Assembly));

        #endregion

        #region RolePermissions
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(PermissionCreateCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(PermissionDeleteCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(PermissionGetByRoleCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(PermissionGetNameListCommand.Handler).Assembly));

        #endregion
        
        #region Authorizations
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(AuthorizeLoginCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(AuthorizeLogoutCommand.Handler).Assembly));

        #endregion

        #endregion


        #region Catalogs
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(CatalogGetCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(CatalogGetListCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(CatalogCreateCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(CatalogUpdateCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(CatalogDeleteCommand.Handler).Assembly));

        #endregion


        #region Products
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(ProductGetCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(ProductGetListCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(ProductCreateCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(ProductUpdateCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(ProductDeleteCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(ProductPictureCreateCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(ProductPictureSetMainCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(ProductPictureDeleteCommand.Handler).Assembly));

        #region Brands
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(BrandGetCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(BrandGetListCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(BrandCreateCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(BrandUpdateCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(BrandDeleteCommand.Handler).Assembly));

        #endregion

        #region Materials
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(MaterialGetCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(MaterialGetListCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(MaterialCreateCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(MaterialUpdateCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(MaterialDeleteCommand.Handler).Assembly));

        #endregion

        #region Colors
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(ColorGetListEnumCommand.Handler).Assembly));

        #endregion

        #region ProductArticles
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(ProductArticleGetCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(ProductArticleGetListCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(ProductArticleGetListByCatalogIdCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(ProductArticleCreateCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(ProductArticleUpdateCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(ProductArticleDeleteCommand.Handler).Assembly));

        #endregion

        #endregion


        #region Organizations
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(getOrganizationCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(OrganizationGetCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(OrganizationGetListCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(OrganizationCreateCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(OrganizationUpdateCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(OrganizationDeleteCommand.Handler).Assembly));


        #region Cities
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(CityGetCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(CityGetListCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(CityCreateCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(CityUpdateCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(CityDeleteCommand.Handler).Assembly));

        #endregion

        #region OrganizationPictures
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(OrganizationBannerCreateCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(OrganizationBannerDeleteCommand.Handler).Assembly));
        
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(OrganizationAvatarCreateCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(OrganizationAvatarDeleteCommand.Handler).Assembly));

        #endregion


        #endregion


        #region Projects
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(getProjectCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(ProjectGetCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(ProjectGetListCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(ProjectCreateCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(ProjectUpdateCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(ProjectDeleteCommand.Handler).Assembly));

        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(ProjectPictureCreateCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(ProjectPictureSetMainCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(ProjectPictureDeleteCommand.Handler).Assembly));
        
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(ProjectLayoutCreateCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(ProjectLayoutDeleteCommand.Handler).Assembly));


        #region ProjectCatalogs
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(ProjectCatalogGetCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(ProjectCatalogGetListCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(ProjectCatalogCreateCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(ProjectCatalogUpdateCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(ProjectCatalogDeleteCommand.Handler).Assembly));

        #endregion

        #endregion


        #region Favourite (Product, Project)
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(FavouriteAddProductCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(FavouriteAddProjectCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(FavouriteRemoveProductCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(FavouriteRemoveProjectCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(FavouriteProductGetListCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(FavouriteProjectGetListCommand.Handler).Assembly));


        #endregion


        #region Basket (Product, Project)
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(BasketAddProductCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(BasketAddProjectCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(BasketRemoveProductCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(BasketRemoveProjectCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(BasketProductGetListCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(BasketProjectGetListCommand.Handler).Assembly));


        #endregion


        #region MainPages
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(MainPageGetListCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(SearchPageProductGetListCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(SearchPageProjectGetListCommand.Handler).Assembly));
        services.AddMediatR(cnf => cnf.RegisterServicesFromAssembly(typeof(SortEnumGetListCommand.Handler).Assembly));
        
        #endregion




        return services;
    }
}