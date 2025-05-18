using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Factory;
using Infrastructure.Adapters;

namespace Application.Service
{
    public class QuoteService
    {
        private readonly QuoteAdapter _adapter = new();

        public async Task<Quote> GetQuoteAsync()
        {
            var quoteDTO = await _adapter.GetQuoteAsync();
            return QuoteFactory.Create(quoteDTO);
        }
    }
}
