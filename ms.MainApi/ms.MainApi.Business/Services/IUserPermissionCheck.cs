using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Identities;
using ms.MainApi.Entity.Models.DbModels.Identity;
using ms.MainApi.Entity.Models.Dtos;
using ms.MainApi.Entity.Models.Enums;
using ms.MainApi.Entity.Models.Services;

namespace ms.MainApi.Business.Services;

public interface IPermissionCheck
{
    public Task<PermissionResponse> CheckPermission(PermissionName permission, List<PermissionAction> actions);
    public Task<List<EnumItemDto>> GetCurrentUserPermissions();
    public Task<PermissionResponse> CheckPermissionWithResponse(PermissionName permission, List<PermissionAction>? actions = null);
}

public class PermissionCheck : IPermissionCheck
{
    #region DI
    private readonly IAuthInformationRepository _authInformationRepository;
    private readonly IUserDal _userDal;
    private readonly IUserRoleDal _userRoleDal;
    private readonly IPermissionDal _permissionDal;

    public PermissionCheck(IAuthInformationRepository authInformationRepository, IUserDal userDal, IUserRoleDal userRoleDal, IPermissionDal permissionDal)
    {
        _authInformationRepository = authInformationRepository;
        _userDal = userDal;
        _userRoleDal = userRoleDal;
        _permissionDal = permissionDal;
    }
    #endregion

    public async Task<PermissionResponse> CheckPermission(PermissionName permission, List<PermissionAction> actions)
    {
        if ((actions == null && actions?.Count == 0) || !Enum.IsDefined(typeof(PermissionName), permission))
            return new PermissionResponse(false, "Request body is empty");

        var currentUser = await _userDal.GetAsync(i => i.id == _authInformationRepository.GetUserId());
        if (currentUser == null)
            return new PermissionResponse(false, "User not found");

        var userRoles = await _userRoleDal.GetAllAsync(i => i.userId == currentUser.id);
        if (!userRoles.Any())
            return new PermissionResponse(false, "User role not found");

        var permissions = await _permissionDal.GetAllAsync(i => userRoles.Select(j => j.roleId).Distinct().ToList().Contains(i.roleId)
            && i.permissionId == (int)permission);

        if (!permissions.Any())
            return new PermissionResponse(false, "Role Permission not found");

        List<int>? Actions = new List<int>();
        foreach (Permission item in permissions)
            if (item.actions != null)
                Actions.AddRange(item.actions);

        List<int> intPermissionActions = new List<int>();
        foreach (PermissionAction item in actions)
            intPermissionActions.Add((int)item);

        if (Actions == null || !Actions.Intersect(intPermissionActions).Any())
            return new PermissionResponse(false, "Action not found in permission");

        return new PermissionResponse(true, "Authorized");
    }

    public async Task<PermissionResponse> CheckPermissionWithResponse(PermissionName permission,
        List<PermissionAction>? actions = null)
    {
        try
        {
            if (!Enum.IsDefined(typeof(PermissionName), permission))
                return new PermissionResponse(false, "Permission request not found");

            var userRoles = await _userRoleDal.GetAllAsync(i => i.userId == _authInformationRepository.GetUserId());
            if (!userRoles.Any())
                return new PermissionResponse(false, "User role not found");

            var permissions = await _permissionDal.GetAllAsync(i => i.permissionId == (int)permission &&
                                                                    userRoles.Select(j => j.roleId).Distinct().ToList().Contains(i.roleId));

            return permissionResponse(permissions, actions);
        }
        catch (Exception ex)
        {
            return new PermissionResponse(false, ex.Message);
        }
    }

    private PermissionResponse permissionResponse(List<Permission>? permissions, List<PermissionAction>? actions = null)
    {
        if (permissions == null || !permissions.Any())
            return new PermissionResponse(false, "Role Permission not found");

        List<int>? Actions = new List<int>(); //permissions.Select(i => i.actions).Cast<int>().Distinct().ToList();
        foreach(List<int>? item in permissions.Select(i => i.actions))
        {
            if(item != null && item.Count > 0)
                Actions.AddRange(item);
        }
        Actions = Actions.Distinct().ToList();

        PermissionResponse resul = new PermissionResponse();                
        foreach (int action in Actions)
        {
            switch (action)
            {
                case (int)PermissionAction.create: resul.permittedActions.canCreate = true; break;
                case (int)PermissionAction.delete: resul.permittedActions.canDelete = true; break;
                case (int)PermissionAction.update: resul.permittedActions.canUpdate = true; break;
                case (int)PermissionAction.getAll: resul.canGetAll = true; break;
                case (int)PermissionAction.getOwn: resul.canGetOwn = true; break;
            }
        }

        List<int> intUserActions = actions?.Cast<int>().ToList() ?? new List<int>();

        if (Actions == null || !Actions.Intersect(intUserActions).Any())
        {
            resul.isSuccess = false;
            resul.message = "User Permission not found";
        }
        else
        {
            resul.isSuccess = true;
            resul.message = "Ok";
        }

        return resul;
    }

    public async Task<List<EnumItemDto>> GetCurrentUserPermissions()
    {        
        List<Permission>? permissions = await getUserRolePermissions();

        if (permissions == null || !permissions.Any())
            return new List<EnumItemDto>();

        List<EnumItemDto> currentUserRolePermissions = new List<EnumItemDto>();
        List<int> permissionsId = permissions.Select(i => i.permissionId).Distinct().ToList();

        foreach (int permissionId in permissionsId.OrderBy(o => o))
        {
            currentUserRolePermissions.Add(new EnumItemDto
            {
                id = permissionId,
                name = PermissionConvert.toString(permissionId)
            });
        }

        return currentUserRolePermissions;
    }

    private async Task<List<Permission>?> getUserRolePermissions()
    {
        List<EnumItemDto> currentUserRolePermissions = new List<EnumItemDto>();

        var currentUser = await _userDal.GetAsync(i => i.id == _authInformationRepository.GetUserId());
        if (currentUser == null)
            return null;

        var userRoles = await _userRoleDal.GetAllAsync(i => i.userId == currentUser.id);
        if (!userRoles.Any())
            return null;

        return await _permissionDal.GetAllAsync(i => userRoles.Select(j => j.roleId).Distinct().ToList().Contains(i.roleId));
    }
}