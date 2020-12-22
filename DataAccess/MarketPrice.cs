using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class MarketPrice
    {
        public double adjusted_price { get; set; }
        public double average_price { get; set; }
        public int type_id { get; set; }
    }
}
