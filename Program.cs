using System;

namespace project
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();

            List<Admin> list = Admin.LayDL();
            List<TheTu> listTheTu = TheTu.docFile("TheTu.txt");
            Admin.ghiFile("Admin.txt", list);

            if (LoginAdmin(list) != null)
            {
                string key = "";
                do
                {
                    Console.Clear();
                    LoginSuccess();
                    Console.Write("\t\t\tBan chon chuc nang so: ");
                    key = Console.ReadLine();
                    switch (key)
                    {
                        case "1":
                            foreach (TheTu item in listTheTu)
                            {
                                Console.WriteLine(item);
                            }
                            Console.ReadKey();
                            break;

                        case "2":
                            NhapTheTu(listTheTu);
                            break;
                    }
                } while (key != "5");
            }
        }
        
        static void NhapTheTu(List<TheTu> listTheTu)
        {
            bool isSucces = false;
            Random random = new Random();
            string id = "", pin = "123456";

            while (!isSucces)
            {

                for (int i = 0; i < 14; i++)
                {
                    id += random.Next(0, 10).ToString(); 
                }

                bool isDuplicate = false;
                foreach (TheTu item in listTheTu)
                {
                    if (item.ID.Equals(id))
                    {
                        isDuplicate = true;
                        break;
                    }
                }
                if (!isDuplicate)
                {
                    isSucces = true;
                }
            }

            listTheTu.Add(new TheTu(id, pin));
            TheTu.ghiFile("TheTu.txt", listTheTu);//update the tu

            Console.Write("\t\t\tNhap ten tai khoan:");
            string tenTK = Console.ReadLine();
            Console.Write("\t\t\tNhap so du:");
            decimal.TryParse(Console.ReadLine(), out decimal soDu);
            Console.Write("\t\t\tNhap loai tien te");
            string loaiTien = Console.ReadLine();
            TaiKhoan tk = new TaiKhoan(id, tenTK, soDu, loaiTien);
            TaiKhoan.TaoVaCapNhatFileTaiKhoan(tk);

        }


        static Admin LoginAdmin(List<Admin> list)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\t\t\t***************************************");
            Console.Write("\t\t\t*          ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("DANG NHAP ADMIN");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("            *");
            Console.WriteLine("\t\t\t***************************************");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\t\t\tUser :");
            string user = Console.ReadLine();
            Console.Write("\t\t\tPin  :");
            string pin = InputPin();
            Admin adLogin = Admin.CheckLogin(user, pin, list);
            if (adLogin == null)
            {
                Console.WriteLine("\t\t\tThong tin dang nhap khong hop le!!");
                return null;
            }
            else
            {
                Console.WriteLine("\t\t\tDang nhap thanh cong!!!");
                Console.ReadKey();
                return adLogin;
            }
        }
        static string InputPin()
        {
            string pin = "";
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    pin += key.KeyChar;
                    Console.Write("*");
                }
                if (key.Key == ConsoleKey.Backspace && pin.Length > 0)
                {
                    pin = pin.Substring(0, pin.Length - 1);
                    Console.Write("\b \b");
                }
            } while (key.Key != ConsoleKey.Enter);
            Console.WriteLine();
            return pin;
        }
        static void LoginSuccess()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\t\t\t*********************************MENU**************************************");
            Console.WriteLine("\t\t\t1.Xem danh sach tai khoan");
            Console.WriteLine("\t\t\t2.Them tai khoan");
            Console.WriteLine("\t\t\t3.Xoa tai khoan");
            Console.WriteLine("\t\t\t4.Mo khoa tai khoan");
            Console.WriteLine("\t\t\t5.Thoat");
            Console.WriteLine("\t\t\t***************************************************************************");
        }


        static string NameLoGo(String name)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            return name;

        }
    }
}
