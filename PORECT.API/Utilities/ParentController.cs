using Microsoft.AspNetCore.Mvc;
using Tes.Business;
using PORECT.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace PORECT.API
{
    [Authorize]
    [EnableCors("corsapp")]
    public class ParentController : Controller
    {
        protected readonly PORECTLog logger = new PORECTLog();
        protected readonly TesProcessor _processor = new TesProcessor();

        public ParentController()
        {
        }

    }
}
