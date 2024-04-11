namespace ms.MainApi.Entity.Models.Services.Identities;

internal class ExternalLoginResult //: ActionResult
{
    public ExternalLoginResult(string provider, string returnUrl)
    {
        Provider = provider;
        ReturnUrl = returnUrl;
    }

    public string Provider { get; private set; }
    public string ReturnUrl { get; private set; }

    //public override void ExecuteResult(ControllerContext context)
    //{
    //    OpenAuth.RequestAuthentication(Provider, ReturnUrl);
    //}
}
