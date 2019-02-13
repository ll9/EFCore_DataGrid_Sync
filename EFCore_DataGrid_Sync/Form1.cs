using EFCore_DataGrid_Sync.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.EndEdit();
            _context.SaveChanges();
        }
    }
}
