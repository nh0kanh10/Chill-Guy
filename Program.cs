using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace project
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            List<Admin> listAD = Admin.LayDL();
            List<TheTu> listTheTu = TheTu.docFile("TheTu.txt");
            Admin.ghiFile("Admin.txt", listAD);

            int chonDN;
            do
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\t\t\t****************** ATM **********************\n");
                Console.WriteLine("\t\t\tBạn chọn đăng nhập với tư cách:");
                Console.WriteLine("\t\t\t1. ADMIN");
                Console.WriteLine("\t\t\t2. NGƯỜI DÙNG");
                Console.WriteLine("\t\t\t3. Thoát!!!\n");
                Console.WriteLine("\t\t\t*******************************************");
                Console.ResetColor();
                Console.Write("\t\t\tBạn chọn chức năng số: ");
                int.TryParse(Console.ReadLine(), out chonDN);

                switch (chonDN)
                {
                    case 1:
                        DNADMIN(listAD, listTheTu);
                        break;
                    case 2:
                        DNUSER(listTheTu);
                        break;
                    case 3:
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("\t\t\t****************** ATM **********************\n");
                        Console.ResetColor();
                        Console.WriteLine($"{"HẸN GẶP LẠI !!!",53}\n");
                        Console.WriteLine("\t\t\t*********************************************");
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\t\t\tChức năng không hợp lệ!");
                        Console.ResetColor();
                        break;
                }

            } while (chonDN != 3);
        }
        static void DoiMaPin(TheTu theTu, List<TheTu> listTheTu)
        {
            Console.Clear();
            Console.WriteLine("\t\t\t*************** ĐỔI MÃ PIN ***************");

            Console.WriteLine("\t\t\tNhập mã PIN hiện tại: ");
            Console.Write("\t\t\t");
            string maPinCu = Console.ReadLine();

            if (maPinCu != theTu.Pin)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t\t\tMã PIN cũ không đúng. Hủy thao tác.");
                Console.ResetColor();
                Console.ReadKey();
                return;
            }

            string maPinMoi1, maPinMoi2;

            while (true)
            {
                Console.Write("\t\t\tNhập mã PIN mới (6 chữ số): ");
                maPinMoi1 = InputPin(6);

                Console.Write("\t\t\tNhập lại mã PIN mới để xác nhận: ");
                maPinMoi2 = InputPin(6);

                if (maPinMoi1 != maPinMoi2)
                {
                    Console.WriteLine("\t\t\tHai mã PIN không khớp. Vui lòng thử lại.");
                }
                else
                {
                    break;
                }
            }

            theTu.Pin = maPinMoi1;
            TheTu.ghiFile("TheTu.txt", listTheTu);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\t\t\tĐổi mã PIN thành công!");
            Console.ResetColor();
            Console.ReadKey();
        }
        static void XemLichSuGiaoDich(TaiKhoan taiKhoan)
        {
            Console.Clear();
            Console.WriteLine("\t\t\t*************** LỊCH SỬ GIAO DỊCH ***************");
            Stack<LichSuGiaoDich> DSlichSu = LichSuGiaoDich.DocLichSuGD($"[LichSu{taiKhoan.ID}].txt");

            if (DSlichSu == null || DSlichSu.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\t\t\tKhông có lịch sử giao dịch nào.");
                Console.ResetColor();
            }
            else
            {
                foreach (LichSuGiaoDich item in DSlichSu)
                {
                    Console.WriteLine($"\t\t\tMã ID: {item.ID}");
                    Console.WriteLine($"\t\t\tThời gian giao dịch: [{item.ThoiGianGD:dd/MM/yyyy HH:mm:ss}] - {item.LoaiGiaoDich}");
                    Console.WriteLine($"\t\t\tSố tiền: {item.SoTienGiaoDich:N0} VND");
                }
            }

            Console.WriteLine("\n\t\t\tNhấn phím bất kỳ để quay lại...");
            Console.ReadKey();
        }
        static void ChuyenTien(TaiKhoan taiKhoanChuyen, List<TheTu> listTheTu)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\t\t\t******************CHUYỂN TIỀN*********************");
                Console.WriteLine("\t\t\tNhập số tài khoản người nhận: ");
                Console.Write("\t\t\t");
                string idNhan = Console.ReadLine();

                TheTu theTuNhan = TheTu.TimTheTu(idNhan, listTheTu);
                if (theTuNhan != null)
                {
                    TaiKhoan taiKhoaNhan = TaiKhoan.DocFileTaiKhoan(theTuNhan.ID);

                    if (taiKhoaNhan == null || taiKhoaNhan.ID == taiKhoanChuyen.ID)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\t\t\tTài khoản người nhận không hợp lệ hoặc trùng với tài khoản hiện tại.");
                        Console.ResetColor();
                        Console.ReadKey();
                    }
                    else
                    {

                        decimal soTienChuyen = NhapSoTienHopLe(taiKhoanChuyen.SoDuTK, "chuyển");

                        Console.WriteLine($"\t\t\tXác nhận chuyển {soTienChuyen:N0} VND đến tài khoản {taiKhoaNhan.ID} - {taiKhoaNhan.TenTK} (Y/N): ");
                        Console.Write("\t\t\t");
                        string xacNhan = Console.ReadLine().Trim().ToUpper();
                        if (xacNhan != "Y")
                        {
                            Console.WriteLine("\t\t\tGiao dịch đã hủy.");

                        }
                        else
                        {
                            taiKhoanChuyen.SoDuTK -= soTienChuyen;
                            taiKhoaNhan.SoDuTK += soTienChuyen;

                            TaiKhoan.TaoVaCapNhatFileTaiKhoan(taiKhoanChuyen);
                            TaiKhoan.TaoVaCapNhatFileTaiKhoan(taiKhoaNhan);

                            LichSuGiaoDich.ThemLichSuGiaoDich(new LichSuGiaoDich(taiKhoanChuyen.ID, $"Chuyển tiền đến {taiKhoaNhan.ID}", soTienChuyen, DateTime.Now));
                            LichSuGiaoDich.ThemLichSuGiaoDich(new LichSuGiaoDich(taiKhoaNhan.ID, $"Nhận tiền từ  {taiKhoanChuyen.ID}", soTienChuyen, DateTime.Now));
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\t\t\tChuyển tiền thành công!");
                            Console.ResetColor();
                        }
                    }

                }
                else Console.WriteLine("\t\t\tKhông tìm thấy tài khoản bạn muốn chuyển khoản");
                Console.WriteLine("\t\t\tBạn có muốn tiếp tục giao dịch không ");
                Console.WriteLine("\t\t\t1.Tiếp tục giao dịch");
                Console.WriteLine("\t\t\t2.Thoát!!!");
                string tiepTucGD = Console.ReadLine();
                if (tiepTucGD == "2")
                {
                    return;
                }
                else if (tiepTucGD != "2" && tiepTucGD != "1")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\t\t\tLựa chọn không hợp lệ. Vui lòng thử lại.");
                    Console.ResetColor();
                }
            }
        }
        static void RutXeng(TaiKhoan taiKhoan)
        {
            while (true)
            {
                Console.Clear();
                decimal tienCanRut = 0;
                decimal soDu = taiKhoan.SoDuTK;
                string luaChon = "";

                Console.WriteLine("\t\t\t*******************************************");
                Console.WriteLine("\t\t\tChọn số tiền cần rút:");
                Console.WriteLine("\t\t\t1. 50.000 VND");
                Console.WriteLine("\t\t\t2. 100.000 VND");
                Console.WriteLine("\t\t\t3. 200.000 VND");
                Console.WriteLine("\t\t\t4. 500.000 VND");
                Console.WriteLine("\t\t\t5. 1.000.000 VND");
                Console.WriteLine("\t\t\t6. 2.000.000 VND");
                Console.WriteLine("\t\t\t7. 3.000.000 VND");
                Console.WriteLine("\t\t\t8. 5.000.000 VND");
                Console.WriteLine("\t\t\t9. Số khác");
                Console.WriteLine("\t\t\t*******************************************");
                Console.Write("\t\t\tBạn chọn: ");

                luaChon = Console.ReadLine();

                switch (luaChon)
                {
                    case "1": tienCanRut = 50000; break;
                    case "2": tienCanRut = 100000; break;
                    case "3": tienCanRut = 200000; break;
                    case "4": tienCanRut = 500000; break;
                    case "5": tienCanRut = 1000000; break;
                    case "6": tienCanRut = 2000000; break;
                    case "7": tienCanRut = 3000000; break;
                    case "8": tienCanRut = 5000000; break;
                    case "9": tienCanRut = NhapSoTienHopLe(soDu, "rút"); break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\t\t\tLựa chọn không hợp lệ. Vui lòng thử lại.");
                        Console.ResetColor();
                        return;
                }


                Console.WriteLine($"\t\t\tXác nhận rút {tienCanRut:N0}VND khỏi tài khoản (Y/N): ");
                Console.Write("\t\t\t");
                string xacNhan = Console.ReadLine().Trim().ToUpper();
                if (xacNhan != "Y")
                {
                    Console.WriteLine("\t\t\tGiao dịch đã hủy.");
                }
                else
                {
                    soDu -= tienCanRut;
                    taiKhoan.SoDuTK = soDu;
                    TaiKhoan.TaoVaCapNhatFileTaiKhoan(taiKhoan);
                    LichSuGiaoDich.ThemLichSuGiaoDich(new LichSuGiaoDich(taiKhoan.ID, "Rút tiền", tienCanRut, DateTime.Now));

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\t\t\tGiao dịch rút tiền thành công!");
                    Console.ResetColor();
                }

                Console.WriteLine("\t\t\tBạn có muốn tiếp tục giao dịch không ");
                Console.WriteLine("\t\t\t1.Tiếp tục giao dịch");
                Console.WriteLine("\t\t\t2.Thoát!!!");
                string tiepTucGD = Console.ReadLine();
                if (tiepTucGD == "2")
                {
                    return;
                }
                else if (tiepTucGD != "2" && tiepTucGD != "1")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\t\t\tLựa chọn không hợp lệ. Vui lòng thử lại.");
                    Console.ResetColor();
                }
            }
        }
        static decimal NhapSoTienHopLe(decimal soDuHienTai, string loaiGD)
        {
            while (true)
            {
                Console.WriteLine($"\t\t\tNhập số tiền cần {loaiGD} (bội số của 50.000): ");
                Console.Write("\t\t\t");
                if (decimal.TryParse(Console.ReadLine(), out decimal soTien))
                {
                    if (soTien < 50000)
                        Console.WriteLine($"\t\t\tSố tiền cần {loaiGD} phải từ 50.000 trở lên.");
                    else if (soTien % 50000 != 0)
                        Console.WriteLine($"\t\t\tSố tiền cần {loaiGD} phải là bội số của 50.000.");
                    else if (soTien > soDuHienTai - 50000)
                        Console.WriteLine("\t\t\tKhông đủ tiền (phải giữ lại ít nhất 50.000).");
                    else
                        return soTien;
                }
                else Console.WriteLine("\t\t\tGiá trị không hợp lệ. Vui lòng nhập lại.");
            }
        }
        static void DNUSER(List<TheTu> listTheTu)
        {
            TheTu user = LoginUSER(listTheTu);
            if (user == null) return;
            TaiKhoan tk = TaiKhoan.DocFileTaiKhoan($"{user.ID}.txt");
            if (tk == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t\t\tKhông thể đọc thông tin tài khoản.");
                Console.ResetColor();
                return;
            }
            string key;
            do
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\t\t\t************** USER MENU ***************");
                Console.ResetColor();
                Console.WriteLine("\t\t\t1. Xem thông tin tài khoản");
                Console.WriteLine("\t\t\t2. Rút tiền");
                Console.WriteLine("\t\t\t3. Chuyển tiền");
                Console.WriteLine("\t\t\t4. Xem lịch sử giao dịch");
                Console.WriteLine("\t\t\t5. Đổi mã Pin");
                Console.WriteLine("\t\t\t6. Thoát");
                Console.Write("\t\t\tBạn chọn: ");
                key = Console.ReadLine();

                switch (key)
                {
                    case "1":
                        ShowAccountInfo(tk);
                        break;
                    case "2":
                        RutXeng(tk);
                        break;
                    case "3":
                        ChuyenTien(tk, listTheTu);
                        break;
                    case "4":
                        XemLichSuGiaoDich(tk);
                        break;
                    case "5":
                        DoiMaPin(user,listTheTu);
                        break;
                    case "6":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("\t\t\tBạn đã chọn thoát.");
                        Console.ResetColor();
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\t\t\tChức năng không hợp lệ!");
                        Console.ResetColor();
                        break;
                }
                Console.ReadKey();
                if (key == "6")
                    Console.WriteLine("\t\t\tNhấn phím bất kỳ để tiếp tục...");

            } while (key != "6");
        }

        static void DNADMIN(List<Admin> listAD, List<TheTu> listTheTu)
        {
            Admin admin = LoginAdmin(listAD);
            if (admin == null) return;

            string key;
            do
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\t\t\t************** ADMIN MENU ***************");
                Console.ResetColor();
                Console.WriteLine("\t\t\t1. Xem danh sách thẻ");
                Console.WriteLine("\t\t\t2. Thêm thẻ");
                Console.WriteLine("\t\t\t3. Xóa thẻ");
                Console.WriteLine("\t\t\t4. Mở khóa thẻ");
                Console.WriteLine("\t\t\t5. Thoát");
                Console.Write("\t\t\tBạn chọn: ");
                key = Console.ReadLine();

                switch (key)
                {
                    case "1": InDSTheTu(listTheTu); break;
                    case "2": AddCard(listTheTu); break;
                    case "3": RemoveCard(listTheTu); break;
                    case "4": OpenLock(listTheTu); break;
                    case "5":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("\t\t\tBạn đã chọn thoát.");
                        Console.ResetColor();
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\t\t\tChức năng không hợp lệ!");
                        Console.ResetColor();
                        break;
                }
                Console.ReadKey();
                if (key == "5")
                    Console.WriteLine("\t\t\tNhấn phím bất kỳ để tiếp tục...");

            } while (key != "5");
        }

        static TheTu LoginUSER(List<TheTu> list)
        {
            while (true)
            {
                Console.Clear();
                TieuDe("ĐĂNG NHẬP USER");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("\t\t\tID: ");
                Console.ResetColor();
                string userId = Console.ReadLine();

                TheTu card = TheTu.TimTheTu(userId, list);
                if (card == null)
                {
                    if (!LuaChon("ID thẻ không tồn tại.")) return null;
                    continue;
                }
                if (card.DaKhoa)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\t\t\tThẻ của bạn đã bị khóa.");
                    Console.ResetColor();
                    return null;
                }

                if (DangNhapUserPin(card, 3, list)) return card;
                else return null;
            }
        }

        static Admin LoginAdmin(List<Admin> list)
        {
            while (true)
            {
                Console.Clear();
                TieuDe("ĐĂNG NHẬP ADMIN");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("\t\t\tUser: ");
                string user = Console.ReadLine();
                Console.Write("\t\t\tPIN : ");
                string pin = InputPin(6);
                Console.ResetColor();

                Admin ad = Admin.CheckLoginADMIN(user, pin, list);
                if (ad != null)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\t\t\tĐăng nhập thành công!");
                    Console.ResetColor();
                    return ad;
                }
                if (!LuaChon("Thông tin đăng nhập không hợp lệ.")) return null;
            }
        }
        static void OpenLock(List<TheTu> listTheTu)
        {
            if (listTheTu == null || listTheTu.Count == 0)
            {
                Console.WriteLine("\t\t\tDanh sách thẻ trống, không có thẻ để mở khóa!!!");
                return;
            }
            string iD = "";
            while (true)
            {
                Console.WriteLine("\t\t\tNhập ID thẻ từ bạn muốn mở khóa: ");
                Console.Write("\t\t\t");
                iD = Console.ReadLine();
                if (TheTu.IsValidID(iD)) break;
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;

                    if (!LuaChon("Mã ID bạn nhập không hợp lệ!!!")) return;
                    Console.ResetColor();
                }
            }
            TheTu theMuonMo = null;
            for (int i = 0; i < listTheTu.Count; i++)
            {
                if (listTheTu[i].ID.Equals(iD))
                {
                    theMuonMo = listTheTu[i];
                    break;
                }
            }
            if (theMuonMo != null)
            {
                if (theMuonMo.DaKhoa == false)
                {
                    Console.WriteLine("\t\t\tThẻ không bị khóa.");
                }
                else
                {
                    theMuonMo.DaKhoa = false;
                    Console.WriteLine("\t\t\tMở khóa thành công!!!");
                    TheTu.ghiFile("TheTu.txt", listTheTu);
                }
            }
            else
            {
                Console.WriteLine("\t\t\tKhông tìm thấy thẻ có ID bạn muốn mở khóa");
            }
        }
        static void RemoveCard(List<TheTu> listTheTu)
        {
            if (listTheTu == null || listTheTu.Count == 0)
            {
                Console.WriteLine("\t\t\tDanh sách thẻ trống, không có thẻ để xóa!!!");
                return;
            }
            string iD = "";
            while (true)
            {
                Console.WriteLine("\t\t\tNhập ID thẻ từ bạn muốn xoá: ");
                Console.Write("\t\t\t");
                iD = Console.ReadLine();
                if (TheTu.IsValidID(iD)) break;
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    if (!LuaChon("Mã ID bạn nhập không hợp lệ!!!")) return;
                    Console.ResetColor();
                }
            }
            string fileName = "";
            bool isFind = false;
            for (int i = 0; i < listTheTu.Count; i++)
            {
                if (listTheTu[i].ID.Equals(iD))
                {
                    isFind = true;
                    fileName = listTheTu[i].ID;
                    listTheTu.Remove(listTheTu[i]);
                    Console.WriteLine($"\t\t\tĐã xóa thẻ từ thành công.");
                    break;
                }
            }
            if (isFind)
            {
                if (File.Exists($"{fileName}.txt"))
                {
                    File.Delete($"{fileName}.txt");
                    Console.WriteLine($"\t\t\tĐã xóa file {fileName}.txt thành công.");
                }
                else
                {
                    Console.WriteLine($"\t\t\tFile {fileName}.txt không tồn tại.");
                }
                TheTu.ghiFile("TheTu.txt", listTheTu);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t\t\tKhông tìm thấy ID bạn muốn xóa trong danh sách thẻ từ");
                Console.ResetColor();
            }
        }
        static void AddCard(List<TheTu> listTheTu)
        {
            bool isSucces = false;
            Random random = new Random();
            string id = "", pin = "123456";

            while (!isSucces)
            {
                id = "";
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
            TheTu.ghiFile("TheTu.txt", listTheTu);
            TaiKhoan tk;
            while (true)
            {
                Console.Clear();
                Console.Write("\t\t\tNhập tên tài khoản: ");
                string tenTK = Console.ReadLine();
                Console.Write("\t\t\tNhập số dư: ");
                decimal.TryParse(Console.ReadLine(), out decimal soDu);
                Console.Write("\t\t\tNhập loại tiền tệ: ");
                string loaiTien = Console.ReadLine();
                tk = new TaiKhoan(id, tenTK, soDu, loaiTien);
                Console.WriteLine("\t\t\tBạn có chắc muốn lưu tài khoản ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\t\t\t" + tk);
                Console.ResetColor();
                Console.WriteLine("\t\t\t1. Có");
                Console.WriteLine("\t\t\t2. Không");
                Console.Write("\t\t\tBạn chọn: ");
                string luaChon = Console.ReadLine();
                if (luaChon == "1") break;
                else if (luaChon == "2") Console.WriteLine("\t\t\tMời bạn nhập lại thông tin tài khoản");
                else Console.WriteLine("\t\t\tPhím bạn chọn không hợp lệ!!!");
            }
            TaiKhoan.TaoVaCapNhatFileTaiKhoan(tk);
        }
        static void ShowAccountInfo(TaiKhoan tk)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\t\t\t***** THÔNG TIN TÀI KHOẢN *****");
            Console.ResetColor();
            Console.WriteLine($"\t\t\tID: {tk.ID}");
            Console.WriteLine($"\t\t\tTên TK: {tk.TenTK}");
            Console.WriteLine($"\t\t\tSố dư: {tk.SoDuTK:N0} {tk.LoaiTienTe}");
            Console.WriteLine("\t\t\t*******************************");
        }

        static bool DangNhapUserPin(TheTu card, int soLanThu, List<TheTu> listTheTu)
        {
            int daThu = 0;
            while (daThu < soLanThu)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("\t\t\tPIN: ");
                Console.ResetColor();
                string pin = InputPin(6);

                if (card.Pin == pin)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\t\t\tĐăng nhập thành công!");
                    Console.ResetColor();
                    return true;
                }
                daThu++;
                int soLanConLai = soLanThu - daThu;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\t\t\tPIN sai! Còn {soLanConLai} lần thử.");
                Console.ResetColor();
            }
            card.DaKhoa = true;
            TheTu.ghiFile("TheTu.txt", listTheTu);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\t\t\tBạn đã nhập sai quá nhiều. Thẻ đã bị khóa.");
            Console.ResetColor();
            return false;
        }

        static bool LuaChon(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\t\t\t{message}");
            Console.ResetColor();
            Console.WriteLine("\t\t\t1. Thử lại   \n\t\t\t2. Thoát");
            Console.Write("\t\t\tBạn chọn: ");
            return Console.ReadLine() == "1";
        }

        static void TieuDe(string title)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\t\t\t***************************************");
            Console.Write("\t\t\t*          ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(title);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("            *");
            Console.WriteLine("\t\t\t***************************************");
            Console.ResetColor();
        }

        static string InputPin(int maxLength)
        {
            string pin = "";
            ConsoleKeyInfo key;

            while (true)
            {
                key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Enter)
                {
                    if (TheTu.IsValidPIN(pin)) break;
                    else
                    {
                        Console.WriteLine("\n\t\t\tMã PIN không hợp lệ. Vui lòng nhập lại.");
                        pin = "";
                        Console.Write("\t\t\t");
                        continue;
                    }
                }

                if (key.Key == ConsoleKey.Backspace && pin.Length > 0)
                {
                    pin = pin.Substring(0, pin.Length - 1);
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(key.KeyChar) && pin.Length < maxLength)
                {
                    pin += key.KeyChar;
                    Console.Write("*");
                }
            }

            Console.WriteLine();
            return pin;
        }

        static void InDSTheTu(List<TheTu> list)
        {
            if (list == null || list.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t\t\tDanh sách thẻ trống!!!");
                Console.ResetColor();
                return;
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\t\t\t{0,-15}{1,-20}{2,-10}", "ID", "PIN", "KHOÁ");
            Console.ResetColor();
            foreach (var item in list)
            {
                Console.WriteLine("\t\t\t{0,-15}{1,-20}{2,-10}", item.ID, item.Pin, item.DaKhoa);
            }
        }
    }
}
