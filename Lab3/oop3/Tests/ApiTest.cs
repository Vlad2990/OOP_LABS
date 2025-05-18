using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Adapters;


namespace Tests
{
    public class ApiTest
    {
        public QuoteAdapter quoteAdapter = new();

        [Fact]
        public async void Test()
        {
            var resp = await quoteAdapter.GetQuoteAsync();
            Assert.NotNull(resp);
            Assert.NotNull(resp.Content);
            Assert.NotNull(resp.Author);
        }
    }
}
