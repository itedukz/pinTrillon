using MediatR;
using ms.MainApi.Entity.Models.Dtos.Identities.PermissionServices;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;

namespace ms.MainApi.Business.Cqrs.Measures;

public class MeasureGetListEnumCommand : IRequest<IMainResponseDto>
{
    public class Handler : IRequestHandler<MeasureGetListEnumCommand, IMainResponseDto>
    {
        public async Task<IMainResponseDto> Handle(MeasureGetListEnumCommand request, CancellationToken cancellationToken)
            => new MainResponseDto(MeasureEnumMethod.list, new PermissionList());
    }
}
