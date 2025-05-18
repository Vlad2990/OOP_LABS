using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DTOs
{
    public class QuoteDTO(string content, string author)
    {
        [JsonProperty("q")]
        public string Content { get; set; } = content;

        [JsonProperty("a")]
        public string Author { get; set; } = author;
    }
}
