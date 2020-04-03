using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace SmartCard
{
    class classDes
    {
        //public static extern void FonksiyonAdi(paramatreler);
        [DllImport("Des", EntryPoint = "EncryptDes")]
        private static extern void EncryptDes(byte[] Key, byte[] InData, byte[] OutData);

        [DllImport("Des", EntryPoint = "DecryptDes")]
        private static extern void DecryptDes(byte[] Key, byte[] InData, byte[] OutData);

        public static byte[] TripleDes(byte[] AnahtarDeger, byte[] KriptolanacakData, byte[] KriptolananData)
        {
            byte[] AnahtarDeger0007 = { AnahtarDeger[0], AnahtarDeger[1], AnahtarDeger[2], AnahtarDeger[3], 
                                        AnahtarDeger[4], AnahtarDeger[5], AnahtarDeger[6], AnahtarDeger[7] };
            byte[] AnahtarDeger0815 = { AnahtarDeger[8], AnahtarDeger[9], AnahtarDeger[10], AnahtarDeger[11], 
                                        AnahtarDeger[12], AnahtarDeger[13], AnahtarDeger[14], AnahtarDeger[15] };

            EncryptDes(AnahtarDeger0007, KriptolanacakData, KriptolananData);
            DecryptDes(AnahtarDeger0815, KriptolananData, KriptolanacakData);
            EncryptDes(AnahtarDeger0007, KriptolanacakData, KriptolananData);

            return KriptolananData;
        }
    }
}
