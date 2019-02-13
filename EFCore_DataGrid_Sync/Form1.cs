using EFCore_DataGrid_Sync.Data;
using EFCore_DataGrid_Sync.Models;
using Microsoft.EntityFrameworkCore;
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

namespace EFCore_DataGrid_Sync
{
    public partial class Form1 : Form
    {
        private ApplicationDbContext _context;

        public Form1()
        {
            InitializeComponent();
            _context = new ApplicationDbContext();

            dataGridView1.DataSource = _context.People.Local.ToBindingList();
            _context.People.Load();
            dataGridView2.AutoGenerateColumns = false;
            dataGridView2.DataSource = new List<Customer>();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.EndEdit();
            _context.SaveChanges();
        }

        private void gridCustomers_CellFormatting(
  object sender,
  DataGridViewCellFormattingEventArgs e)
        {
            if ((dataGridView2.Rows[e.RowIndex].DataBoundItem != null) &&
                (dataGridView2.Columns[e.ColumnIndex].DataPropertyName.Contains(".")))
            {
                e.Value = BindProperty(
                              dataGridView2.Rows[e.RowIndex].DataBoundItem,
                              dataGridView2.Columns[e.ColumnIndex].DataPropertyName
                            );
            }
        }

        private string BindProperty(object property, string propertyName)
        {
            string retValue = "";

            if (propertyName.Contains("."))
            {
                PropertyInfo[] arrayProperties;
                string leftPropertyName;

                leftPropertyName = propertyName.Substring(0, propertyName.IndexOf("."));
                arrayProperties = property.GetType().GetProperties();

                foreach (PropertyInfo propertyInfo in arrayProperties)
                {
                    if (propertyInfo.Name == leftPropertyName)
                    {
                        retValue = BindProperty(
                          propertyInfo.GetValue(property, null),
                          propertyName.Substring(propertyName.IndexOf(".") + 1));
                        break;
                    }
                }
            }
            else
            {
                Type propertyType;
                PropertyInfo propertyInfo;

                propertyType = property.GetType();
                propertyInfo = propertyType.GetProperty(propertyName);
                retValue = propertyInfo.GetValue(property, null).ToString();
            }

            return retValue;
        }
    }
}
