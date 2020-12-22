using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EveApi
{
    public class MarketRepository : IDisposable
    {
        private bool _disposedValue;
        private HttpClient _client = new HttpClient();

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _client.Dispose();
                }

                _client = null;
                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private async Task<T> Get<T>(string url)
        {
            using (HttpResponseMessage response = await _client.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
                }
                else
                {
                    return default(T);
                }
            }
        }

        public async Task<IEnumerable<MarketOrder>> GetMarketOrdersForTheForge(int typeId)
        {
            const string THE_FORGE_REGION_ID = "10000002";
            string url = $"https://esi.evetech.net/latest/markets/{THE_FORGE_REGION_ID}/orders/?datasource=tranquility&order_type=all&page=1&type_id={typeId}";

            return await this.Get<IEnumerable<MarketOrder>>(url);
        }
    }
}
