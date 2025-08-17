using Accounting.Attributes;
using Accounting.Models;
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

namespace Accounting.Forms
{
    [DisplayName("記一筆")]
    [Order(1)]
    public partial class AccountForm : Form
    {
        public AccountForm()
        {
            InitializeComponent();

            //DialogResult dialogResult = MessageBox.Show(text: "text", caption: "title", MessageBoxButtons.YesNoCancel);
            //Console.WriteLine(dialogResult);
            //Console.WriteLine(DialogResult.OK);
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
            if (Type.SelectedValue is string)
            {
                Detail.DataSource = DataModels.Type_Mapping[Type.SelectedValue.ToString()];
            }
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
            string path = "C:\\Users\\Albert\\Github\\repos\\private\\c_sharp\\leo_class\\winform\\AccountingDatas";
            string directory = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string file = "records.csv";
            string filePath = Path.Combine(path, directory, file);

            if (!Directory.Exists($"{path}\\{directory}\\_imageLocation")) { Directory.CreateDirectory($"{path}\\{directory}\\_imageLocation"); }

            string[] pathArray = filePath.Split('\\');
            string imgDirectory = string.Join("\\", pathArray.Take(pathArray.Length - 1).ToArray());


            Guid g1 = Guid.NewGuid();
            Guid g2 = Guid.NewGuid();
            SaveImage(pictureBox1, $"{imgDirectory}\\_imageLocation", g1);
            SaveImage(pictureBox2, $"{imgDirectory}\\_imageLocation", g2);
            string guid_1 = SaveImage2(pictureBox1, $"{imgDirectory}\\_imageLocation", g1);
            string guid_2 = SaveImage2(pictureBox2, $"{imgDirectory}\\_imageLocation", g2);


            AccountRecord accountRecord = new AccountRecord()
            {
                datetime = this.dateTimePicker1.Value.ToString("yyyy-MM-dd"),
                money = this.Money.Text,
                type = this.Type.Text,
                detail = this.Detail.Text,
                target = this.Target.Text,
                payment = this.Payment.Text,
                image1 = guid_1,
                image2 = guid_2
            };
            //List<AccountRecord> record_list = CSVHelper.Read<AccountRecord>(filePath);
            //foreach (AccountRecord record in record_list)
            //{
            //    CSVHelper.Write("C:\\Users\\Albert\\Github\\repos\\private\\c_sharp\\leo_class\\winform\\Accounting\\WriteTextAsync-2.txt", record, true);
            //}
            CSVHelper.Write(filePath, accountRecord, true);
        }

        private bool CheckFileHeader(string filepath)
        {
            using (StreamReader sr = new StreamReader(filepath))
            {
                string line;
                line = sr.ReadLine();
                if (line != "datetime,money,type,detail,target,payment,image1,image2")
                {
                    return false;
                }
            }
            return true;
        }
        private bool CheckFileEndtoStream(string filepath)
        {
            using (StreamReader sr = new StreamReader(filepath))
            {
                return sr.EndOfStream;
            }
        }
        private string SaveImage(PictureBox pictureBox, string path, Guid g)
        {
            //Guid g = Guid.NewGuid();
            using (Bitmap bmp1 = new Bitmap(pictureBox.Image))
            {
                ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                EncoderParameters myEncoderParameters = new EncoderParameters(1);
                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 10L);
                myEncoderParameters.Param[0] = myEncoderParameter;
                bmp1.Save($"{path}\\popup_{g.ToString()}.jpg", jpgEncoder, myEncoderParameters);
            }
            //pictureBox.Image.Save($"{path}\\{g.ToString()}.jpg");
            return $"{path}\\popup_{g.ToString()}.jpg";
        }
        private string SaveImage2(PictureBox pictureBox, string path, Guid g)
        {
            //Guid g = Guid.NewGuid();
            Bitmap bmp1 = new Bitmap(pictureBox.Image);
            Bitmap resizedImage = new Bitmap(40, 40);

            using (Graphics graphics = Graphics.FromImage(resizedImage))
            {
                graphics.DrawImage(bmp1, 0, 0, 40, 40);
            }

            resizedImage.Save($"{path}\\{g.ToString()}.jpg", ImageFormat.Jpeg);
            return $"{path}\\{g.ToString()}.jpg";
        }
        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }
}

