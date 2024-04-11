using ms.MainApi.Entity.Models.Dtos;

namespace ms.MainApi.Entity.Models.Enums;

public enum PermissionName
{
    user = 1,           //Раздел работы с пользователями. Доступные операции: create, update, delete, get-читать
    role = 2,           //Раздел работы с ролями в системе. Доступные операции: create, update, delete, get-читать
    userRoleBind = 3,   //Раздел связывания роли с пользователями. Доступные операции: create, update, delete, get-читать
    permission = 4,     //Раздел назначения полномочии указанным ролям. Доступные операции: create, update, delete, get-читать

    catalog = 5,        //Раздел работы с категориями товаров. Доступные операции: create, update, delete, get-читать
    product = 6,        //Раздел работы с товарами. Доступные операции: create, update, delete, get-читать
    productSettings = 7,//Color, Brend, Model

    /// <summary>
    /// Дизайн и проект это одно и то же
    /// </summary>
    projectCatalog = 8, //Раздел работы с каталогами проект-дизайнов. Доступные операции: create, update, delete, get-читать
    project = 9,        //Раздел работы с проектами. Доступные операции: create, update, delete, get-читать

    organization = 10,  //Раздел работы с поставщиками. Доступные операции: create, update, delete, get-читать
    city = 11,  //Раздел работы с поставщиками. Доступные операции: create, update, delete, get-читать
    
    //designer = 11,     //Раздел работы с дизайнерами. Доступные операции: create, update, delete, get-читать
}

public static class PermissionConvert
{
    public static string toString(int permissionNameId)
    {
        return permissionNameId switch
        {
            (int)PermissionName.user => PermissionName.user.ToString(),
            (int)PermissionName.role => PermissionName.role.ToString(),
            (int)PermissionName.userRoleBind => PermissionName.userRoleBind.ToString(),
            (int)PermissionName.permission => PermissionName.permission.ToString(),

            (int)PermissionName.catalog => PermissionName.catalog.ToString(),
            (int)PermissionName.product => PermissionName.product.ToString(),
            (int)PermissionName.productSettings => PermissionName.productSettings.ToString(),

            (int)PermissionName.projectCatalog => PermissionName.projectCatalog.ToString(),
            (int)PermissionName.project => PermissionName.project.ToString(),

            (int)PermissionName.organization => PermissionName.organization.ToString(),
            (int)PermissionName.city => PermissionName.city.ToString(),
            //(int)PermissionName.designer => PermissionName.designer.ToString(),
            _ => ""
        };
    }

    public static EnumItemDto toBaseClass(int permissionNameId)
    {
        return permissionNameId switch
        {
            (int)PermissionName.user => new EnumItemDto() { id = (int)PermissionName.user, name = PermissionName.user.ToString() } ,
            (int)PermissionName.role => new EnumItemDto() { id = (int)PermissionName.role, name = PermissionName.role.ToString() } ,
            (int)PermissionName.userRoleBind => new EnumItemDto() { id = (int)PermissionName.userRoleBind, name = PermissionName.userRoleBind.ToString() },
            (int)PermissionName.permission => new EnumItemDto() { id = (int)PermissionName.permission, name = PermissionName.permission.ToString() },

            (int)PermissionName.catalog => new EnumItemDto() { id = (int)PermissionName.catalog, name = PermissionName.catalog.ToString() },
            (int)PermissionName.product => new EnumItemDto() { id = (int)PermissionName.product, name = PermissionName.product.ToString() } ,
            (int)PermissionName.productSettings => new EnumItemDto() { id = (int)PermissionName.productSettings, name = PermissionName.productSettings.ToString() } ,

            (int)PermissionName.projectCatalog => new EnumItemDto() { id = (int)PermissionName.projectCatalog, name = PermissionName.projectCatalog.ToString() },
            (int)PermissionName.project => new EnumItemDto() { id = (int)PermissionName.project, name = PermissionName.project.ToString() },
            (int)PermissionName.organization => new EnumItemDto() { id = (int)PermissionName.organization, name = PermissionName.organization.ToString() },
            (int)PermissionName.city => new EnumItemDto() { id = (int)PermissionName.city, name = PermissionName.city.ToString() },
            
            _ => new EnumItemDto() { id = 0, name = "" }
        };
    }
}