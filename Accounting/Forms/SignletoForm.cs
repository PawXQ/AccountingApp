using Accounting.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Accounting.Forms
{
    internal class SignletoForm
    {
        private static Dictionary<string, Form> dict_forms = new Dictionary<string, Form>();
        private static Form lastForm = null;

        private SignletoForm()
        {
        }

        public static Form CreateForm(string str)
        {
            if (lastForm != null) { lastForm.Hide(); }

            if (dict_forms.ContainsKey(str))
            {
                lastForm = dict_forms[str];
            }
            else
            {
                Type type = Type.GetType("Accounting.Forms." + str);
                dict_forms[str] = (Form)Activator.CreateInstance(type);
            }
            lastForm = dict_forms[str];

            FieldInfo[] fieldInfos = lastForm.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            FieldInfo navbarField = fieldInfos.FirstOrDefault<FieldInfo>(x => x.FieldType == typeof(Navbar));
            //Navbar navbar = (Navbar)fieldInfos.FirstOrDefault<FieldInfo>(x => x.FieldType == typeof(Navbar));
            //Console.WriteLine(fieldInfos[3].GetValue(lastForm));
            //Navbar navbar = (Navbar)fieldInfos[3].GetValue(lastForm);
            Navbar navbar = (Navbar)navbarField.GetValue(lastForm);

            navbar.Disable_Button(lastForm.Text);

            return lastForm;
        }
    }
}
