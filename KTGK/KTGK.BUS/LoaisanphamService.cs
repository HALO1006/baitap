using KTGK.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTGK.BUS
{
    public class LoaisanphamService
    {
        // lấy tất cả các loại sản phẩm       
        public List<LoaiSP> GetAll()
        {
            Model1 context = new Model1();
            return context.LoaiSP.ToList();
        }
    }
}
