using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Listener.Helpers
{
    public delegate void OnConnectionError(ConnectionEventArgs e);

    public delegate void OnPaketSend(int paketBouyutu, int ToplamSent, int HexPaketBouyutu);

    public delegate void OnPaketGeldi(int paketBouyutu);

    public delegate void OnConnecting(ConnectionEventArgs e);

    public delegate void OnConnected(ConnectionEventArgs e);
    public enum KonsModel
    {
        MiniRSC = 0,
        RScUltra = 1,
        M3 = 2
    }
    public class AmiGlobals
    {
        public string[] HexOku(string hex)
        {
            try
            {
                //t = new FileInfo(dosyaAdi);
                //if (!t.Exists)
                //{
                //    //LogYaz(AMR_SOFTWARE.AMR_SERVER, AMR_EXCEPTION_TYPE.ERROR, new Exception(dosyaAdi + " Dosyası Bulunamadı"), dosyaAdi + " Dosyası Bulunamadı");
                //    return null;
                //}

                string[] lines = hex.Split('\n');

                for (int i = 0; i < lines.Length; i++)
                {
                    lines[i] = lines[i] + "\r";
                }

                return lines;
            }
            catch (Exception ex)
            {
                //LogYaz(AMR_SOFTWARE.AMR_SERVER, AMR_EXCEPTION_TYPE.ERROR, ex, dosyaAdi + " Dosya Okuma hatası");
                return null;
            }
        }

        public List<byte> GetHex(KonsModel model, string HexFromWeb)
        {
            List<byte> alFirmware = new List<byte>();

            string[] hexDizi = HexFromWeb.Split('\n');// HexOku(HexFromWeb);
            hexDizi[hexDizi.Length - 1] = hexDizi[hexDizi.Length - 1] + "\r";
            #region dosyadan

            //hexDizi = HexOku(dosyaAd);

            if (hexDizi != null && hexDizi.Length > 0)
            {
                if (model == KonsModel.MiniRSC || model == KonsModel.RScUltra) alFirmware = HexToList(hexDizi);
                else alFirmware = HexToListOsos(hexDizi);
            }
            else
            {
                //log.LogYaz(AMR_SOFTWARE.AMR_SERVER, AMR_EXCEPTION_TYPE.WARNING, "Versiyon data dosyadan okunamadı");
                //MessageBox.Show("Versiyon data dosyadan okunamadı");
                return null;
            }

            #endregion

            if (alFirmware.Count > 0) return alFirmware;

            return null;
        }

        public string[] HexOkuDosyadan(string dosyaAdi)
        {
            FileInfo t;

            try
            {
                t = new FileInfo(dosyaAdi);
                if (!t.Exists)
                {
                    //LogYaz(AMR_SOFTWARE.AMR_SERVER, AMR_EXCEPTION_TYPE.ERROR, new Exception(dosyaAdi + " Dosyası Bulunamadı"), dosyaAdi + " Dosyası Bulunamadı");
                    return null;
                }

                string[] lines = System.IO.File.ReadAllLines(dosyaAdi);

                for (int i = 0; i < lines.Length; i++)
                {
                    lines[i] = lines[i] + "\r";
                }

                return lines;
            }
            catch (Exception ex)
            {
                //LogYaz(AMR_SOFTWARE.AMR_SERVER, AMR_EXCEPTION_TYPE.ERROR, ex, dosyaAdi + " Dosya Okuma hatası");
                return null;
            }
        }

        public List<byte> GetHexDosya(KonsModel model, string dosyaAd)
        {
            List<byte> alFirmware = new List<byte>();

            string[] hexDizi;

            #region dosyadan

            hexDizi = HexOkuDosyadan(dosyaAd);

            if (hexDizi != null && hexDizi.Length > 0)
            {
                if (model == KonsModel.MiniRSC || model == KonsModel.RScUltra) alFirmware = HexToList(hexDizi);
                else alFirmware = HexToListOsos(hexDizi);
            }
            else
            {
                //log.LogYaz(AMR_SOFTWARE.AMR_SERVER, AMR_EXCEPTION_TYPE.WARNING, "Versiyon data dosyadan okunamadı");
                //MessageBox.Show("Versiyon data dosyadan okunamadı");
                return null;
            }

            #endregion

            if (alFirmware.Count > 0) return alFirmware;

            return null;
        }

        // Mini RSC ve ULTRA lar için
        public List<byte> HexToList(string[] hexDizi)
        {
            List<byte> alFirmware = new List<byte>();

            foreach (string h in hexDizi)
            {
                if (h != "")
                {
                    string str = h;
                    str = str.Remove(0, 1);
                    str = str.Remove(str.Length - 3, 3);

                    for (int i = 0; i < str.Length; i += 2)
                    {
                        string bb;
                        bb = str.Substring(i, 2);
                        byte ss = byte.Parse(bb, System.Globalization.NumberStyles.HexNumber);
                        alFirmware.Add(ss);
                    }
                }
            }

            return alFirmware;
        }
        //M3 için
        public List<byte> HexToListOsos(string[] hexDizi)
        {
            List<byte> alFirmware = new List<byte>();

            foreach (string h in hexDizi)
            {
                if (h != "")
                {
                    string str = h;
                    str = str.Remove(0, 1);
                    str = str.Remove(str.Length - 1, 1);

                    alFirmware.Add(0X3A);

                    for (int i = 0; i < str.Length; i += 2)
                    {
                        string bb;
                        bb = str.Substring(i, 2);
                        byte ss = byte.Parse(bb, System.Globalization.NumberStyles.HexNumber);
                        alFirmware.Add(ss);
                    }
                }
            }

            return alFirmware;
        }

        public void DosyaKaydet(string dosyaAd, string bilgi)
        {
            try
            {
                if (File.Exists(dosyaAd)) File.Delete(dosyaAd);

                StreamWriter w = File.AppendText(dosyaAd);
                w.WriteLine(bilgi);
                w.Flush();
                w.Close();

            }
            catch (Exception ex)
            {

            }
        }

        public void DosyaSil(string dosyaAd)
        {
            try
            {
                if (File.Exists(dosyaAd)) File.Delete(dosyaAd);
            }
            catch (Exception ex)
            {

            }
        }

        public ArrayList DosyaOku(string dosyaAdi)
        {
            ArrayList al;
            FileInfo t;

            try
            {
                al = new ArrayList();
                t = new FileInfo(dosyaAdi);
                if (!t.Exists)
                {
                    return null;
                }

                StreamReader Tex = t.OpenText();
                while (!Tex.EndOfStream)
                {
                    al.Add(Tex.ReadLine());
                }

                Tex.Close();
                return al;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
