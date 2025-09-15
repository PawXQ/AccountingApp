using Accounting.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Accounting.Forms
{
    [DisplayName("圖表分析")]
    [Order(4)]
    public partial class ReportForm : Form
    {
        public ReportForm()
        {
            InitializeComponent();
        }

    }
}
