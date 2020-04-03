using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualBasic;

namespace SmartCard
{
    public class Hexcon
    {
        public static string BytetoString(byte Index, byte Boyut, params byte[] GonderilenVeriler)
        {
            string Deger = "";

            for (int i = Index; i < Index + Boyut; i++)
            {
                Deger += Convert.ToChar(GonderilenVeriler[i]).ToString();
            }

            return Deger;
        }

        public static byte[] StringToByte(string InString)
        {
            string[] ByteStrings;
            ByteStrings = InString.Split("-".ToCharArray());
            byte[] ByteOut;
            ByteOut = new byte[ByteStrings.Length];
            for (int i = 0; i < ByteStrings.Length; i++)
            {
                ByteOut[i] = byte.Parse(ByteStrings[i], System.Globalization.NumberStyles.HexNumber);
            }
            return ByteOut;
        }

        public static string TarihOlustur(byte[] tarih)
        {
            int gun = 0;
            byte temp = (byte)((int)0X0F & (int)tarih[0]);
            int ay = (byte)temp;

            byte tempGun = (byte)(((int)0XF0 & (int)tarih[0]) >> 4);

            temp = (byte)((int)0X01 & (int)tarih[1]);
            if (temp == 1) gun = tempGun + 16; else gun = tempGun;

            temp = (byte)(((int)0XFE & (int)tarih[1]) >> 1);

            int yil = (int)temp;

            return gun + "." + ay + "." + yil;
        }
        
        public struct ByteToBit
        {

            public int Bit1, Bit2, Bit3, Bit4, Bit5, Bit6, Bit7, Bit8;
            public int Deger;
            public ByteToBit(int Alinacak)
            {
                Bit1 = Alinacak & 1;
                Bit2 = (Alinacak & 2) >> 1;
                Bit3 = (Alinacak & 4) >> 2;
                Bit4 = (Alinacak & 8) >> 3;
                Bit5 = (Alinacak & 16) >> 4;
                Bit6 = (Alinacak & 32) >> 5;
                Bit7 = (Alinacak & 64) >> 6;
                Bit8 = (Alinacak & 128) >> 7;

                Deger = Alinacak;
            }

        }

        public static UInt32 Byte4toUInt32(byte[] OkunacakDizi)
        {
            return (UInt32)((OkunacakDizi[0] << 24) + (OkunacakDizi[1] << 16) + (OkunacakDizi[2] << 8) + OkunacakDizi[3]);

        }

        public static UInt32 Byte4toUInt32(byte bir, byte iki, byte uc, byte dort)
        {
            return (UInt32)((dort << 24) + (uc << 16) + (iki << 8) + bir);

        }

        public static void UInt32toByte4(UInt32 Deger, byte[] YazilacakDizi)
        {
            YazilacakDizi[3] = Convert.ToByte((Deger & 0xFF000000) >> 24);
            YazilacakDizi[2] = Convert.ToByte((Deger & 0x00FF0000) >> 16);
            YazilacakDizi[1] = Convert.ToByte((Deger & 0x0000FF00) >> 8);
            YazilacakDizi[0] = Convert.ToByte((Deger & 0x000000FF));
        }

        public static void UInt16toByte2(UInt16 Deger, byte[] YazilacakDizi)
        {
            YazilacakDizi[1] = Convert.ToByte((Deger & 0x0000FF00) >> 8);
            YazilacakDizi[0] = Convert.ToByte((Deger & 0x000000FF));
        }

        /// <summary>
        /// Sayı bitlerini orlama ile set eder
        /// </summary>
        /// <param name="set edilecek sayı"></param>
        /// <param name="hata ve major bitlerini set etmek için kullanılır major için 48, hata için 128 "></param>
        /// <returns>bitleri değiştirilmiş sayı </returns>
        public static int SetBitBir(int sayi, int deger) //major bitini set etmek için kullanılır (3)
        {
            sayi |= deger;
            return sayi;
        }
        public static int SetBitZero(int sayi) //hata bitini set etmek için kullanılır 
        {
            sayi &= 0X7F;
            return sayi;
        }

