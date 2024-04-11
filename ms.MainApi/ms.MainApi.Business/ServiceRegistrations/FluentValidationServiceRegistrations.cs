using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ms.MainApi.Business.Cqrs.Baskets.Validators;
using ms.MainApi.Business.Cqrs.Catalogs.Validators;
using ms.MainApi.Business.Cqrs.Favourites.Validators;
using ms.MainApi.Business.Cqrs.Identities.Permissions.Validators;
using ms.MainApi.Business.Cqrs.Identities.Roles.Validators;
using ms.MainApi.Business.Cqrs.Identities.UserRoles.Validators;
using ms.MainApi.Business.Cqrs.Identities.Users.Validators;
using ms.MainApi.Business.Cqrs.Organizations.Cities.Validators;
using ms.MainApi.Business.Cqrs.Organizations.OrganizationPictures.Validators;
using ms.MainApi.Business.Cqrs.Organizations.Validators;
using ms.MainApi.Business.Cqrs.Products.Brands.Validators;
using ms.MainApi.Business.Cqrs.Products.Materials.Validators;
using ms.MainApi.Business.Cqrs.Products.ProductArticles.Validators;
using ms.MainApi.Business.Cqrs.Products.Validators;
using ms.MainApi.Business.Cqrs.Projects.ProjectCatalogs.Validators;
using ms.MainApi.Business.Cqrs.Projects.Validators;

namespace ms.MainApi.Business.ServiceRegistrations;

public static class FluentValidationServiceRegistrations
{
    public static IServiceCollection AddFluentValidationServices
        (this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        #region BaseValidators
        services.AddFluentValidation(config => { config.AutomaticValidationEnabled = false; });

        #endregion


        #region Identities

        #region Users
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(UserBanCommandValidator)));
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(UserCreateCommandValidator)));
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(UserDeleteCommandValidator)));
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(UserGetCommandValidator)));
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(UserResetPasswordCommandValidator)));
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(UserUpdateCommandValidator)));
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(UserUpdatePasswordCommandValidator)));

        #endregion

        #region Roles
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(RoleCreateCommandValidator)));
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(RoleUpdateCommandValidator)));
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(RoleDeleteCommandValidator)));
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(RoleGetCommandValidator)));

        #endregion

        #region UserRoles
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(UserRoleCreateCommandValidator)));
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(UserRoleDeleteCommandValidator)));
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(UserRoleGetRolesCommandValidator)));
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(UserRoleGetUsersCommandValidator)));

        #endregion

        #region Permissions
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(PermissionCreateCommandValidator)));
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(PermissionDeleteCommandValidator)));
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(PermissionGetByRoleCommandValidator)));

        #endregion

        #endregion


        #region Product Validator
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(ProductCreateCommandValidator)));
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(ProductUpdateCommandValidator)));
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(ProductDeleteCommandValidator)));
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(ProductGetCommandValidator)));


        #region ProductArticle Validator
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(ProductArticleCreateCommandValidator)));
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(ProductArticleUpdateCommandValidator)));
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(ProductArticleDeleteCommandValidator)));
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(ProductArticleGetCommandValidator)));

        #endregion

        #region Brand Validator
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(BrandCreateCommandValidator)));
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(BrandUpdateCommandValidator)));
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(BrandDeleteCommandValidator)));
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(BrandGetCommandValidator)));

        #endregion

        #region Material Validator
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(MaterialCreateCommandValidator)));
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(MaterialUpdateCommandValidator)));
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(MaterialDeleteCommandValidator)));
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(MaterialGetCommandValidator)));

        #endregion

        #region Catalog Validators
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(CatalogCreateCommandValidator)));
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(CatalogDeleteCommandValidator)));
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(CatalogGetCommandValidator)));
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(CatalogUpdateCommandValidator)));

        #endregion

        #endregion


        #region Organization Validator
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(OrganizationCreateCommandValidator)));
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(OrganizationUpdateCommandValidator)));
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(OrganizationDeleteCommandValidator)));
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(OrganizationGetCommandValidator)));

        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(OrganizationBannerCreateCommandValidator)));
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(OrganizationAvatarCreateCommandValidator)));


        #region City Validator
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(CityCreateCommandValidator)));
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(CityUpdateCommandValidator)));
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(CityDeleteCommandValidator)));
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(CityGetCommandValidator)));

        #endregion


        #endregion


        #region ProjectCatalog, Project Validator
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(ProjectCreateCommandValidator)));
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(ProjectUpdateCommandValidator)));
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(ProjectDeleteCommandValidator)));
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(ProjectGetCommandValidator)));


        #region ProjectCatalog Validator
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(ProjectCatalogCreateCommandValidator)));
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(ProjectCatalogUpdateCommandValidator)));
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(ProjectCatalogDeleteCommandValidator)));
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(ProjectCatalogGetCommandValidator)));

        #endregion


        #endregion


        #region Favourites, Baskets
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(FavouriteAddProductCommandValidator)));
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(FavouriteAddProjectCommandValidator)));

        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(BasketAddProductCommandValidator)));
        services.AddFluentValidation((fv) => fv.RegisterValidatorsFromAssemblyContaining(typeof(BasketAddProjectCommandValidator)));


        #endregion





        return services;
    }
}