using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Factory
{
    public static class QuoteFactory
    {
        public static Quote Create(QuoteDTO quote)
        {
            return new Quote(quote.Content, quote.Author);
        }
    }
}