        public static Int32 ByteToDecimal(byte[] InBytes, int okunacak) //dev no 4 byte 
        {

            string StringOut = "";
            for (int i = okunacak - 1; 0 <= i; i--)
            {
                StringOut = StringOut + String.Format("{0:X2}", InBytes[i]);
            }

            return Int32.Parse(StringOut, System.Globalization.NumberStyles.HexNumber);
        }

        public static byte[] ByteDiziOlustur(byte[] dizi, int index, int adet)
        {
            byte[] temp = new byte[4];
            Array.Copy(dizi, index, temp, 0, adet);
            return temp;
        }

        public static byte[] CharToByteArray(char ch)
        {
            byte c = (byte)ch;
            byte[] dizi = new byte[1];
            dizi[0] = c;
            return dizi;
        }

        public static byte[] ByteToByteArray(byte b)
        {
            byte[] dizi = new byte[1];
            dizi[0] = b;
            return dizi;
        }

        public void BufferTemizle(byte[] dizi)
        {
            for (int i = 0; i < dizi.Length; i++)
            {
                dizi[i] = 0;
            }
        }

        //public void UInt32to4byte(UInt32 deger, byte[] dizi)
        //{
        //    dizi[3] = (byte)(deger % (256 * 256 * 256));
        //    deger = deger / 256;
        //    dizi[2] = (byte)(deger % (256 * 256));
        //    deger = deger / 256;
        //    dizi[1] = (byte)(deger % 256);
        //    dizi[0] = (byte)(deger / 256);
        //}
    }

    public struct Integer2Byte
    {
        public byte bir, iki;
        public UInt16 value;
        public Integer2Byte(UInt16 Alinacak)
        {
            bir = Convert.ToByte(Alinacak & 0xFF);
            iki = Convert.ToByte((Alinacak & 0xFF00) >> 8);
            value = Alinacak;
        }

        public Integer2Byte(byte Veri1, byte Veri2)
        {
            bir = Veri1;
            iki = Veri2;
            value = Convert.ToUInt16(Veri2 * 256 + Veri1);
        }

        public Integer2Byte(byte Veri1, byte Veri2, byte Exor)
        {
            bir = Convert.ToByte(Veri1 ^ Exor);
            iki = Convert.ToByte(Veri2 ^ Exor);
            value = Convert.ToUInt16(iki * 256 + bir);
        }

    }

    public struct Integer4Byte
    {
        public byte bir, iki, uc, dort;
        public UInt32 value;
        public Int32 valuee;

        public Integer4Byte(UInt32 Alinacak)
        {
            bir = Convert.ToByte(Alinacak & 0xFF);
            iki = Convert.ToByte((Alinacak & 0xFF00) >> 8);
            uc = Convert.ToByte((Alinacak & 0xFF0000) >> 16);
            dort = Convert.ToByte((Alinacak & 0xFF000000) >> 24);

            value = Alinacak;
            valuee = 0;
        }

        public Integer4Byte(Int32 Alinacak)
        {
            bir = Convert.ToByte(Alinacak & 0xFF);
            iki = Convert.ToByte((Alinacak & 0xFF00) >> 8);
            uc = Convert.ToByte((Alinacak & 0xFF0000) >> 16);
            dort = Convert.ToByte((Alinacak & 0xFF000000) >> 24);

            valuee = Alinacak;
            value = 0;
        }

        public Integer4Byte(UInt16 Veri1, UInt16 Veri2)
        {
            bir = Convert.ToByte((Veri1 & 0xFF00) >> 8);
            iki = Convert.ToByte(Veri1 & 0xFF);

            uc = Convert.ToByte((Veri2 & 0xFF00) >> 8);
            dort = Convert.ToByte(Veri2 & 0xFF);

            value = (UInt32)(dort * 16777216 + uc * 65536 + iki * 256 + bir);
            valuee = 0;
        }

