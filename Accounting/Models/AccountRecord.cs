using Accounting.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Models
{
    internal class AccountRecord
    {
        [DisplayName("日期")]
        [ReadOnly(true)]
        public string datetime { get; set; }

        [DisplayName("金額")]
        public string money { get; set; }

        [DisplayName("類別")]
        [ComboBoxColumn]
        public string type { get; set; }

        [DisplayName("細項")]
        [ComboBoxColumn]
        public string detail { get; set; }

        [DisplayName("對象")]
        [ComboBoxColumn]
        public string target { get; set; }

        [DisplayName("消費方式")]
        [ComboBoxColumn]
        public string payment { get; set; }

        [DisplayName("發票1")]
        [ImageColumn]
        public string image1 { get; set; }

        [DisplayName("發票2")]
        [ImageColumn]
        public string image2 { get; set; }


        public AccountRecord() { }


    }
}
