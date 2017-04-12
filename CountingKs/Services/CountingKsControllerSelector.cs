using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace CountingKs.Services
{
    public class CountingKsControllerSelector : DefaultHttpControllerSelector
    {
        private HttpConfiguration _config;

        public CountingKsControllerSelector(HttpConfiguration config) : base(config)
        {
            _config = config;
        }

        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            var controllers = GetControllerMapping();

            var routeData = request.GetRouteData();

            var controllerName = (string) routeData.Values["controller"];

            HttpControllerDescriptor descriptor;

            if (controllers.TryGetValue(controllerName, out descriptor))
            {
                var version = GetVersionFromQueryString(request);

                var newName = string.Concat(controllerName, "V", version);

                HttpControllerDescriptor versionedDescriptor;

                if (controllers.TryGetValue(newName, out versionedDescriptor))
                {
                    return versionedDescriptor;
                }

                return descriptor;
            }

            return null;
        }

        private string GetVersionFromQueryString(HttpRequestMessage request)
        {
            var query = HttpUtility.ParseQueryString(request.RequestUri.Query);

            var version = query["v"];
            if (version != null)
            {
                return version;
            }

            return "1";
        }
    }
}