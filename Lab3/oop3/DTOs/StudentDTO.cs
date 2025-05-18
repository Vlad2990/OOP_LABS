using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class StudentDTO(string name, int grade)
    {
        public readonly string name = name;
        public readonly int grade = grade;
    }
}
