using System;
using System.Linq;

namespace project
{
    internal class TheTu
    {

        private string iD;
        private string pin;
        private bool daKhoa;

        public string ID
        {
            get => iD;
            set => iD = IsValidID(value) ? value : "00000000000000";
        }

        public string Pin
        {
            get => pin;
            set => pin = IsValidPIN(value) ? value : "000000";
        }
        public bool DaKhoa { get => daKhoa; set => daKhoa = value; }

        public TheTu(string id, string pin)
        {
            ID = id;
            Pin = pin;
            daKhoa = false;
        }
        public static void ghiFile(string nameFile, List<TheTu> list)
        {
            using (StreamWriter sw = new StreamWriter(nameFile))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    sw.WriteLine(list[i].ToString());
                }
            }
        }
        public static List<TheTu> docFile(string nameFile)
        {
            List<TheTu> list = new List<TheTu>();
            if (!File.Exists(nameFile)) return list;
            using (StreamReader sr = new StreamReader(nameFile))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    string[] arr = line.Split('-');
                    if (arr.Length < 2) continue;

                    list.Add(new TheTu(arr[0], arr[1]));
                }
            }
            return list;
        }
        public static bool IsValidID(string value)
        {
           return value.Length == 14 && value.All(char.IsDigit);
        }

        public static bool IsValidPIN(string value)
        {
            return value.Length == 6 && value.All(char.IsDigit);
        }

        public override string? ToString()
        {
            return $"{ID}-{Pin}";
        }
    }
}
