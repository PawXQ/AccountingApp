using Accounting.Attributes;
using Accounting.Models;
using Accounting.Presenter;
using CSVLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Accounting.Contracts.AddRecordContract;

namespace Accounting.Forms
{
    [DisplayName("記一筆")]
    [Order(1)]
    public partial class AccountForm : Form, IAddRecordView
    {
        IAddRecordPresenter addRecordPresenter;
        public AccountForm()
        {
            InitializeComponent();

            this.addRecordPresenter = new AddRecordPresenter(this);
            //DialogResult dialogResult = MessageBox.Show(text: "text", caption: "title", MessageBoxButtons.YesNoCancel);
            //Console.WriteLine(dialogResult);
            //Console.WriteLine(DialogResult.OK);
            Initial();
        }

        private void AccountForm_Load(object sender, EventArgs e)
        {
            Type.DataSource = DataModels.type;
            Target.DataSource = DataModels.target;
            Payment.DataSource = DataModels.payment;
            pictureBox1.Image = Image.FromFile("C:\\Users\\Albert\\Github\\repos\\private\\c_sharp\\leo_class\\winform\\AccountingDatas\\cloud-upload.jpg");
            pictureBox2.Image = Image.FromFile("C:\\Users\\Albert\\Github\\repos\\private\\c_sharp\\leo_class\\winform\\AccountingDatas\\cloud-upload.jpg");
        }

        private void Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (Type.SelectedValue is string)
            //{
            //    Detail.DataSource = DataModels.Type_Mapping[Type.SelectedValue.ToString()];
            //}
            addRecordPresenter.GetTypeDetailList(Type.SelectedValue.ToString());
        }

        private void UploadImageClick(object sender, EventArgs e)
        {
            PictureBox pictureBox = (PictureBox)sender;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "圖片檔|*.png;*.jpg;*.JPEG";
                //openFileDialog.Filter = "PNG|*.png|JPG|*.jpg";
                DialogResult dialogsResult = openFileDialog.ShowDialog();
                //if (dialogsResult == DialogResult.OK) { pictureBox.Image = Image.FromFile(openFileDialog.FileName); }
                if (dialogsResult == DialogResult.OK)
                {
                    if (pictureBox.Image != null)
                    {
                        pictureBox.Image.Dispose();
                        pictureBox.Image = Image.FromFile(openFileDialog.FileName);
                    }
                }
            }
            GC.Collect();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RecordDTO recordDTO = new RecordDTO()
            {
                datetime = this.dateTimePicker1.Value.ToString("yyyy-MM-dd"),
                money = this.Money.Text,
                type = this.Type.Text,
                detail = this.Detail.Text,
                target = this.Target.Text,
                payment = this.Payment.Text,
                image1 = (Bitmap)pictureBox1.Image,
                image2 = (Bitmap)pictureBox2.Image,
            };

            addRecordPresenter.AddRecord(recordDTO);
        }

        public void RenderCombox(FormDataOptionDTO formDataOptionDTO)
        {
            this.Type.DataSource = formDataOptionDTO.typeList;
            this.Detail.DataSource = formDataOptionDTO.detailList;
            this.Target.DataSource = formDataOptionDTO.targetList;
            this.Payment.DataSource = formDataOptionDTO.paymentList;
        }

        public void RenderDetailCombox(List<string> detailList)
        {
            this.Detail.DataSource = detailList;
        }

        public void Initial()
        {
            this.Money.Text = "0";
            pictureBox1.Image = Image.FromFile("C:\\Users\\Albert\\Github\\repos\\private\\c_sharp\\leo_class\\winform\\AccountingDatas\\cloud-upload.jpg");
            pictureBox2.Image = Image.FromFile("C:\\Users\\Albert\\Github\\repos\\private\\c_sharp\\leo_class\\winform\\AccountingDatas\\cloud-upload.jpg");
        }
    }

}

