using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Models
{
    internal class ModifyRecordDTO
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
