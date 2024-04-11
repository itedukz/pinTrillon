using FluentValidation;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Identities;

namespace ms.MainApi.Business.Cqrs.Identities.UserRoles.Validators;

public class UserRoleCreateCommandValidator : AbstractValidator<UserRoleCreateCommand>
{
    private readonly IMessagesRepository _messagesRepository;
    private readonly IRoleDal _roleDal;
    private readonly IUserDal _userDal;

    public UserRoleCreateCommandValidator(IMessagesRepository messagesRepository, IRoleDal roleDal, IUserDal userDal)
    {
        _messagesRepository = messagesRepository;
        _roleDal = roleDal;
        _userDal = userDal;

        RuleFor(v => v.form.userId)
            .GreaterThan(0)
            .WithMessage(_messagesRepository.NotEqual("userId", "0"))
            .MustAsync(async (ctx, context, token) =>
                await IsExistUser(ctx.form.userId, token))
            .WithMessage(_messagesRepository.NotFound("User"));

        RuleFor(v => v.form.rolesId)
           .MustAsync(async (ctx, context, token) =>
               await IsExistRols(ctx.form.rolesId, token))
           .WithMessage(_messagesRepository.NotFound("Rols"));

    }

    private async Task<bool> IsExistUser(int userId, CancellationToken token)
        => await _userDal.AnyAsync(i => i.id == userId);


    private async Task<bool> IsExistRols(List<int>? rolsId, CancellationToken token)
    {
        if (rolsId != null && rolsId.Count > 0)
        {
            if (rolsId.Count == 1 && rolsId[0] == 0)
                return true;

            bool isRoleNotFound = true;
            foreach (int roleId in rolsId)
            {
                if (!await _roleDal.AnyAsync(i => i.id == roleId))
                {
                    isRoleNotFound = false;
                    break;
                }
            }

            return isRoleNotFound;
        }

        return true;
    }
}