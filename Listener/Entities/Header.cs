using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listener.Entities
{
    [Serializable]
    public class Header
    {
        public string StrPaket = string.Empty;
        public string Zone = "";
        public string ProjeKod;
        public string KonsSeriNo = string.Empty;
        public string Versiyon = string.Empty;
        public string Tip = string.Empty;
        public string State = string.Empty;
        public string RandomSayi = string.Empty;
        public string TipStr = string.Empty;
        public string TarihSaat = string.Empty;
        public string RSSI = string.Empty;
        public string Analog = "";

        public Header() { }

        public Header(string strPaket)
        {
            StrPaket = strPaket;
            string[] k = strPaket.Split(':');
            if (k.Length == 8)
            {
                Zone = "X";
                ProjeKod = k[0];
                KonsSeriNo = k[1];
                Versiyon = k[2];
                Tip = k[3]; //1:RS, 2:PL, 3:RF
                State = k[4];
                RandomSayi = k[5];
                if (Tip == "1") TipStr = "RS";
                if (Tip == "2") TipStr = "PL";
                if (Tip == "3") TipStr = "RF";

                string[] tT = k[6].Split('-');

                TarihSaat = tT[0] + "." + tT[1] + "." + tT[2] + " " + tT[3] + ":" + tT[4];

                if (tT.Length == 6) TarihSaat += ":" + tT[5];

                RSSI = k[7];

                Analog = "0";
            }
            else if (k.Length == 10)
            {
                Zone = k[0];
                ProjeKod = k[1];
                KonsSeriNo = k[2];
                Versiyon = k[3];
                Tip = k[4]; //1:RS, 2:PL, 3:RF
                State = k[5];
                RandomSayi = k[6];
                if (Tip == "1") TipStr = "RS";
                if (Tip == "2") TipStr = "PL";
                if (Tip == "3") TipStr = "RF";

                string[] tT = k[7].Split('-');

                TarihSaat = tT[0] + "." + tT[1] + "." + tT[2] + " " + tT[3] + ":" + tT[4];

                if (tT.Length == 6) TarihSaat += ":" + tT[5];

                RSSI = k[8];

                Analog = k[9];
            }
        }

        public Header(byte[] bytePaket)
        {
            byte[] paket = new byte[bytePaket.Length - 4];
            Buffer.BlockCopy(bytePaket, 2, paket, 0, bytePaket.Length - 4);

            StrPaket = BytesTostr(paket);
            string[] k = StrPaket.Split(':');

            if (k.Length == 8)
            {
                Zone = "X";
                ProjeKod = k[0];
                KonsSeriNo = k[1];
                Versiyon = k[2];
                Tip = k[3]; //1:RS, 2:RF, 3:PL
                State = k[4];
                RandomSayi = k[5];

                if (Tip == "1") TipStr = "RS";
                if (Tip == "2") TipStr = "RF";
                if (Tip == "3") TipStr = "PL";

                string[] tT = k[6].Split('-');

                TarihSaat = tT[0] + "." + tT[1] + "." + tT[2] + " " + tT[3] + ":" + tT[4];

                if (tT.Length == 6) TarihSaat += ":" + tT[5];

                RSSI = k[7];

                Analog = k[8];
            }
            else if (k.Length == 10)
            {
                Zone = k[0];
                ProjeKod = k[1];
                KonsSeriNo = k[2];
                Versiyon = k[3];
                Tip = k[4]; //1:RS, 2:RF, 3:PL
                State = k[5];
                RandomSayi = k[6];
                if (Tip == "1") TipStr = "RS";
                if (Tip == "2") TipStr = "RF";
                if (Tip == "3") TipStr = "PL";

                string[] tT = k[7].Split('-');

                TarihSaat = tT[0] + "." + tT[1] + "." + tT[2] + " " + tT[3] + ":" + tT[4];

                if (tT.Length == 6) TarihSaat += ":" + tT[5];

                RSSI = k[8];

                Analog = k[9];
            }
        }

        public string BytesTostr(byte[] data)
        {
            string str = string.Empty;

            foreach (byte b in data)
            {
                str += Convert.ToChar(b).ToString();
            }

            return str;
        }

        public override string ToString()
        {
            string str = StrPaket + "\r\n" +
              "Zone: " + Zone + "\r\n" +
              "Proje Kod: " + ProjeKod + "\r\n" +
              "Seri No: " + KonsSeriNo + "\r\n" +
              "Versiyon: " + Versiyon + "\r\n" +
              "Tip: " + TipStr + "\r\n" +
              "Durum: " + State + "\r\n" +
              "Tarih: " + TarihSaat + "\r\n" +
              "Rssi: " + RSSI + "\r\n" +
              "Analog: " + Analog;

            return str;
        }
    }
}
