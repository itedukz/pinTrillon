using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ms.MainApi.DataAccess;
using ms.MainApi.DataAccess.Baskets;
using ms.MainApi.DataAccess.Favourites;
using ms.MainApi.DataAccess.Identities;
using ms.MainApi.DataAccess.Organizations;
using ms.MainApi.DataAccess.Products;
using ms.MainApi.DataAccess.Projects;
using ms.MainApi.Entity.Contexts;

namespace ms.MainApi.Business.ServiceRegistrations;

public static class DalServiceRegistrations
{
    public static IServiceCollection AddDalServices
        (this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        #region DbContexts
        services.AddDbContext<dbContext>(options =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString("designDb"),
                builder => builder.MigrationsAssembly("ms.MainApi")
            );
        });
        #endregion

        
        #region Identity
        services.AddScoped<IUserDal, UserDal>();
        services.AddScoped<IRoleDal, RoleDal>();
        services.AddScoped<IUserRoleDal, UserRoleDal>();
        services.AddScoped<IPermissionDal, PermissionDal>();

        #endregion


        #region Catalogs, Products, Pictures, ProductPictures, Brands, Materials
        services.AddScoped<ICatalogDal, CatalogDal>();
        services.AddScoped<IProductDal, ProductDal>();
        services.AddScoped<IProductArticleDal, ProductArticleDal>();
        services.AddScoped<IPictureDal, PictureDal>();
        services.AddScoped<IProductPictureDal, ProductPictureDal>();

        services.AddScoped<IBrandDal, BrandDal>();
        services.AddScoped<IMaterialDal, MaterialDal>();

        #endregion


        #region Organizations 
        services.AddScoped<ICityDal, CityDal>();
        services.AddScoped<IOrganizationDal, OrganizationDal>();
        services.AddScoped<IOrganizationPictureDal, OrganizationPictureDal>();

        #endregion


        #region Projects 
        services.AddScoped<IProjectCatalogDal, ProjectCatalogDal>();
        services.AddScoped<IProjectDal, ProjectDal>();
        services.AddScoped<IProjectPictureDal, ProjectPictureDal>();
        services.AddScoped<IProjectLayoutDal, ProjectLayoutDal>();

        #endregion


        #region Favourites, Baskets
       //services.AddScoped<IFavouriteDal, FavouriteDal>();
        services.AddScoped<IFavouriteProductDal, FavouriteProductDal>();
        services.AddScoped<IFavouriteProjectDal, FavouriteProjectDal>();

        services.AddScoped<IBasketProductDal, BasketProductDal>();
        services.AddScoped<IBasketProjectDal, BasketProjectDal>();
        
        #endregion




        return services;
    }
}