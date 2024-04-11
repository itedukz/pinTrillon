using MediatR;
using ms.MainApi.Entity.Models.Dtos;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;

namespace ms.MainApi.Business.Cqrs.MainPages;

public class SortEnumGetListCommand : IRequest<IMainResponseDto>
{
    public class Handler : IRequestHandler<SortEnumGetListCommand, IMainResponseDto>
    {
        public async Task<IMainResponseDto> Handle(SortEnumGetListCommand request, CancellationToken cancellationToken)
        {
            List<EnumItemDto> sources = SortEnumMethod.List;
            
            return new MainResponseDto(sources);
        }
    }
}
