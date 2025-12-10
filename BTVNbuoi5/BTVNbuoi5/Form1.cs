using BTVNbuoi5.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace BTVNbuoi5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            try
            {
               Model1 context = new Model1();

                // Lấy danh sách khoa
                List<Faculty> listFacultys = context.Faculty.ToList();

                // Lấy danh sách sinh viên
                List<Student> listStudents = context.Student.ToList();

                // Đổ dữ liệu vào combobox
                FillFacultyCombobox(listFacultys);

                // Đổ dữ liệu vào DataGridView / ListView
                BindGrid(listStudents);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //Hàm binding list có tên hiện thị là tên khoa, giá trị là Mã khoa
        private void FillFacultyCombobox(List<Faculty> listFaculties)
        {
            this.comboBox1.DataSource = listFaculties;
            this.comboBox1.DisplayMember = "FacultyName"; // Hiển thị tên khoa
            this.comboBox1.ValueMember = "FacultyID";     // Giá trị là ID khoa
        }

        // Hàm đổ dữ liệu sinh viên vào DataGridView
        private void BindGrid(List<Student> listStudents)
        {
            dataGridView1.Rows.Clear();  // Xóa hết dòng cũ

            foreach (var stu in listStudents)
            {
                int index = dataGridView1.Rows.Add(); // Tạo dòng mới

                dataGridView1.Rows[index].Cells[0].Value = stu.StudentID;
                dataGridView1.Rows[index].Cells[1].Value = stu.FullName;

                // Kiểm tra khoa tránh lỗi null Reference
                dataGridView1.Rows[index].Cells[2].Value =
                    stu.Faculty != null ? stu.Faculty.FacultyName : "Không có";

                dataGridView1.Rows[index].Cells[3].Value = stu.AverageScore;
            }
        }

        private void ResetForm()
        {
            txtMaSV.Clear();
            txtHoTen.Clear();
            txtDiemTB.Clear();
            comboBox1.SelectedIndex = 0;
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                txtMaSV.Text = row.Cells[0].Value.ToString();
                txtHoTen.Text = row.Cells[1].Value.ToString();
                txtDiemTB.Text = row.Cells[3].Value.ToString();

                // Chọn khoa trong combobox
                comboBox1.Text = row.Cells[2].Value.ToString();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string id = txtMaSV.Text.Trim();
            string name = txtHoTen.Text.Trim();
            string gpaText = txtDiemTB.Text.Trim();

            // kiểm tra bắt buộc
            if (id == "" || name == "" || gpaText == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            // kiểm tra MSSV
            if (id.Length != 10)
            {
                MessageBox.Show("Mã số sinh viên phải có 10 kí tự!");
                return;
            }

            float gpa;
            if (!float.TryParse(gpaText, out gpa))
            {
                MessageBox.Show("Điểm không hợp lệ!");
                return;
            }

            try
            {
                Model1 context = new Model1();

                // kiểm tra trùng MSSV
                Student existing = context.Student.FirstOrDefault(s => s.StudentID == id);
                if (existing != null)
                {
                    MessageBox.Show("Mã sinh viên đã tồn tại!");
                    return;
                }

                // thêm mới
                Student stu = new Student()
                {
                    StudentID = id,
                    FullName = name,
                    AverageScore = gpa,
                    FacultyID = (int)comboBox1.SelectedValue
                };

                context.Student.Add(stu);
                context.SaveChanges();

                MessageBox.Show("Thêm mới dữ liệu thành công!");

                // load lại grid
                BindGrid(context.Student.ToList());

                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string id = txtMaSV.Text.Trim();
            string name = txtHoTen.Text.Trim();
            string gpaText = txtDiemTB.Text.Trim();

            if (id == "" || name == "" || gpaText == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            if (id.Length != 10)
            {
                MessageBox.Show("Mã số sinh viên phải có 10 kí tự!");
                return;
            }

            float gpa;
            if (!float.TryParse(gpaText, out gpa))
            {
                MessageBox.Show("Điểm không hợp lệ!");
                return;
            }

            try
            {
                Model1 context = new Model1();

                Student stu = context.Student.FirstOrDefault(s => s.StudentID == id);
                if (stu == null)
                {
                    MessageBox.Show("Không tìm thấy MSSV cần sửa!");
                    return;
                }

                stu.FullName = name;
                stu.AverageScore = gpa;
                stu.FacultyID = (int)comboBox1.SelectedValue;

                context.SaveChanges();

                MessageBox.Show("Cập nhật dữ liệu thành công!");

                BindGrid(context.Student.ToList());
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string id = txtMaSV.Text.Trim();

            if (id == "")
            {
                MessageBox.Show("Bạn chưa nhập MSSV cần xóa!");
                return;
            }

            try
            {
                Model1 context = new Model1();

                Student stu = context.Student.FirstOrDefault(s => s.StudentID == id);
                if (stu == null)
                {
                    MessageBox.Show("Không tìm thấy MSSV cần xóa!");
                    return;
                }

                DialogResult dr = MessageBox.Show("Bạn có chắc muốn xóa?",
                    "Xác nhận", MessageBoxButtons.YesNo);

                if (dr == DialogResult.Yes)
                {
                    context.Student.Remove(stu);
                    context.SaveChanges();

                    MessageBox.Show("Xóa sinh viên thành công!");

                    BindGrid(context.Student.ToList());
                    ResetForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Bạn có chắc muốn thoát chương trình không?",
                "Xác nhận thoát",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close();  // Đóng form
            }
        }
    }
}
