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

            dataGridView1.CellValueChanged += new DataGridViewCellEventHandler(dataGridView1_CellValueChanged);
            dataGridView1.CurrentCellDirtyStateChanged += new EventHandler(dataGridView1_CurrentCellDirtyStateChanged);


            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();

            GC.Collect();


            string path = "C:\\Users\\Albert\\Github\\repos\\private\\c_sharp\\leo_class\\winform\\AccountingDatas";
            string file = "records.csv";

            for (int i = 0; i < diffDays + 1; i++)
            {
                string directory = startDatePicker.Value.AddDays(i).ToString("yyyy-MM-dd");
                string filePath = Path.Combine(path, directory, file);
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

            var props = typeof(AccountRecord).GetProperties();

            foreach (var prop in props)
            {
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
                    NullValue = new Bitmap("C:\\Users\\Albert\\Github\\repos\\private\\c_sharp\\leo_class\\winform\\Accounting\\" + "istockphoto.jpg")
                }
            };

            dataGridView1.Columns.Add(trashImageColumn);

            for (int row = 0; row < dataGridView1.Rows.Count; row++)
            {
                dataGridView1.Rows[row].Cells
                    .OfType<DataGridViewImageCell>()
                    .Where(x => dataGridView1.Columns[x.ColumnIndex].HeaderText != "丟棄")
                    .ToList()
                    .ForEach(x => x.Value = new Bitmap(dataGridView1.Rows[row].Cells[dataGridView1.Columns[x.ColumnIndex].Tag.ToString()].Value.ToString()));


                var detailCombobox = dataGridView1.Rows[row].Cells
                   .OfType<DataGridViewComboBoxCell>()
                   .First(x => dataGridView1.Columns[x.ColumnIndex].Name == "detail_combobox");

                detailCombobox.DataSource = DataModels.Type_Mapping[dataGridView1.Rows[row].Cells["type"].Value.ToString()];
                detailCombobox.Value = dataGridView1.Rows[row].Cells["detail"].Value;
            }
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
            if (dataGridViewCellCollection[e.ColumnIndex] is DataGridViewImageCell)
            {
                var columnValue = dataGridView1.Columns[e.ColumnIndex].Tag.ToString();
                var originImagePath = dataGridView1.Rows[e.RowIndex].Cells[columnValue].Value.ToString();
                string[] pathArray = originImagePath.ToString().Split('\\');
                string fileName = pathArray[pathArray.Length - 1];
                string newFileName = "popup_" + fileName;
                pathArray[pathArray.Length - 1] = newFileName;
                string newPath = string.Join("\\", pathArray);


                Form imageBox = new ImageBox($"{newPath}");
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
        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView1.IsCurrentCellDirty)
            {
                // This fires the cell value changed handler below
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }


    }
}
