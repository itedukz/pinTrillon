using ms.MainApi.Entity.Models.Dtos;

namespace ms.MainApi.Entity.Models.Enums;

public enum PermissionAction
{
    getOwn = 1,   //Читать только свои созданные сущности/записи, относительно Permission раздела
    getAll = 2,   //Читать все сущности/записи, относительно Permission раздела
    create = 3,   //Создать сущности, относительно Permission раздела
    update = 4,   //Редактировать сущности, относительно Permission раздела
    delete = 5,   //Удалить сущности, относительно Permission раздела
}

public static class PermissionActionMethod
{
    public static List<entityPermissionDtlList> entityPermissions
    {
        get
        {
            List<entityPermissionDtlList> list = new List<entityPermissionDtlList>();
            list.Add(new entityPermissionDtlList { id = (int)PermissionName.user, name = PermissionName.user.ToString(), permissions = Crud });
            list.Add(new entityPermissionDtlList { id = (int)PermissionName.role, name = PermissionName.role.ToString(), permissions = Crud });
            list.Add(new entityPermissionDtlList { id = (int)PermissionName.userRoleBind, name = PermissionName.userRoleBind.ToString(), permissions = Crud });
            list.Add(new entityPermissionDtlList { id = (int)PermissionName.permission, name = PermissionName.permission.ToString(), permissions = Crud });
            
            list.Add(new entityPermissionDtlList { id = (int)PermissionName.catalog, name = PermissionName.catalog.ToString(), permissions = Crud });
            list.Add(new entityPermissionDtlList { id = (int)PermissionName.productSettings, name = PermissionName.productSettings.ToString(), permissions = Crud });
            list.Add(new entityPermissionDtlList { id = (int)PermissionName.product, name = PermissionName.product.ToString(), permissions = CrudOwn });
            
            list.Add(new entityPermissionDtlList { id = (int)PermissionName.projectCatalog, name = PermissionName.projectCatalog.ToString(), permissions = Crud });
            list.Add(new entityPermissionDtlList { id = (int)PermissionName.project, name = PermissionName.project.ToString(), permissions = CrudOwn });
            
            list.Add(new entityPermissionDtlList { id = (int)PermissionName.organization, name = PermissionName.organization.ToString(), permissions = CrudOwn });
            list.Add(new entityPermissionDtlList { id = (int)PermissionName.city, name = PermissionName.city.ToString(), permissions = Crud });

            return list;
        }
    }

    public static List<entityPermissionDtl> Crud
    {
        get
        {
            List<entityPermissionDtl> list = new List<entityPermissionDtl>();
            list.Add(new entityPermissionDtl { id = (int)PermissionAction.getAll, name = PermissionAction.getAll.ToString(), column = 0, group = "get" });
            list.Add(new entityPermissionDtl { id = (int)PermissionAction.create, name = PermissionAction.create.ToString(), column = 0, group = "create" });
            list.Add(new entityPermissionDtl { id = (int)PermissionAction.update, name = PermissionAction.update.ToString(), column = 0, group = "update" });
            list.Add(new entityPermissionDtl { id = (int)PermissionAction.delete, name = PermissionAction.delete.ToString(), column = 0, group = "delete" });

            return list;
        }
    }

    public static List<entityPermissionDtl> CrudOwn
    {
        get
        {
            List<entityPermissionDtl> list = new List<entityPermissionDtl>();
            list.Add(new entityPermissionDtl { id = (int)PermissionAction.getAll, name = PermissionAction.getAll.ToString(), column = 0, group = "get" });
            list.Add(new entityPermissionDtl { id = (int)PermissionAction.getOwn, name = PermissionAction.getOwn.ToString(), column = 1, group = "get" });
            list.Add(new entityPermissionDtl { id = (int)PermissionAction.create, name = PermissionAction.create.ToString(), column = 0, group = "create" });
            list.Add(new entityPermissionDtl { id = (int)PermissionAction.update, name = PermissionAction.update.ToString(), column = 0, group = "update" });
            list.Add(new entityPermissionDtl { id = (int)PermissionAction.delete, name = PermissionAction.delete.ToString(), column = 0, group = "delete" });

            return list;
        }
    }

    public static PermissionAction? toEnum(int actionId)
    {
        return actionId switch
        {
            (int)PermissionAction.getOwn => PermissionAction.getOwn,
            (int)PermissionAction.getAll => PermissionAction.getAll,
            (int)PermissionAction.create => PermissionAction.create,
            (int)PermissionAction.update => PermissionAction.update,
            (int)PermissionAction.delete => PermissionAction.delete,
            _ => null
        };
    }
}