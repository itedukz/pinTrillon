using AngleSharp.Dom;
using AutoMapper;
using MediatR;
using ms.MainApi.Business.Services;
using ms.MainApi.DataAccess.Projects;
using ms.MainApi.Entity.Models.DbModels.Projects;
using ms.MainApi.Entity.Models.Dtos.Pictures;
using ms.MainApi.Entity.Models.Dtos.Projects;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using ms.MainApi.Entity.Models.Services;
using Newtonsoft.Json;

namespace ms.MainApi.Business.Cqrs.Projects;

public class ProjectGetListCommand : IRequest<IMainResponseDto>
{
    public QueryPagination Form { get; }

    public ProjectGetListCommand(QueryPagination form)
    {
        Form = form;
    }

    public class Handler : IRequestHandler<ProjectGetListCommand, IMainResponseDto>
    {
        #region DI
        private readonly IMapper _mapper;
        private readonly IProjectDal _entityDal;
        private readonly IProjectPictureDal _projectPictureDal;
        private readonly IProjectLayoutDal _projectLayoutDal;
        private readonly IPermissionCheck _checkPermission;

        public Handler(IMapper mapper, IProjectDal entityDal, IProjectPictureDal projectPictureDal, IProjectLayoutDal projectLayoutDal, IPermissionCheck checkPermission)
        {
            _mapper = mapper;
            _entityDal = entityDal;
            _projectPictureDal = projectPictureDal;
            _projectLayoutDal = projectLayoutDal;
            _checkPermission = checkPermission;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(ProjectGetListCommand request, CancellationToken cancellationToken)
        {
            #region checkPermission
            //List<PermissionAction> actions = new List<PermissionAction>() {
            //    PermissionAction.getAll,
            //    PermissionAction.getOwn
            //};
            //var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.project, actions);

            //if (!permission.isSuccess)
            //    return new MainResponseDto(permission.message, HttpStatusCode.Forbidden);
            #endregion

            #region checkPermission
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.project);
            #endregion

            string Query = JsonConvert.SerializeObject(request.Form.query);
            List<Project> sources = await _entityDal.GetAllQueryAsync(Query);

            if (!string.IsNullOrEmpty(request.Form.search))
            {
                string srchTxt = request.Form.search.ToLower();

                sources = await _entityDal.GetAllAsync(i => i.name.ToLower().Contains(srchTxt) ||
                                                            i.description!.ToLower().Contains(srchTxt));
            }

            int totalItems = sources.Count;
            int pageSize = request.Form.pageSize > 0 ? request.Form.pageSize : 10;
            int page = request.Form.page > 0 ? request.Form.page - 1 : 0;
            sources = sources.Skip(page * pageSize).Take(pageSize).ToList();

            List<ProjectPicture> pictures = await _projectPictureDal.GetAllAsync(i => i.isMain);
            List<ProjectLayout> layouts = await _projectLayoutDal.GetAllAsync();

            List<ProjectDto> sourcesDto = new List<ProjectDto>();
            foreach (Project source in sources)
            {
                ProjectDto sourceDto = _mapper.Map<ProjectDto>(source);

                List<ProjectPicture> sourcePictures = pictures.Where(i => i.projectId == source.id).OrderBy(o => o.isMain).Take(2).ToList();
                sourceDto.pictures = sourcePictures.Select(i => _mapper.Map<PictureDto>(i)).ToList();
                sourceDto.layout = _mapper.Map<ProjectLayoutDto>(layouts.FirstOrDefault(i => i.projectId == source.id));

                sourcesDto.Add(sourceDto);
            }

            return new MainResponseDto(sourcesDto, permission.permittedActions, totalItems, page, pageSize);
        }
    }
}