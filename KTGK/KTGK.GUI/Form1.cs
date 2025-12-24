using KTGK.BUS;
using KTGK.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KTGK.GUI
{
    public partial class Form1 : Form
    {
        private readonly SanphamService sanphamservice = new SanphamService();
        private readonly LoaisanphamService loaisanphamservice = new LoaisanphamService();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                setGridViewStyle(dataGridView1);

                var listsanpham = sanphamservice.GetAll();
                var listloaisanpham = loaisanphamservice.GetAll();

                FillLoaiSp(listloaisanpham);
                BindGrid(listsanpham);
            }

            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FillLoaiSp(List<LoaiSP> listloaisanpham)
        {
            comboBox1.DataSource = listloaisanpham;
            comboBox1.DisplayMember = "TenLoai";
            comboBox1.ValueMember = "MaLoai";
            comboBox1.SelectedIndex = -1; // Không chọn mục nào ban đầu
        }

        private void BindGrid(List<Sanpham> listsanpham)
        {
            dataGridView1.Rows.Clear();

            foreach (var sp in listsanpham)
            {
                int index = dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells[0].Value = sp.MaSP;
                dataGridView1.Rows[index].Cells[1].Value = sp.TenSP;
                dataGridView1.Rows[index].Cells[2].Value =
                    sp.NgayNhap.HasValue ? sp.NgayNhap.Value.ToString("dd/MM/yyyy") : "";

                dataGridView1.Rows[index].Cells[3].Value =
                    sp.LoaiSP != null ? sp.LoaiSP.TenLoai : "";
            }
        }




        public void setGridViewStyle(DataGridView dgv)
        {
            dgv.BorderStyle = BorderStyle.None;
            dgv.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.BackgroundColor = Color.White;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void ResetForm()
        {
            textBox1.Clear();
            textBox2.Clear();

            dateTimePicker1.Value = DateTime.Now;

            comboBox1.SelectedIndex = -1;

            textBox1.Enabled = true; // cho phép nhập mã mới khi thêm
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Sanpham sp = new Sanpham
                {
                    MaSP = textBox1.Text.Trim(),
                    TenSP = textBox2.Text.Trim(),
                    NgayNhap = dateTimePicker1.Value,
                    MaLoai = comboBox1.SelectedValue.ToString()
                };

                sanphamservice.InsertUpdate(sp);

                BindGrid(sanphamservice.GetAll());
                ResetForm();

                MessageBox.Show("Thêm thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Sanpham sp = new Sanpham
                {
                    MaSP = textBox1.Text.Trim(), // khóa chính
                    TenSP = textBox2.Text.Trim(),
                    NgayNhap = dateTimePicker1.Value,
                    MaLoai = comboBox1.SelectedValue.ToString()
                };

                sanphamservice.InsertUpdate(sp);

                BindGrid(sanphamservice.GetAll());
                ResetForm();

                MessageBox.Show("Sửa thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi sửa: " + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra xem người dùng đã chọn sản phẩm chưa
                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm cần xóa!");
                    return;
                }

                // Hỏi xác nhận
                DialogResult dr = MessageBox.Show("Bạn có chắc muốn xóa sản phẩm này?",
                                                  "Xác nhận",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);

                if (dr == DialogResult.Yes)
                {
                    // Gọi Delete với chuỗi MaSP
                    sanphamservice.Delete(textBox1.Text.Trim());

                    // Cập nhật lại DataGridView và reset form
                    BindGrid(sanphamservice.GetAll());
                    ResetForm();

                    MessageBox.Show("Xóa thành công!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa: " + ex.Message);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra nếu click vào header hoặc ngoài vùng dữ liệu
            if (e.RowIndex < 0) return;

            // Lấy dòng đang chọn
            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

            // Gán giá trị từ DataGridView vào TextBox, ComboBox, DateTimePicker
            textBox1.Text = row.Cells[0].Value?.ToString(); // MaSP
            textBox2.Text = row.Cells[1].Value?.ToString(); // TenSP

            // Ngày nhập
            if (DateTime.TryParse(row.Cells[2].Value?.ToString(), out DateTime ngayNhap))
            {
                dateTimePicker1.Value = ngayNhap;
            }
            else
            {
                dateTimePicker1.Value = DateTime.Now;
            }

            // Loại sản phẩm
            string maLoai = "";
            foreach (LoaiSP loai in comboBox1.DataSource as List<LoaiSP>)
            {
                if (loai.TenLoai == row.Cells[3].Value?.ToString())
                {
                    maLoai = loai.MaLoai;
                    break;
                }
            }

            comboBox1.SelectedValue = maLoai;

        }

    }
}
