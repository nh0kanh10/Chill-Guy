using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project
{
    internal class Admin
    {
        private string user;
        private string password;

        public string User { get => user; set => user = value; }
        public string Password { get => password; set => password = value; }

        public Admin(string user, string password)
        {
            User = user;
            Password = password;
        }
        public Admin() { }

        public override string? ToString()
        {
            return $"{User}-{Password}";
        }

        // doc  danh sach admin tu file 
        public static List<Admin> docFile(string nameFile)
        {
            List<Admin> list = new List<Admin>();
            if (!File.Exists(nameFile)) return list;
            using (StreamReader sr = new StreamReader("Admin.txt"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    string[] arr = line.Split('-');
                    if (arr.Length < 2) continue;

                    list.Add(new Admin(arr[0], arr[1]));
                }
            }
            return list;
        }
        // ghi lai danh sach admin vao file
        public static void ghiFile(string nameFile, List<Admin> list)
        {
            using (StreamWriter sw = new StreamWriter(nameFile))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    sw.WriteLine(list[i].ToString());
                }
            }
        }
        //check xem ten dang nhap co trong danh sach admin khong va co dung mat khau khong
        public static Admin CheckLoginADMIN(string user, string pin, List<Admin> list)
        {
            foreach (Admin item in list)
            {
                if (user.Equals(item.User) && pin.Equals(item.Password))
                {
                    return item; 
                }
            }
            return null;
        }
        public static List<Admin> LayDL()
        {
            List<Admin> list = new List<Admin>();
            list.Add(new Admin("NguyenVanA", "123456"));
            list.Add(new Admin("TranThiB", "098765"));
            list.Add(new Admin("DinhVanC", "102938"));
            return list;
        }
    }
}
