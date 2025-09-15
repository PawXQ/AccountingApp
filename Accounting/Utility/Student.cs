using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Utility
{
    internal class Student
    {
        public int Name;


        public static Student operator +(Student std, object data)
        {
            std.Name += (int)data;
            return std;
        }

    }
}
