using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oop2
{
    public class EditorStrategy : IRoleStrategy
    {
        public bool CanEdit => true;
        public bool CanManageUsers => false;
    }
}
