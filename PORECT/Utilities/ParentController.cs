using Microsoft.AspNetCore.Mvc;
using PORECT.Helper;

namespace PORECT
{
    public class ParentController : Controller
    {
        protected readonly PORECTLog logger = new PORECTLog();
        protected readonly ApiProcessHelper _api = new ApiProcessHelper();
        protected readonly IHttpContextAccessor _contextAccessor;

        public ParentController()
        {
            
        }
        public ParentController(IServiceProvider? serviceProvider, IHttpContextAccessor? contextAccessor = null)
        {
            if (contextAccessor != null)
                _contextAccessor = contextAccessor;
        }

    }
}
