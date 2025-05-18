using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Factory;
using DTOs;

namespace Tests
{
    public class ValdiationTest
    {
        [Fact]
        public void Test()
        {
            StudentDTO test = new StudentDTO("Test", -1);
            Assert.Throws<ArgumentOutOfRangeException>(() => { StudentFactory.Create(test); });
        }
    }
}
