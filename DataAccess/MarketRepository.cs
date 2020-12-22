using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DataAccess
{
    public class MarketRepository
    {
        private static List<MarketPrice> _marketPrices = null;

        private T Get<T>(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = client.GetAsync(url).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
                    }
                    else
                    {
                        return default(T);
                    }
                }
            }
        }

        private IEnumerable<MarketOrder> GetMarketOrdersForTheForge(int typeId)
        {
            const string THE_FORGE_REGION_ID = "10000002";
            string url = $"https://esi.evetech.net/latest/markets/{THE_FORGE_REGION_ID}/orders/?datasource=tranquility&order_type=all&page=1&type_id={typeId}";
            IEnumerable<MarketOrder> results = this.Get<IEnumerable<MarketOrder>>(url);

            return results;
        }

        private IEnumerable<MarketPrice> GetMarketPrices()
        {
            if(_marketPrices == null)
            {
                string url = "https://esi.evetech.net/latest/markets/prices/?datasource=tranquility";
                
                _marketPrices = this.Get<List<MarketPrice>>(url);
            }

            return _marketPrices;
        }

        public double GetProductLowSellPrice(int typeId)
        {
            double result = 0;
            double orderPrice =  this.GetMarketOrdersForTheForge(typeId).Where(t => !t.is_buy_order).OrderBy(t => t.price).FirstOrDefault().price;
            double adjustedPrice = this.GetMarketPrices().Single(t => t.type_id == typeId).adjusted_price;

            if(orderPrice > adjustedPrice && Math.Abs((orderPrice - adjustedPrice) / adjustedPrice) > .1)
            {
                result = adjustedPrice;
            }
            else
            {
                result = orderPrice;
            }

            return result;
        }
    }
}
