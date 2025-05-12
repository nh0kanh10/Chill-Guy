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
            using (StreamWriter writer = new StreamWriter(tenFile))
            {
                writer.WriteLine(tk.ToString());
            }
        }
        public static TaiKhoan DocFileTaiKhoan(string nameFile)
        {
            if (!File.Exists(nameFile)) return null;

            using (StreamReader sr = new StreamReader(nameFile))
            {
                string line = sr.ReadLine();
                if (!string.IsNullOrWhiteSpace(line))
                {
                    string[] arr = line.Split('-');
                    if (arr.Length == 4)
                    {
                        string id = arr[0];
                        string tenTK = arr[1];
                        if (decimal.TryParse(arr[2], out decimal soDuTK))
                        {
                            string loaiTienTe = arr[3];
                            return new TaiKhoan(id, tenTK, soDuTK, loaiTienTe);
                        }
                    }
                }
            }

            return null;
        }
        public TaiKhoan()
        {
        }


        public override string? ToString()
        {
            return $"{iD}-{tenTK}-{soDuTK:N0}-{loaiTienTe}";
        }


    }
}
