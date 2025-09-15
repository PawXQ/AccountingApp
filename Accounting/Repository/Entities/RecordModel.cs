using Accounting.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Repository.Entities
{
    internal class RecordModel
    {
        public string datetime { get; set; }

        public string money { get; set; }

        public string type { get; set; }

        public string detail { get; set; }

        public string target { get; set; }

        public string payment { get; set; }

        public string image1 { get; set; }

        public string image2 { get; set; }

    }
}
