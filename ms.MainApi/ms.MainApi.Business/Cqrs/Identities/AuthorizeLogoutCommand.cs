using MediatR;
using Microsoft.AspNetCore.Http;
using ms.MainApi.Entity.Models.Dtos.Responses;

namespace ms.MainApi.Business.Cqrs.Identities;

public class AuthorizeLogoutCommand : IRequest<IMainResponseDto>
{ 
    public class Handler : IRequestHandler<AuthorizeLogoutCommand, IMainResponseDto>
    {
        #region DI
        private readonly IHttpContextAccessor _httpContext;

        public Handler(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(AuthorizeLogoutCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //if(_httpContext != null && _httpContext.HttpContext != null)
                //{
                //    await _httpContext.HttpContext.SignOutAsync("grcAuth");
                //}

                _httpContext?.HttpContext?.Request?.Headers?.Clear();
                _httpContext?.HttpContext?.Response.Headers.Add("Cache-Control", "no-store");

                _httpContext?.HttpContext?.Session.Clear();
                _httpContext?.HttpContext?.Response.Clear();
            }
            catch { }

            return new MainResponseDto("User logout");
        }
    }
}