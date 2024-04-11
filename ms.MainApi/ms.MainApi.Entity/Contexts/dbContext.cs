using Microsoft.EntityFrameworkCore;
using ms.MainApi.Entity.Models.DbModels;
using ms.MainApi.Entity.Models.DbModels.Baskets;
using ms.MainApi.Entity.Models.DbModels.Catalogs;
using ms.MainApi.Entity.Models.DbModels.Favourites;
using ms.MainApi.Entity.Models.DbModels.Identity;
using ms.MainApi.Entity.Models.DbModels.Organizations;
using ms.MainApi.Entity.Models.DbModels.Products;
using ms.MainApi.Entity.Models.DbModels.Projects;
using ms.MainApi.Entity.Models.Services;

namespace ms.MainApi.Entity.Contexts;

public class dbContext : DbContext
{
    #region Identities
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Permission> Permissions { get; set; }

    #endregion

    
    public DbSet<Picture> Pictures { get; set; }


    #region Products, Catalogs, ProductPictures, Brands, Materials
    public DbSet<Product> Products { get; set; }
    public DbSet<Catalog> Catalogs { get; set; }
    public DbSet<ProductPicture> ProductPictures { get; set; }

    public DbSet<Brand> Brands { get; set; }
    public DbSet<Material> Materials { get; set; }
    public DbSet<ProductArticle> ProductArticles { get; set; }

    #endregion


    #region Organizations, Cities
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<OrganizationPicture> OrganizationPictures { get; set; }
    public DbSet<City> Cities { get; set; }

    #endregion


    #region Projects, ProjectCatalogs
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectCatalog> ProjectCatalogs { get; set; }
    public DbSet<ProjectPicture> ProjectPictures { get; set; }
    public DbSet<ProjectLayout> ProjectLayouts { get; set; }

    #endregion


    #region Favourites, Baskets
    public DbSet<FavouriteProduct> FavouriteProducts { get; set; }
    public DbSet<FavouriteProject> FavouriteProjects { get; set; }

    public DbSet<BasketProduct> BasketProducts { get; set; }
    public DbSet<BasketProject> BasketProjects { get; set; }

    #endregion


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<User>().HasData(
        //    new User
        //    {
        //        id = 1,
        //        email = "itedu.kz@gmail.com",
        //        passwordHash = HashMD5.HashMD5String("123"),

        //        firstName = "Алмат",
        //        lastName = "Тажіхан ұлы",
        //        phoneNumber = "87472203400",
        //        regDate = DateTime.Now,
        //        isActive = true
        //    },
        //    new User
        //    {
        //        id = 2,
        //        email = "pin@gmail.com",
        //        passwordHash = HashMD5.HashMD5String("123"),

        //        firstName = "Айнұр",
        //        lastName = "Бердикул",
        //        phoneNumber = "8747",
        //        regDate = DateTime.Now,
        //        isActive = true
        //    });

        //modelBuilder.Entity<Role>().HasData(
        //    new Role
        //    {
        //        id = 1,
        //        name = "Admin",
        //        description = "Admin users role",
        //    });

        ///modelBuilder.Entity<Permission>().Property(e => e.roleId)
        //modelBuilder.Entity<Permission>().HasData(
        //    new Permission
        //    {
        //        id = 1,
        //        roleId = 1,
        //        permissionId = (int)PermissionName.authorization,
        //        actions = new List<int>() { (int)PermissionAction.create, (int)PermissionAction.update, (int)PermissionAction.delete, (int)PermissionAction.get },
        //        createdBy = 1
        //    });
        
        //modelBuilder.Entity<UserRole>().HasData(
        //    new UserRole
        //    {
        //        id = 1,
        //        userId = 1,
        //        roleId = 1
        //    },
        //    new UserRole
        //    {
        //        id = 1,
        //        userId = 2,
        //        roleId = 1
        //    });


        base.OnModelCreating(modelBuilder);
    }


    public dbContext() { }

    public dbContext(DbContextOptions<dbContext> options) : base(options) { }
}
