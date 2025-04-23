using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oop2
{
    class ViewerStrategy : IRoleStrategy
    {
        public bool CanEdit => false;
        public bool CanManageUsers => false;

       
    }
}
