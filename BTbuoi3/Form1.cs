using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTbuoi3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAddName_Click(object sender, EventArgs e)
        {
            ListViewItem item = new ListViewItem(txtLastName.Text);
            item.SubItems.Add(txtFirstName.Text);
            item.SubItems.Add(txtPhone.Text);

            lvSinhVien.Items.Add(item);

        }

        private void lvSinhVien_Click(object sender, EventArgs e)
        {
            if (lvSinhVien.SelectedItems.Count > 0)
            {
                ListViewItem item = lvSinhVien.SelectedItems[0];
                txtLastName.Text = item.SubItems[0].Text;
                txtFirstName.Text = item.SubItems[1].Text;
                txtPhone.Text = item.SubItems[2].Text;
            }
        }
    }
}
