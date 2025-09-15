using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Accounting.Utility
{
    internal static class FormExtension
    {
        private static System.Threading.Timer timer;
        public static void DebunceTime(this Form Form, Action callback, int durationTime)
        {
            if (timer == null)
            {
                timer = new System.Threading.Timer(_ =>
                { Form.Invoke(new Action(() => callback())); }, null, Timeout.Infinite, 0);
            }
            timer.Change(durationTime, -1);
        }

    }
}
