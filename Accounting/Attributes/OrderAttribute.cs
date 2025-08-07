using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Attributes
{
    internal class OrderAttribute : Attribute
    {
        private int _id;
        public int OrderID => _id;
        public OrderAttribute(int id)
        {
            this._id = id;
        }
        public void echo()
        {
            Console.WriteLine("123");
        }
    }
}
