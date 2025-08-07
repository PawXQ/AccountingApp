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
    [DisplayName("帳戶分析")]
    [Order(3)]
    public partial class CalculateForm : Form
    {
        public CalculateForm()
        {
            InitializeComponent();
        }
    }
}
