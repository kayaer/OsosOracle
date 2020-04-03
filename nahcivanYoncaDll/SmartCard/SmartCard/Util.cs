using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace SmartCard
{
    internal class Util
    {
        // todo vvvvvvv
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mesaj"></param>
        /// <param name="UyariParam">B:Bilgilendirme, Q:Soru, S:Stop, W:Warning, E:Error</param>
        public void Uyar(string mesaj, string UyariParam)
        {
            if (UyariParam == "B") MessageBox.Show(mesaj, "INFO", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
            else if (UyariParam == "Q") MessageBox.Show(mesaj, "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            else if (UyariParam == "S") MessageBox.Show(mesaj, "STOP", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            else if (UyariParam == "W") MessageBox.Show(mesaj, "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            else if (UyariParam == "E") MessageBox.Show(mesaj, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            else MessageBox.Show(mesaj);
        }
        /// <summary>
        /// İnternet Bağlantı Kontrolü
        /// İnternet bağlantısı varsa True yoksa False döner
        /// </summary>
        /// <returns>bool</returns>
        public bool NetKontrol(string url)
        {
            try
            {
                System.Net.Sockets.TcpClient clnt = new System.Net.Sockets.TcpClient(url, 80);
                clnt.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public bool SayiKontrol(string sayi)
        {
            System.Text.RegularExpressions.Regex allow = new System.Text.RegularExpressions.Regex("^[0-9.]*$");
            if (!allow.IsMatch(sayi)) return false;
            else return true;
        }

        public string Encode(string toEncode)
        {
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            return System.Convert.ToBase64String(toEncodeAsBytes);
        }

        public string Decode(string encodedData)
        {
            byte[] encodedDataAsBytes = System.Convert.FromBase64String(encodedData);
            return System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes, 0, encodedDataAsBytes.Length);
        }

        public DateTime strToDate(string str, DateTime defaultValue)
        {
            try
            {
                int iYil = objToInt(str.Substring(0, 4), -1);
                int iAy = objToInt(str.Substring(4, 2), -1);
                int iGun = objToInt(str.Substring(6, 2), -1);

                return new DateTime(iYil, iAy, iGun);
            }
            catch (Exception ex)
            {
                return defaultValue;
            }
        }

        public int objToInt(object obj, int defaultValue)
        {
            try
            {
                return Convert.ToInt32(obj);
            }
            catch
            {
                return defaultValue;
            }
        }

        public UInt32 SendAboneCsc(UInt32 dn, UInt32 alfa)
        {
            UInt32 carry;
            UInt32 rmask, pmask, qmask;
            UInt32 t1, t2, t3, t4;
            UInt32 sr;
            UInt32 i;

            pmask = 1 << 1;
            rmask = 1 << 2;
            qmask = 1 << 3;

            sr = dn;
            sr = sr + alfa;
            if (sr == 0) sr = 1;
            for (i = 0; i < 16; i++)
            {
                if ((sr & pmask) != 0) t1 = 1; else t1 = 0;
                if ((sr & rmask) != 0) t2 = 1; else t2 = 0;
                if ((sr & qmask) != 0) t3 = 1; else t3 = 0;
                if ((sr & 0x8000) != 0) t4 = 1; else t4 = 0;
                carry = t1 ^ t2 ^ t3 ^ t4;
                sr <<= 1;
                if (carry != 0)
                {
                    sr |= 1;
                }
            }
            return sr;
        }
        public void Sleep(int time)
        {
            System.Threading.Thread.Sleep(time);
        }

        public string TipTanimlama(string IssuerArea, string zone)
        {
            string stIssuer = zone;

            if (stIssuer + "A " == IssuerArea)
            {
                IssuerArea = "Abone Karti";
                return IssuerArea;
            }
            if (stIssuer + "YA" == IssuerArea)
            {
                IssuerArea = "Yetki Açma Karti";
                return IssuerArea;
            }
            if (stIssuer + "YE" == IssuerArea)
            {
                IssuerArea = "Yetki Bilgi Karti";
                return IssuerArea;
            }
            if (stIssuer + "YK" == IssuerArea)
            {
                IssuerArea = "Yetki Kapama";
                return IssuerArea;
            }
            if (stIssuer + "YS" == IssuerArea)
            {
                IssuerArea = "Yetki Saat";
                return IssuerArea;
            }

            switch (IssuerArea)
            {
                case "ASUS":
                    IssuerArea = "Üretim Sıfırlama";
                    break;
                case "ASUR":
                    IssuerArea = "Üretim Reel";
                    break;
                case "ASUT":
                    IssuerArea = "Üretim Test";
                    break;
                case "ASUA":
                    IssuerArea = "Üretim Açma";
                    break;
                case "ASUK":
                    IssuerArea = "Üretim Kapama";
                    break;
                case "ASUF":
                    IssuerArea = "Üretim Format";
                    break;
                case "ASUZ":
                    IssuerArea = "Üretim Cihaz No";
                    break;
                case "ASUI":
                    IssuerArea = "Zone Kartı";
                    break;
                case "BTLD":
                    IssuerArea = "BootLoader";
                    break;
                case "\0\0\0\0":
                    IssuerArea = "Boş Kart";
                    break;
            }
            return IssuerArea;

        }
    }
}
