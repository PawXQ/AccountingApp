using Accounting.Components;
using Accounting.Forms;
using Accounting.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Accounting
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {





            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form AccountBookForm = SignletoForm.CreateForm("AccountBookForm");
            Application.Run(AccountBookForm);
        }
    }
}
