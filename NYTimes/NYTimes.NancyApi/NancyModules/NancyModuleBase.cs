using Nancy;
using Nancy.Validation;

namespace NYTimes.NancyApi.NancyModules
{
    public class NancyModuleBase : NancyModule
    {
        protected Response GetErrorResult(ArticlesModule articlesModule, ModelValidationResult modelValidationResult)
          => articlesModule.Response.AsJson(modelValidationResult, HttpStatusCode.BadRequest);

        protected Response GetNotFoundResult(ArticlesModule articlesModule)
          => articlesModule.Response.AsJson(new { status = nameof(HttpStatusCode.NotFound) }, HttpStatusCode.NotFound);
    }
}
