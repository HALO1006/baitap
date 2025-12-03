using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTVNbuoi4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            comboBoxKhoa.SelectedItem = "QTKD";   // Giá trị mặc định

            // giới tính nữ được chọn mặc định
            radioNu.Checked = true;

            // --- Thêm dữ liệu sẵn ---
            int rowIndex;

            rowIndex = dataGridView1.Rows.Add();
            DataGridViewRow row = dataGridView1.Rows[rowIndex];
            row.Cells["MSSV"].Value = "SV001";
            row.Cells["HoTen"].Value = "Nguyen Van A";
            row.Cells["Khoa"].Value = "QTKD";
            row.Cells["GioiTinh"].Value = "Nam";
            row.Cells["DiemTB"].Value = 7.5;

            rowIndex = dataGridView1.Rows.Add();
            row = dataGridView1.Rows[rowIndex];
            row.Cells["MSSV"].Value = "SV002";
            row.Cells["HoTen"].Value = "Tran Thi B";
            row.Cells["Khoa"].Value = "CNTT";
            row.Cells["GioiTinh"].Value = "Nữ";
            row.Cells["DiemTB"].Value = 8.2;

            // Cập nhật tổng Nam/Nữ
            CapNhatTongSV();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra thông tin bắt buộc
            if (string.IsNullOrWhiteSpace(txtMaSV.Text) ||
                string.IsNullOrWhiteSpace(txtHoTen.Text) ||
                string.IsNullOrWhiteSpace(txtDiemTB.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra điểm TB hợp lệ
            if (!double.TryParse(txtDiemTB.Text, out double diemTB))
            {
                MessageBox.Show("Điểm trung bình phải là số!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            // 2. Kiểm tra MSSV đã có trong DataGridView chưa
            bool exists = false;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["MSSV"].Value != null && row.Cells["MSSV"].Value.ToString() == txtMaSV.Text)
                {
                    exists = true;

                    // Cập nhật dữ liệu
                    row.Cells["HoTen"].Value = txtHoTen.Text;
                    row.Cells["Khoa"].Value = comboBoxKhoa.SelectedItem.ToString();
                    row.Cells["GioiTinh"].Value = radioNam.Checked ? "Nam" : "Nữ";
                    row.Cells["DiemTB"].Value = diemTB;

                    MessageBox.Show("Cập nhật dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                }
            }

            // 3. Nếu chưa có MSSV -> thêm mới
            if (!exists)
            {
                int rowIndex = dataGridView1.Rows.Add();
                DataGridViewRow row = dataGridView1.Rows[rowIndex];

                row.Cells["MSSV"].Value = txtMaSV.Text;
                row.Cells["HoTen"].Value = txtHoTen.Text;
                row.Cells["Khoa"].Value = comboBoxKhoa.SelectedItem.ToString();
                row.Cells["GioiTinh"].Value = radioNam.Checked ? "Nam" : "Nữ";
                row.Cells["DiemTB"].Value = diemTB;

                MessageBox.Show("Thêm mới dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // 4. Cập nhật tổng Nam/Nữ
            CapNhatTongSV();

            // 5. Reset form nếu cần
            txtMaSV.Clear();
            txtHoTen.Clear();
            txtDiemTB.Clear();
            comboBoxKhoa.SelectedIndex = 0;
            radioNu.Checked = true;

        }

        private void txtMaSV_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string maSVCanXoa = txtMaSV.Text.Trim();

            // 1. Kiểm tra nhập MSSV
            if (string.IsNullOrEmpty(maSVCanXoa))
            {
                MessageBox.Show("Vui lòng nhập MSSV cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Tìm MSSV trong DataGridView
            DataGridViewRow rowCanXoa = null;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["MSSV"].Value != null && row.Cells["MSSV"].Value.ToString() == maSVCanXoa)
                {
                    rowCanXoa = row;
                    break;
                }
            }

            // 3. Nếu không tìm thấy MSSV
            if (rowCanXoa == null)
            {
                MessageBox.Show("Không tìm thấy MSSV cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 4. Hiển thị cảnh báo YES/NO
            DialogResult result = MessageBox.Show($"Bạn có chắc muốn xóa sinh viên MSSV: {maSVCanXoa}?",
                                                  "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                dataGridView1.Rows.Remove(rowCanXoa);
                MessageBox.Show("Xóa sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Reset TextBox nếu cần
                txtMaSV.Clear();
            }

            CapNhatTongSV();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra người dùng không click vào header
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                txtMaSV.Text = row.Cells["MSSV"].Value?.ToString();
                txtHoTen.Text = row.Cells["HoTen"].Value?.ToString();
                txtDiemTB.Text = row.Cells["DiemTB"].Value?.ToString();

                comboBoxKhoa.SelectedItem = row.Cells["Khoa"].Value?.ToString();

                string gioiTinh = row.Cells["GioiTinh"].Value?.ToString();
                if (gioiTinh == "Nam")
                    radioNam.Checked = true;
                else
                    radioNu.Checked = true;
            }
        }

        private void CapNhatTongSV()
        {
            int tongNam = 0;
            int tongNu = 0;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["GioiTinh"].Value != null)
                {
                    string gt = row.Cells["GioiTinh"].Value.ToString();
                    if (gt == "Nam")
                        tongNam++;
                    else if (gt == "Nữ")
                        tongNu++;
                }
            }

            lblTongNam.Text = tongNam.ToString();
            lblTongNu.Text = tongNu.ToString();
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            CapNhatTongSV();
        }

        private void dataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            CapNhatTongSV();
        }

    }
}
