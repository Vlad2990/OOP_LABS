using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DTOs;

namespace Infrastructure.Adapters
{
    public class QuoteAdapter
    {
        string apiUrl = "https://zenquotes.io/api/random";
        public async Task<QuoteDTO?> GetQuoteAsync()
        {
            using HttpClient client = new();
            try
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                var quotes = JsonConvert.DeserializeObject<QuoteDTO[]>(responseBody);

                if (quotes != null && quotes.Length > 0)
                {
                    return quotes[0];
                }
                return null;
            }
            catch (HttpRequestException)
            {
                return new QuoteDTO("APIs sleep, but robust code never does.", "DeepSeek");
            }
        }
    }
}
