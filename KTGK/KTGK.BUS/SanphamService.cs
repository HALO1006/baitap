using KTGK.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTGK.BUS
{
    public class SanphamService
    {
        // lấy tất cả sản phẩm
        public List<DAL.Models.Sanpham> GetAll()
        {
            Model1 context = new Model1();
            return context.Sanpham.ToList();
        }

        public void InsertUpdate(Sanpham s)
        {
            Model1 context = new Model1();
            context.Sanpham.AddOrUpdate(s);
            context.SaveChanges();
        }

        //xóa sản phẩm
        public void Delete(string masp)
        {
            using (Model1 context = new Model1())
            {
                var sp = context.Sanpham.FirstOrDefault(x => x.MaSP == masp);
                if (sp != null)
                {
                    context.Sanpham.Remove(sp);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Sản phẩm không tồn tại!");
                }
            }
        }
    }
}