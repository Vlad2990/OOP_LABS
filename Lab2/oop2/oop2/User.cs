using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oop2
{
    public class User
    {
        public string Name { get; }

        public List<string> News { get; private set; } = new();

        public User(string name)
        {
            Name = name;
        }

        public void AddNews(string news)
        {
            News.Add(news);
        }

    }
}