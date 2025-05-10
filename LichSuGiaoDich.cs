using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project
{
    internal class LichSuGiaoDich
    {
        //ID,loại giao dịch, số tiền giao dịch, thời gian giao dịch.
        private string iD;
        private string loaiGiaoDich;
        private decimal soTienGiaoDich;
        private DateTime thoiGianGD;

        public LichSuGiaoDich(string iD, string loaiGiaoDich, decimal soTienGiaoDich, DateTime thoiGianGD)
        {
            ID = iD;
            LoaiGiaoDich = loaiGiaoDich;
            SoTienGiaoDich = soTienGiaoDich;
            ThoiGianGD = thoiGianGD;
        }

        public string ID { get => iD; set => iD = value; }
        public string LoaiGiaoDich { get => loaiGiaoDich; set => loaiGiaoDich = value; }
        public decimal SoTienGiaoDich { get => soTienGiaoDich; set => soTienGiaoDich = value; }
        public DateTime ThoiGianGD { get => thoiGianGD; set => thoiGianGD = value; }

        public static void ThemLichSuGiaoDich(LichSuGiaoDich ls)
        {
            using (StreamWriter sw = new StreamWriter($"[LichSu{ls.ID}].txt",append:true))
            {
                sw.WriteLine(ls.ToString());
            }
        }

        public override string? ToString()
        {
            return $"{ID}-{loaiGiaoDich}-{soTienGiaoDich}-{thoiGianGD}";
        }

        public static Stack<LichSuGiaoDich> DocLichSuGD(string nameFile)
        {
            Stack<LichSuGiaoDich> list = new Stack<LichSuGiaoDich>();
            if (!File.Exists(nameFile)) return list;
            using (StreamReader sr = new StreamReader("Admin.txt"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] arr = line.Split('-');
                    decimal.TryParse(arr[2], out decimal soTienGD);
                    DateTime.TryParse(arr[3], out DateTime thoiGian);
                    list.Append(new LichSuGiaoDich(arr[0], arr[1], soTienGD, thoiGian));
                }
            }
            return list;
        }
    }
}



    


