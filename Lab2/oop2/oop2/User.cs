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
        public IRoleStrategy Role { get; private set; }

        public User(string name, IRoleStrategy role)
        {
            Name = name;
            Role = role;
        }

        public bool ChangeRole(IRoleStrategy newRole, User requester)
        {
            if (requester.Role is not AdminStrategy)
            {
                return false;
            }
            Role = newRole;
            return true;
        }

    }
}