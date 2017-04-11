using Ninject;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace CountingKs.Services
{
    public class NinjectWebApiFilterProvider : IFilterProvider
    {
        private IKernel _kernel;

        public NinjectWebApiFilterProvider(IKernel kernel)
        {
            _kernel = kernel;
        }

        public IEnumerable<FilterInfo> GetFilters(HttpConfiguration configuration, HttpActionDescriptor actionDescriptor)
        {
            var controllerFilters = actionDescriptor.ControllerDescriptor.GetFilters().Select(instance => new FilterInfo(instance, FilterScope.Controller));
            var actionFilters = actionDescriptor.GetFilters().Select(instance => new FilterInfo(instance, FilterScope.Action));

            var filters = controllerFilters.Concat(actionFilters);

            foreach (var filter in filters)
            {
                // Injection
                _kernel.Inject(filter.Instance);
            }

            return filters;
        }
    }
}