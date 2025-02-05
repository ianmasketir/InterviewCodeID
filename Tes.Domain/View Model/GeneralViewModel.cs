using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tes.Domain
{
    public class GeneralViewModel
    {
        public int? ID { get; set; }
        /// <summary>
        /// Insert/Update/View
        /// </summary>
        public string? FormMode { get; set; } = "View";
        public string? Value { get; set; }
    }
}
