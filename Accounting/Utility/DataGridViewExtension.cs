using Accounting.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Accounting.Utility
{
    internal static class DataGridViewExtension
    {
        public static void GenerateComboboxColumn(this DataGridView dataGridView, PropertyInfo prop)
        {
            DataGridViewComboBoxColumn dataGridViewComboBoxColumn = new DataGridViewComboBoxColumn()
            {
                Name = prop.Name + "_combobox",
                HeaderText = prop.GetCustomAttribute<DisplayNameAttribute>().DisplayName,
                DataSource = prop.Name == "detail" ? null : typeof(DataModels).GetField(prop.Name, BindingFlags.Public | BindingFlags.Static).GetValue(null),
                Tag = prop.Name,
                DataPropertyName = prop.Name,
            };

            int propCurrentColumnIndex = dataGridView.Columns[prop.Name].Index;
            dataGridView.Columns.Insert(propCurrentColumnIndex, dataGridViewComboBoxColumn);
            dataGridView.Columns[prop.Name].Visible = false;
        }
        public static void GenerateImageColumn(this DataGridView dataGridView, PropertyInfo prop)
        {

            DataGridViewImageColumn dataGridViewImageColumn = new DataGridViewImageColumn()
            {
                Name = prop.Name + "_image",
                HeaderText = prop.GetCustomAttribute<DisplayNameAttribute>().DisplayName,
                Tag = prop.Name,
            };

            int propCurrentColumnIndex = dataGridView.Columns[prop.Name].Index;
            dataGridView.Columns.Insert(propCurrentColumnIndex, dataGridViewImageColumn);
            dataGridView.Columns[prop.Name].Visible = false;
        }
        public static void GenerateTrashImageColumn(this DataGridView dataGridView, string headerText, string path)
        {
            DataGridViewImageColumn dataGridViewImageColumn = new DataGridViewImageColumn()
            {
                HeaderText = headerText,
                ImageLayout = DataGridViewImageCellLayout.Zoom,
                DefaultCellStyle = new DataGridViewCellStyle()
                {
                    NullValue = new Bitmap(Path.Combine(path, "istockphoto.jpg"))
                }
            };
            dataGridView.Columns.Add(dataGridViewImageColumn);
        }
    }
}
