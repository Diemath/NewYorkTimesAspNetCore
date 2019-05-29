using Nancy;
using Nancy.ErrorHandling;
using Nancy.Extensions;
using Nancy.Responses;

namespace NYTimes.NancyApi.Handlers
{
    public class CustomExceptionHandler : IStatusCodeHandler
    {
        public bool HandlesStatusCode(HttpStatusCode statusCode, NancyContext context)
        {
            if (statusCode != HttpStatusCode.InternalServerError)
            {
                return false;
            }

            var exception = context.GetException();

            return exception != null;
        }

        public void Handle(HttpStatusCode statusCode, NancyContext context)
        {
            var exception = context.GetException();

            var response = new JsonResponse(new { error = exception.InnerException.Message }, new DefaultJsonSerializer(context.Environment), context.Environment);

            response.StatusCode = HttpStatusCode.InternalServerError;

            context.Response = response;

            context.Response.WithHeader("Access-Control-Allow-Origin", "*")
                .WithHeader("Access-Control-Allow-Methods", "POST,GET")
                .WithHeader("Access-Control-Allow-Headers", "Accept, Origin, Content-type");
        }
    }
}
