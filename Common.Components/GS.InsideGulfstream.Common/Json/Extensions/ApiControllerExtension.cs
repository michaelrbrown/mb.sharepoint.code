using System.Web.Http;

namespace GS.InsideGulfstream.Common.Json.Extensions
{
    public class ApiControllerExtension
    {
        public static NotFoundActionResult NotFound(ApiController controller, string message)
        {
            return new NotFoundActionResult(controller.Request, message);
        }

        public static InternalServerErrorActionResult InternalServerError(ApiController controller, string message)
        {
            return new InternalServerErrorActionResult(controller.Request, message);
        }

    }
}
