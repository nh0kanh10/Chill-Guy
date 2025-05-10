using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project
{
    internal class TaiKhoan
    {
        //ID, ten tài khoản,số dư tài khoản, loại tiền tệ
        private string iD;
        private string tenTK;
        private decimal soDuTK;
        private string loaiTienTe;

        public string ID { get => iD; set => iD = value; }
        public string TenTK { get => tenTK; set => tenTK = value; }
        public decimal SoDuTK { get => soDuTK; set => soDuTK = value; }
        public string LoaiTienTe { get => loaiTienTe; set => loaiTienTe = value; }

        public TaiKhoan(string iD, string tenTK, decimal soDuTK, string loaiTienTe)
        {
            ID = iD;
            TenTK = tenTK;
            SoDuTK = soDuTK;
            LoaiTienTe = loaiTienTe;
        }
        public static void TaoVaCapNhatFileTaiKhoan(TaiKhoan tk)
        {
            string tenFile = $"{tk.ID}.txt";

            string thongTinTK = $"Tên tài khoản: {tk.TenTK}\nSố dư tài khoản: {tk.SoDuTK}\nLoại tiền tệ: {tk.LoaiTienTe}";

            using (StreamWriter writer = new StreamWriter(tenFile))
            {
                writer.WriteLine(thongTinTK);
            }
        }

        public TaiKhoan()
        {
        }
        
        public override string? ToString()
        {
            return $"{iD}-{tenTK}-{soDuTK}-{loaiTienTe}";
        }

        
    }
}
