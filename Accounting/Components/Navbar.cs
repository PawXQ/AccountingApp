using Accounting.Attributes;
using Accounting.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Accounting.Components
{
    public partial class Navbar : UserControl
    {
        public Navbar()
        {
            InitializeComponent();

            List<TypeInfo> types = Assembly.GetExecutingAssembly().DefinedTypes.Where(x => x.BaseType == typeof(Form)).ToList();
            this.flowLayoutPanel1.AutoSize = true;
            int margin = 0;
            int totalButtonWidth = flowLayoutPanel1.Width - (margin * (types.Count() - 1));
            int buttonWidth = totalButtonWidth / types.Count();

            types = types.OrderBy(x => x.GetCustomAttribute<OrderAttribute>()?.OrderID ?? 999).ToList();

            foreach (var type in types)
            {
                if (type.GetCustomAttribute<DisplayNameAttribute>() == null) continue;
                string formName = type.GetCustomAttribute<DisplayNameAttribute>().DisplayName;
                int orderid = type.GetCustomAttribute<OrderAttribute>().OrderID;
                Button button = new Button();
                button.Text = formName;
                button.Width = buttonWidth;
                button.Tag = type.Name;
                button.Height = flowLayoutPanel1.Height;
                button.Margin = new Padding(0, 0, margin, 0);
                button.Click += ChangePage_Click;
                this.flowLayoutPanel1.Controls.Add(button);
            }
        }

        private void ChangePage_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            Form form = SignletoForm.CreateForm(button.Tag.ToString());

            form?.Show();
        }

        public void Disable_Button(string formName)
        {
            var buttons = this.flowLayoutPanel1.Controls.OfType<Button>().ToList();

            Button button = buttons.FirstOrDefault<Button>(x => x.Tag.ToString() == formName);
            button.Enabled = false;
        }

        private void Navbar_SizeChanged(object sender, EventArgs e)
        {
            Form current_form = (Form)this.Parent;
            if (current_form != null)
            {
                var buttons = this.flowLayoutPanel1.Controls.OfType<Button>().ToList();

                this.flowLayoutPanel1.AutoSize = true;
                this.flowLayoutPanel1.Width = current_form.Width;
                this.flowLayoutPanel1.Height = this.Height;
                int margin = 0;
                int totalButtonWidth = flowLayoutPanel1.Width - (margin * (buttons.Count() - 1));
                int buttonWidth = totalButtonWidth / buttons.Count();


                foreach (Button button in buttons)
                {
                    button.Width = buttonWidth;
                    button.Height = flowLayoutPanel1.Height;
                }
            }
        }
    }
}
