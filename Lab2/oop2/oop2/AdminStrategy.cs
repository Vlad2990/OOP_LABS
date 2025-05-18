using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oop2
{
    class AdminStrategy : IRoleStrategy
    {
        public bool CanEdit => true;
        public bool CanManageUsers => true;
    }
}
