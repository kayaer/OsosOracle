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
    }
}
