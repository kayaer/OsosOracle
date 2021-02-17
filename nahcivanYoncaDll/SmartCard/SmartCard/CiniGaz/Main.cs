using SmartCard.CiniGaz.Global;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace SmartCard.CiniGaz
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class Main
    {
        private CardProcess cp = new CardProcess();

        /// <summary>
        /// Kart Tipi Bilgisinin Çağırıldığı Fonksiyon
        /// </summary>
        /// <returns>String Veri Tipinde Geri Dönüş Yapar.  Parser Exm:("\n" Satır, "|" Sütun)</returns>
        public string KartTipi()
        {
            return cp.KartTipi();
        }

        /// <summary>
        /// Abonenin Kart İçerisindeki Tüm Bilgilerinin Çağırıldığı Fonksiyon
        /// </summary>
        /// <returns>String Veri Tipinde Geri Dönüş Yapar. Parser Exm:("\n" Satır, "|" Sütun)</returns>
        public string AboneOku()
        {
            return cp.AboneOku();
        }

        /// <summary>
        /// Boş Bir Kartı Abone Kartı Yapmak İçin Çağırılan Fonksiyon
        /// </summary>
        /// <param name="cihazNo">Cihaz No</param>
        /// <param name="aboneNo">Abone No</param>
        /// <param name="kartNo">Kart No</param>
        /// <param name="cap">Çap</param>
        /// <param name="tip">Tip</param>
        /// <param name="donem">Dönem</param>
        /// <param name="fiyat1">Fiyat 1</param>
        /// <param name="fiyat2">Fiyat 2</param>
        /// <param name="fiyat3">Fiyat 3</param>
        /// <param name="limit1">Limit 1</param>
        /// <param name="limit2">Limit 2</param>
        /// <param name="bayram1Gunn">1. Bayram İçin Ayın Günü</param>
        /// <param name="bayram1Ayy">1. Bayram İçin Yılın Ayı</param>
        /// <param name="bayram1Suree">1. Bayramın Süresi</param>
        /// <param name="bayram2Gunn">2. Bayram İçin Ayın Günü</param>
        /// <param name="bayram2Ayy">2. Bayram İçin Yılın Ayı</param>
        /// <param name="bayram2Suree">2. Bayramın Süresi</param>
        /// <param name="avansOnayi">Hafta Sonu Avans Onayı</param>
        /// <returns>Enums.ResultRC Tipinde Geri Dönüş Yapar.</returns>
        public int AboneYap(UInt32 cihazNo, UInt32 aboneNo, byte kartNo,
                            byte cap, byte tip, byte donem,
                            UInt32 fiyat1, UInt32 fiyat2, UInt32 fiyat3,
                            UInt32 limit1, uint limit2,
                            byte bayram1Gunn, byte bayram1Ayy, byte bayram1Suree,
                            byte bayram2Gunn, byte bayram2Ayy, byte bayram2Suree,
                            byte avansOnayi)
        {
            return cp.AboneYap(cihazNo, aboneNo, kartNo, cap, tip, donem, fiyat1, fiyat2, fiyat3, limit1, limit2, bayram1Gunn, bayram1Ayy, bayram1Suree, bayram2Gunn, bayram2Ayy, bayram2Suree, avansOnayi);
        }

        /// <summary>
        /// Abonenin Kart İçerisindeki Kredi Bilgilerinin Çağırıldığı Fonksiyon
        /// </summary>
        /// <returns>String Veri Tipinde Geri Dönüş Yapar. Parser Exm:("\n" Satır, "|" Sütun)</returns>
        public string KrediOku()
        {
            return cp.KrediOku();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cihazNo">Cihaz No</param>
        /// <param name="anaKredi">Ana Kredi</param>
        /// <param name="yedekKredi">Yedek Kredi</param>
        /// <param name="cap">Çap</param>
        /// <param name="donem">Dönem</param>
        /// <param name="fiyat1">Fiyat 1</param>
        /// <param name="fiyat2">Fiyat 2</param>
        /// <param name="fiyat3">Fiyat 3</param>
        /// <param name="limit1">Limit 1</param>
        /// <param name="limit2">Limit 2</param>
        /// <param name="bayram1Gunn">1. Bayram İçin Ayın Günü</param>
        /// <param name="bayram1Ayy">1. Bayram İçin Yılın Ayı</param>
        /// <param name="bayram1Suree">1. Bayramın Süresi</param>
        /// <param name="bayram2Gunn">2. Bayram İçin Ayın Günü</param>
        /// <param name="bayram2Ayy">2. Bayram İçin Yılın Ayı</param>
        /// <param name="bayram2Suree">2. Bayramın Süresi</param>
        /// <param name="avansOnayi">Hafta Sonu Avans Onayı</param>
        /// <returns>Enums.ResultRC Tipinde Geri Dönüş Yapar.</returns>
        public int KrediYaz(UInt32 cihazNo, Int32 anaKredi, Int32 yedekKredi,
                            byte cap, byte donem,
                            UInt32 fiyat1, UInt32 fiyat2, UInt32 fiyat3,
                            UInt32 limit1, uint limit2,
                            byte bayram1Gunn, byte bayram1Ayy, byte bayram1Suree,
                            byte bayram2Gunn, byte bayram2Ayy, byte bayram2Suree,
                            byte avansOnayi)
        {
            return cp.KrediYaz(cihazNo, anaKredi, yedekKredi, cap, donem, fiyat1, fiyat2, fiyat3, limit1, limit2, bayram1Gunn, bayram1Ayy, bayram1Suree, bayram2Gunn, bayram2Ayy, bayram2Suree, avansOnayi);
        }

        /// <summary>
        /// Abone Kartını Boş Kart Haline Getiren Fonksion
        /// </summary>
        /// <returns>Enums.ResultRC Tipinde Geri Dönüş Yapar.</returns>
        public int AboneBosalt()
        {
            return cp.AboneBosalt();
        }
    }
}
