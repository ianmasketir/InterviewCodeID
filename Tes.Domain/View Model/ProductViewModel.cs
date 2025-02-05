using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tes.Domain
{
    public class ProductViewModel
    {
        public string? FormMode { get; set; } = "View";
        public ProductResponse? Product { get; set; }
    }
}
