using Microsoft.AspNetCore.Mvc;
using Tes.Business;
using PORECT.Helper;

namespace PORECT.API
{
    public class ParentController : Controller
    {
        protected readonly PORECTLog logger = new PORECTLog();
        protected readonly TesProcessor _processor = new TesProcessor();

        public ParentController()
        {
        }

    }
}
