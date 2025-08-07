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
    public partial class ImageBox : Form
    {
        public ImageBox(string path)
        {
            InitializeComponent();
            pictureBox1.Image = Image.FromFile(path);
        }

        private void ImageBox_FormClosing(object sender, FormClosingEventArgs e)
        {
            pictureBox1.Image.Dispose();
        }
    }
}
