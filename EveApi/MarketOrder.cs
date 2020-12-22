using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveApi
{
    public class MarketOrder
    {
        public int duration { get; set; }
        public bool is_buy_order { get; set; }
        public DateTime issued { get; set; }
        public long location_id { get; set; }
        public double price { get; set; }
        public string range { get; set; }
        public long system_id { get; set; }
        public long type_id { get; set; }
        public int volume_remain { get; set; }
        public int volume_total { get; set; }
    }
}
