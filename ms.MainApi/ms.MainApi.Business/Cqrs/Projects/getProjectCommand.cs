using AutoMapper;
using MediatR;
using ms.MainApi.DataAccess.Projects;
using ms.MainApi.Entity.Models.DbModels.Projects;
using ms.MainApi.Entity.Models.Dtos.Pictures;
using ms.MainApi.Entity.Models.Dtos.Projects;
using ms.MainApi.Entity.Models.Dtos.Projects.ProjectCatalogs;
using ms.MainApi.Entity.Models.Enums;

namespace ms.MainApi.Business.Cqrs.Projects;

public class getProjectCommand : IRequest<ProjectDto?>
{
    public int id { get; }

    public getProjectCommand(int _id)
    {
        id = _id;
    }

    public class Handler : IRequestHandler<getProjectCommand, ProjectDto?>
    {
        #region DI
        private readonly IMapper _mapper;
        private readonly IProjectDal _entityDal;
        private readonly IProjectPictureDal _projectPictureDal;
        private readonly IProjectLayoutDal _projectLayoutDal;
        private readonly IProjectCatalogDal _catalogDal;

        public Handler(IMapper mapper, IProjectDal entityDal, IProjectPictureDal projectPictureDal, IProjectLayoutDal projectLayoutDal, IProjectCatalogDal catalogDal)
        {
            _mapper = mapper;
            _entityDal = entityDal;
            _projectPictureDal = projectPictureDal;
            _projectLayoutDal = projectLayoutDal;
            _catalogDal = catalogDal;
        }
        #endregion

        public async Task<ProjectDto?> Handle(getProjectCommand request, CancellationToken cancellationToken)
        {
            Project? entity = await _entityDal.GetAsync(i => i.id == request.id);
            if (entity == null)
                return null;

            ProjectDto entityDto = _mapper.Map<ProjectDto>(entity);
            entityDto.projectCatalog = _mapper.Map<ProjectCatalogDto>(await _catalogDal.GetAsync(i => i.id == entity.projectCatalogId));
            entityDto.measure = new Entity.Models.Dtos.Measures.MeasureDto(entity.height, entity.width, entity.length, (int)MeasureEnum.m);
            //entityDto.colors = ColorEnumMethod.getByList(entity.colorsId);

            List<ProjectPicture> pictures = await _projectPictureDal.GetAllAsync(i => i.projectId == entity.id);
            entityDto.pictures = pictures.Select(i => _mapper.Map<PictureDto>(i)).ToList();

            entityDto.layout = _mapper.Map<ProjectLayoutDto>(await _projectLayoutDal.GetAsync(i => i.projectId == entity.id));

            return entityDto;
        }
    }
}