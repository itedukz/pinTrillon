using MediatR;
using ms.MainApi.Entity.Models.Dtos.Identities.PermissionServices;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using ms.MainApi.Entity.Models.Services;

namespace ms.MainApi.Business.Cqrs.Products.Colors;

public class ColorGetListEnumCommand : IRequest<IMainResponseDto>
{
    public QueryPagination Form { get; }

    public ColorGetListEnumCommand(QueryPagination form)
    {
        Form = form;
    }

    public class Handler : IRequestHandler<ColorGetListEnumCommand, IMainResponseDto>
    {
        public async Task<IMainResponseDto> Handle(ColorGetListEnumCommand request, CancellationToken cancellationToken)
        {
            int pageSize = request.Form.pageSize > 0 ? request.Form.pageSize : 10;
            int page = request.Form.page > 0 ? request.Form.page - 1 : 0;

            List<ColorInfo> sources = ColorEnumMethod.List;
            int totalItems = sources.Count;

            if (!string.IsNullOrEmpty(request.Form.search))
            {
                string srchTxt = request.Form.search.ToLower();

                sources = sources.Where(i => i.nameRus.ToLower().Contains(srchTxt) ||
                                                            i.nameEng.ToLower().Contains(srchTxt)).ToList();
                totalItems = sources.Count;
            }

            sources = sources.Skip(page * pageSize).Take(pageSize).ToList();

            return new MainResponseDto(sources, new PermissionList(),  totalItems, page, pageSize);
        }
    }
}
