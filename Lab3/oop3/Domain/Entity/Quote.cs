using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Quote
    {
        public string Content {  get; set; }

        public string Author { get; set; }

        public Quote(string content, string author)
        {
            if (string.IsNullOrEmpty(content) || string.IsNullOrEmpty(author)) throw new ArgumentNullException();
            Content = content;
            Author = author;
        }
    }
}