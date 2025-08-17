using Accounting.Attributes;
using Accounting.Models;
using Accounting.Utinity;
using CSVLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Accounting.Forms
{
    [DisplayName("記帳本")]
    [Order(2)]
    public partial class AccountBookForm : Form
    {
        private List<AccountRecord> record_list = new List<AccountRecord>();
        string path = "C:\\Users\\Albert\\Github\\repos\\private\\c_sharp\\leo_class\\winform\\AccountingDatas";
        string recordFile = "records.csv";

        public AccountBookForm()
        {
            InitializeComponent();
        }

        private void AccountBookForm_Load(object sender, EventArgs e)
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            this.startDatePicker.Value = new DateTime(year, month, 1);
            this.endDatePicker.Value = new DateTime(year, month + 1, 1).AddDays(-1);
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        private void loadData()
        {
            var diff = endDatePicker.Value - startDatePicker.Value;
            int diffDays = diff.Days;


            this.record_list.Clear();
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();

            GC.Collect();


            for (int i = 0; i < diffDays + 1; i++)
            {
                string directory = startDatePicker.Value.AddDays(i).ToString("yyyy-MM-dd");
                string filePath = Path.Combine(this.path, directory, this.recordFile);
                if (!File.Exists(filePath)) { continue; }
                this.record_list.AddRange(CSVHelper.Read<AccountRecord>(filePath));
            }


            //DataGridColumn 欄 (父類別)
            //DataGridRow 列
            //DataGridCell 格

            //List<AccountRecord> record_list = CSVHelper.Read<AccountRecord>(filePath);
            dataGridView1.DataSource = this.record_list;
            // 1.根據AccountRecord 類別進行反射找出所有Property
            // 2.根據每一個Property統一建立DataGridTextboxColumn
            // 3.將record_list 跑 for loop 逐一建立每一筆的DataGridRow
            // 4.將每一筆DataGridRow裡面的 DataGridTextboxCell 填充該筆 list內的資料到每一格cell內


            foreach (var prop in typeof(AccountRecord).GetProperties())
            {
                bool? IsReadOnly = prop.GetCustomAttribute<ReadOnlyAttribute>()?.IsReadOnly;
                if (IsReadOnly != null && IsReadOnly.Value)
                {
                    dataGridView1.Columns[prop.Name].ReadOnly = true;
                }

                if (prop.GetCustomAttribute<ComboBoxColumnAttribute>() != null)
                {
                    DataGridViewComboBoxColumn dataGridViewComboBoxColumn = new DataGridViewComboBoxColumn()
                    {
                        Name = prop.Name + "_combobox",
                        HeaderText = prop.GetCustomAttribute<DisplayNameAttribute>().DisplayName,
                        DataSource = prop.Name == "detail" ? null : typeof(DataModels).GetField(prop.Name, BindingFlags.Public | BindingFlags.Static).GetValue(null),
                        Tag = prop.Name,
                        DataPropertyName = prop.Name,
                    };

                    int currentColumnIndex = dataGridView1.Columns[prop.Name].Index;
                    dataGridView1.Columns.Insert(currentColumnIndex, dataGridViewComboBoxColumn);
                    dataGridView1.Columns[prop.Name].Visible = false;
                }

                if (prop.GetCustomAttribute<ImageColumnAttribute>() != null)
                {
                    DataGridViewImageColumn dataGridViewImageColumn = new DataGridViewImageColumn()
                    {
                        Name = prop.Name + "_image",
                        HeaderText = prop.GetCustomAttribute<DisplayNameAttribute>().DisplayName,
                        Tag = prop.Name
                    };

                    int currentColumnIndex = dataGridView1.Columns[prop.Name].Index;
                    dataGridView1.Columns.Insert(currentColumnIndex, dataGridViewImageColumn);
                    dataGridView1.Columns[prop.Name].Visible = false;
                }
            }

            DataGridViewImageColumn trashImageColumn = new DataGridViewImageColumn()
            {
                HeaderText = $"丟棄",
                ImageLayout = DataGridViewImageCellLayout.Zoom,
                DefaultCellStyle = new DataGridViewCellStyle()
                {
                    NullValue = new Bitmap(Path.Combine(path, "istockphoto.jpg"))
                }
            };

            dataGridView1.Columns.Add(trashImageColumn);



            for (int row = 0; row < dataGridView1.Rows.Count; row++)
            {
                dataGridView1.Rows[row].Cells
                    .OfType<DataGridViewImageCell>()
                    .Where(x => dataGridView1.Columns[x.ColumnIndex].HeaderText != "丟棄")
                    .ToList()
                    .ForEach(x =>
                    {
                        byte[] imageBytes = File.ReadAllBytes(dataGridView1.Rows[row].Cells[dataGridView1.Columns[x.ColumnIndex].Tag.ToString()].Value.ToString());
                        x.Value = new Bitmap(new MemoryStream(imageBytes));
                    });

                var detailCombobox = dataGridView1.Rows[row].Cells
                   .OfType<DataGridViewComboBoxCell>()
                   .First(x => dataGridView1.Columns[x.ColumnIndex].Name == "detail_combobox");

                detailCombobox.DataSource = DataModels.Type_Mapping[dataGridView1.Rows[row].Cells["type"].Value.ToString()];
                detailCombobox.Value = dataGridView1.Rows[row].Cells["detail"].Value;
            }

            dataGridView1.CellValueChanged += new DataGridViewCellEventHandler(dataGridView1_CellValueChanged);
            dataGridView1.CurrentCellDirtyStateChanged += new EventHandler(dataGridView1_CurrentCellDirtyStateChanged);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.endDatePicker.Value < this.startDatePicker.Value)
            {
                MessageBox.Show(text: "End Time can't before start time");
                return;
            }
            this.DebunceTime(this.loadData, 1000);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0) { return; }

            DataGridViewCellCollection dataGridViewCellCollection = dataGridView1.Rows[e.RowIndex].Cells;

            if (dataGridViewCellCollection[e.ColumnIndex] is DataGridViewImageCell &&
                dataGridView1.Columns[e.ColumnIndex].HeaderText == "丟棄")
            {
                typeof(AccountRecord).GetProperties()
                    .Where(x => x.GetCustomAttribute<ImageColumnAttribute>() != null)
                    .ToList()
                    .ForEach(x =>
                    {
                        var imageLocation = dataGridView1.Rows[e.RowIndex].Cells[x.Name].Value.ToString();
                        var imagePopLocation = reOrgImagePath(imageLocation);
                        ((Bitmap)dataGridView1.Rows[e.RowIndex].Cells[$"{x.Name}_image"].Value).Dispose();
                        File.Delete(imageLocation.ToString());
                        File.Delete(imagePopLocation.ToString());
                    });


                //dataGridView1.Rows[e.RowIndex].Cells
                //    .OfType<DataGridViewImageCell>()
                //    .Where(x => x.Value != null)
                //    .ToList()
                //    .ForEach(x =>
                //    {
                //        string propName = dataGridView1.Columns[x.ColumnIndex].Tag.ToString();
                //        var imageLocation = dataGridView1.Rows[e.RowIndex].Cells[propName].Value.ToString();
                //        var imagePopLocation = reOrgImagePath(imageLocation);
                //        ((Bitmap)dataGridView1.Rows[e.RowIndex].Cells[$"{propName}_image"].Value).Dispose();
                //        File.Delete(imageLocation.ToString());
                //        File.Delete(imagePopLocation.ToString());
                //    });

                string deleteDatetime = dataGridView1.Rows[e.RowIndex].Cells["datetime"].Value.ToString();

                AccountRecord removedRecored = record_list[e.RowIndex];
                record_list.Remove(removedRecored);
                var remainDatas = record_list.Where(x => x.datetime == removedRecored.datetime).ToList();

                string reWriteRecordFile = Path.Combine(this.path, deleteDatetime, this.recordFile);

                Console.WriteLine(reWriteRecordFile);
                File.Delete(Path.Combine(this.path, deleteDatetime, this.recordFile));
                CSVHelper.WriteList(reWriteRecordFile, remainDatas, true);


                string directoryName = Path.GetDirectoryName(removedRecored.image1.ToString());
                if (Directory.GetFiles(directoryName).Length == 0)
                {
                    Directory.Delete(Path.Combine(this.path, deleteDatetime), true);
                }

                this.loadData();
                return;
            }

            if (dataGridViewCellCollection[e.ColumnIndex] is DataGridViewImageCell)
            {
                var columnValue = dataGridView1.Columns[e.ColumnIndex].Tag.ToString();
                var originImagePath = dataGridView1.Rows[e.RowIndex].Cells[columnValue].Value.ToString();
                var popImagePath = reOrgImagePath(originImagePath.ToString());

                Form imageBox = new ImageBox($"{popImagePath}");
                imageBox.ShowDialog();
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "type_combobox")
            {
                DataGridViewComboBoxCell cellDetail = (DataGridViewComboBoxCell)this.dataGridView1.Rows[e.RowIndex].Cells["detail_combobox"];
                List<string> datas = DataModels.Type_Mapping[dataGridView1.Rows[e.RowIndex].Cells["type_combobox"].Value.ToString()];
                cellDetail.DataSource = datas;
                cellDetail.Value = datas[0];
            }

            string reWriteDatetime = dataGridView1.Rows[e.RowIndex].Cells["datetime"].Value.ToString();
            string reWriteRecordFile = Path.Combine(this.path, reWriteDatetime, this.recordFile);
            File.Delete(Path.Combine(this.path, reWriteDatetime, this.recordFile));
            CSVHelper.WriteList(reWriteRecordFile, record_list, true);
        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView1.IsCurrentCellDirty)
            {
                // This fires the cell value changed handler below
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private string reOrgImagePath(string path)
        {
            string[] pathArray = path.ToString().Split('\\');
            string fileName = pathArray[pathArray.Length - 1];
            string newFileName = "popup_" + fileName;
            pathArray[pathArray.Length - 1] = newFileName;
            string newPath = string.Join("\\", pathArray);

            return newPath;
        }
    }
}
