using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tes.Domain
{
    public class ProductRequest
    {
        public int? ID { get; set; }
        public string? TransactionType { get; set; } = "View";
        public string? ProductCode { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDtm { get; set; }
    }
}
