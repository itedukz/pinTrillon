using AutoMapper;
using MediatR;
using ms.MainApi.Business.ExpressionParser;
using ms.MainApi.DataAccess.Projects;
using ms.MainApi.Entity.Models.DbModels.Projects;
using ms.MainApi.Entity.Models.Dtos.Pictures;
using ms.MainApi.Entity.Models.Dtos.Projects;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using ms.MainApi.Entity.Models.Pages.SearchPages;

namespace ms.MainApi.Business.Cqrs.MainPages;

public class SearchPageProjectGetListCommand : IRequest<IMainResponseDto>
{
    public SearchProject Form { get; }

    public SearchPageProjectGetListCommand(SearchProject form)
    {
        Form = form;
    }

    public class Handler : IRequestHandler<SearchPageProjectGetListCommand, IMainResponseDto>
    {
        #region DI
        private readonly IMapper _mapper;
        private readonly IProjectDal _entityDal;
        private readonly IProjectPictureDal _projectPictureDal;
        private readonly ISearchProjectExpressionParser _expressionParser;

        public Handler(IMapper mapper, IProjectDal entityDal, IProjectPictureDal projectPictureDal,
            ISearchProjectExpressionParser expressionParser)
        {
            _mapper = mapper;
            _entityDal = entityDal;
            _projectPictureDal = projectPictureDal;
            _expressionParser = expressionParser;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(SearchPageProjectGetListCommand request, CancellationToken cancellationToken)
        {
            var filter = _expressionParser.ParseExpressionOf<Project>(request.Form);

            List<Project> projects = await _entityDal.GetAllAsync(filter);

            //if (request.Form.query!.colorsId != null && request.Form.query!.colorsId.Count > 0 && request.Form.query!.colorsId[0] != 0)
            //{
            //    try
            //    {
            //        projects = projects.Where(i => i.colorsId.All(m => request.Form.query!.colorsId.Contains(m))).ToList();
            //    }
            //    catch { }
            //}

            int totalItems = projects.Count;
            int pageSize = request.Form.pageSize > 0 ? request.Form.pageSize : 10;
            int page = request.Form.page > 0 ? request.Form.page - 1 : 0;
            projects = projects.Skip(page * pageSize).Take(pageSize).ToList();

            #region Sort
            //if (false && request.Form.sort != null)
            //{
            //    if (request.Form.sort.orderByName != null)
            //    {
            //        bool orderByName = request.Form.sort.orderByName ?? true;

            //        projects = orderByName ?
            //            projects.OrderBy(o => o.name).ToList() :
            //            projects.OrderByDescending(o => o.name).ToList();
            //    }
            //    else if (request.Form.sort.orderByPrice != null)
            //    {
            //        bool orderByPrice = request.Form.sort.orderByPrice ?? true;

            //        projects = orderByPrice ?
            //            projects.OrderBy(o => o.price).ToList() :
            //            projects.OrderByDescending(o => o.price).ToList();
            //    }
            //    else if (request.Form.sort.orderByDate != null)
            //    {
            //        bool orderByDate = request.Form.sort.orderByDate ?? true;

            //        projects = orderByDate ?
            //            projects.OrderBy(o => o.createdAt).ToList() :
            //            projects.OrderByDescending(o => o.createdAt).ToList();
            //    }
            //}
            #endregion

            #region projects with main pictures

            SearchProjectPage result = new SearchProjectPage
            {
                projects = projects.Select(i => _mapper.Map<ProjectShortDto>(i)).ToList()
            };
            //result.projects = projects.Select(i => _mapper.Map<ProjectShortDto>(i)).ToList();

            List<int> projectsId = projects.Select(i => i.id).ToList();
            List<ProjectPicture> pictures = await _projectPictureDal.GetAllAsync(i => projectsId.Contains(i.projectId) && i.isMain);

            foreach (ProjectShortDto project in result.projects)
            {
                ProjectPicture? picture = pictures.FirstOrDefault(i => i.projectId == project.id);
                project.picture = _mapper.Map<PictureDto>(picture);
            }
            #endregion

            #region Sort
            result.projects = request.Form.sort switch
            {
                (int)SortEnum.orderByName => result.projects.OrderBy(o => o.name).ToList(),
                (int)SortEnum.orderByNameDesc => result.projects.OrderByDescending(o => o.name).ToList(),
                (int)SortEnum.orderByPrice => result.projects.OrderBy(o => o.price).ToList(),
                (int)SortEnum.orderByPriceDesc => result.projects.OrderByDescending(o => o.price).ToList(),
                (int)SortEnum.orderByDate => result.projects.OrderBy(o => o.id).ToList(),
                (int)SortEnum.orderByDateDesc => result.projects.OrderByDescending(o => o.id).ToList(),
                _ => result.projects.OrderBy(o => o.name).ToList()
            };
            #endregion

            return new MainResponseDto(result, totalItems, page, pageSize);
        }
    }
}