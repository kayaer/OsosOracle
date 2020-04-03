using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace SmartCard
{
    internal class classDes
    {
        //public static extern void FonksiyonAdi(paramatreler);
        //[DllImport("Des", EntryPoint = "EncryptDes")]
        //private static extern void EncryptDes(byte[] Key, byte[] InData, byte[] OutData);

        //[DllImport("Des", EntryPoint = "DecryptDes")]
        //private static extern void DecryptDes(byte[] Key, byte[] InData, byte[] OutData);

        public static byte[] TripleDes(byte[] AnahtarDeger, byte[] KriptolanacakData, byte[] KriptolananData)
        {
            byte[] AnahtarDeger0007 = { AnahtarDeger[0], AnahtarDeger[1], AnahtarDeger[2], AnahtarDeger[3], 
                                        AnahtarDeger[4], AnahtarDeger[5], AnahtarDeger[6], AnahtarDeger[7] };
            byte[] AnahtarDeger0815 = { AnahtarDeger[8], AnahtarDeger[9], AnahtarDeger[10], AnahtarDeger[11], 
                                        AnahtarDeger[12], AnahtarDeger[13], AnahtarDeger[14], AnahtarDeger[15] };

            TrDes.EncryptDes(AnahtarDeger0007, KriptolanacakData, ref KriptolananData);
            TrDes.DecryptDes(AnahtarDeger0815, KriptolananData, ref KriptolanacakData);
            TrDes.EncryptDes(AnahtarDeger0007, KriptolanacakData, ref KriptolananData);

            return KriptolananData;
        }
    }

    public class TrDes
    {
        static byte[] IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };

        public static void EncryptDes(byte[] Key, byte[] InData, ref byte[] OutData)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Mode = CipherMode.ECB;
            des.Padding = PaddingMode.Zeros;
            ICryptoTransform cEncryptor = des.CreateEncryptor(Key, IV);
            OutData = cEncryptor.TransformFinalBlock(InData, 0, InData.Length);

        }

        public static void DecryptDes(byte[] Key, byte[] InData, ref byte[] OutData)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Mode = CipherMode.ECB;
            des.Padding = PaddingMode.Zeros;
            ICryptoTransform cDecryptor = des.CreateDecryptor(Key, IV);
            OutData = cDecryptor.TransformFinalBlock(InData, 0, InData.Length);
        }
    }
}
