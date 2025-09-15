using Accounting.Attributes;
using Accounting.Models;
using Accounting.Presenter;
using Accounting.Utility;
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
using static Accounting.Contracts.ModifyRecordContract;

namespace Accounting.Forms
{
    [DisplayName("記帳本")]
    [Order(2)]
    internal partial class AccountBookForm : Form, IModifyRecordView
    {
        IModifyRecordPresenter modifyRecordPresenter;

        private List<AccountRecord> record_list = new List<AccountRecord>();
        string path = "C:\\Users\\Albert\\Github\\repos\\private\\c_sharp\\leo_class\\winform\\AccountingDatas";

        public AccountBookForm()
        {
            InitializeComponent();
            this.modifyRecordPresenter = new ModifyRecordPresenter(this);
        }

        private void AccountBookForm_Load(object sender, EventArgs e)
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            this.startDatePicker.Value = new DateTime(year, month, 1);
            this.endDatePicker.Value = new DateTime(year, month + 1, 1).AddDays(-1);
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        private void RenderData()
        {
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();

            GC.Collect();

            dataGridView1.DataSource = this.record_list;

            //DataGridColumn 欄 (父類別)
            //DataGridRow 列
            //DataGridCell 格
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
                    this.dataGridView1.GenerateComboboxColumn(prop);
                }

                if (prop.GetCustomAttribute<ImageColumnAttribute>() != null)
                {
                    this.dataGridView1.GenerateImageColumn(prop);
                }
            }

            this.dataGridView1.GenerateTrashImageColumn(headerText: "丟棄", path: path);


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
            this.DebunceTime(() => modifyRecordPresenter.GetRecord(startDatePicker.Value, endDatePicker.Value), 1000);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0) { return; }

            DataGridViewCellCollection dataGridViewCellCollection = dataGridView1.Rows[e.RowIndex].Cells;

            if (dataGridViewCellCollection[e.ColumnIndex] is DataGridViewImageCell &&
                dataGridView1.Columns[e.ColumnIndex].HeaderText == "丟棄")
            {
                //TODO: 這裡之後會用AutoMapper 做掉

                ModifyRecordDTO modifyRecordDTO = new ModifyRecordDTO();
                AccountRecord deletedAccountRecord = this.record_list[e.RowIndex];

                PropertyInfo[] accountRecordProps = typeof(AccountRecord).GetProperties();

                foreach (PropertyInfo sourceProp in accountRecordProps)
                {
                    object sourceValue = sourceProp.GetValue(deletedAccountRecord);
                    PropertyInfo destProp = typeof(ModifyRecordDTO).GetProperty(sourceProp.Name);
                    if (destProp != null)
                    {
                        object destValue = Convert.ChangeType(sourceValue, destProp.PropertyType);
                        destProp.SetValue(modifyRecordDTO, destValue);
                    }
                }

                modifyRecordPresenter.DeleteRecord(modifyRecordDTO);
                modifyRecordPresenter.GetRecord(startDatePicker.Value, endDatePicker.Value);

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

            //TODO: 這裡之後會用AutoMapper 做掉
            ModifyRecordDTO modifyRecordDTO = new ModifyRecordDTO();
            AccountRecord modifyAccountRecord = this.record_list[e.RowIndex];

            PropertyInfo[] accountRecordProps = typeof(AccountRecord).GetProperties();

            foreach (PropertyInfo sourceProp in accountRecordProps)
            {
                object sourceValue = sourceProp.GetValue(modifyAccountRecord);
                PropertyInfo destProp = typeof(ModifyRecordDTO).GetProperty(sourceProp.Name);
                if (destProp != null)
                {
                    object destValue = Convert.ChangeType(sourceValue, destProp.PropertyType);
                    destProp.SetValue(modifyRecordDTO, destValue);
                }
            }

            modifyRecordPresenter.ModifyRecord(modifyRecordDTO);
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
            string newFileName = "pop_" + fileName;
            pathArray[pathArray.Length - 1] = newFileName;
            string newPath = string.Join("\\", pathArray);

            return newPath;
        }

        public void RenderDateGridView(List<ModifyRecordDTO> modifyRecordDTOList)
        {
            this.record_list.Clear();
            this.record_list = modifyRecordDTOList
                .Select(x => new AccountRecord
                {
                    datetime = x.datetime,
                    money = x.money,
                    type = x.type,
                    detail = x.detail,
                    target = x.target,
                    payment = x.payment,
                    image1 = x.image1,
                    image2 = x.image2,
                }).ToList();

            RenderData();
        }
    }
}