        public Integer4Byte(Int16 Veri1, Int16 Veri2)
        {
            bir = Convert.ToByte((Veri1 & 0xFF00) >> 8);
            iki = Convert.ToByte(Veri1 & 0xFF);

            uc = Convert.ToByte((Veri2 & 0xFF00) >> 8);
            dort = Convert.ToByte(Veri2 & 0xFF);

            valuee = (dort * 16777216 + uc * 65536 + iki * 256 + bir);
            value = 0;
        }

    }

    public class TarihAl
    {
        public byte bir, iki, uc, dort;
        public int gun, ay, yil, saat, dakika;

        public TarihAl(DateTime IstenenTarih)
        {
            byte bGun, bAy, bYil;

            bGun = Convert.ToByte(DateAndTime.Day(IstenenTarih));
            bAy = Convert.ToByte(DateAndTime.Month(IstenenTarih));
            bYil = Convert.ToByte(DateAndTime.Year(IstenenTarih) - 2000);

            bir = Convert.ToByte(((bGun & 0xF) << 4) + bAy);
            iki = Convert.ToByte((bYil << 1) + ((bGun & 0xF0) >> 4));

            uc = Convert.ToByte(DateAndTime.Hour(IstenenTarih));
            dort = Convert.ToByte(DateAndTime.Minute(IstenenTarih));

            gun = 0;
            ay = 0;
            yil = 0;
            saat = uc;
            dakika = dort;

        }

        public TarihAl(string Tarih)
        {
            DateTime IstenenTarih = Convert.ToDateTime(Tarih);
            byte bGun, bAy, bYil;

            bGun = Convert.ToByte(DateAndTime.Day(IstenenTarih));
            bAy = Convert.ToByte(DateAndTime.Month(IstenenTarih));
            bYil = Convert.ToByte(DateAndTime.Year(IstenenTarih) - 2000);

            bir = Convert.ToByte(((bGun & 0xF) << 4) + bAy);
            iki = Convert.ToByte((bYil << 1) + ((bGun & 0xF0) >> 4));

            uc = Convert.ToByte(DateAndTime.Hour(IstenenTarih));
            dort = Convert.ToByte(DateAndTime.Minute(IstenenTarih));

            gun = 0;
            ay = 0;
            yil = 0;
            saat = uc;
            dakika = dort;

        }

        public TarihAl(byte[] Tarih)
        {
            bir = Tarih[0];
            iki = Tarih[1];
            if (Tarih.Length > 2)
            {
                uc = Tarih[2];
                dort = Tarih[3];
            }
            gun = 0;
            byte temp = (byte)((int)0X0F & (int)Tarih[0]);
            ay = (byte)temp;

            byte tempGun = (byte)((int)0X0F & ((int)Tarih[0] >> 4));

            temp = (byte)((int)0X01 & (int)Tarih[1]);
            if (temp == 1) gun = tempGun + 16; else gun = tempGun;

            temp = (byte)((int)0XEF & ((int)Tarih[1] >> 1));

            yil = (int)temp;

            if (Tarih.Length > 2)
            {
                saat = Tarih[2];
                dakika = Tarih[3];
            }
        }

        public byte[] TarihDondur4Byte(DateTime IstenenTarih)
        {
            byte bGun, bAy, bYil;

            bGun = Convert.ToByte(DateAndTime.Day(IstenenTarih));
            bAy = Convert.ToByte(DateAndTime.Month(IstenenTarih));
            bYil = Convert.ToByte(DateAndTime.Year(IstenenTarih) - 2000);

            bir = Convert.ToByte(((bGun & 0xF) << 4) + bAy);
            iki = Convert.ToByte((bYil << 1) + ((bGun & 0xF0) >> 4));

            uc = Convert.ToByte(DateAndTime.Hour(IstenenTarih));
            dort = Convert.ToByte(DateAndTime.Minute(IstenenTarih));

            gun = 0;
            ay = 0;
            yil = 0;
            saat = uc;
            dakika = dort;

            return new byte[4] { bir, iki, uc, dort };

        }

        public string ToString()
        {
            return gun + "." + ay + "." + yil;
        }
    }
}
