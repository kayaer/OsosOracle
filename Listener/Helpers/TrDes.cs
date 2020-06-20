using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Listener.Helpers
{
    public class TrDes
    {

        static byte[] IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };

        public void EncryptDes(byte[] Key, byte[] InData, ref byte[] OutData)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Mode = CipherMode.ECB;
            des.Padding = PaddingMode.Zeros;
            ICryptoTransform cEncryptor = des.CreateEncryptor(Key, IV);
            OutData = cEncryptor.TransformFinalBlock(InData, 0, InData.Length);

        }

        public void DecryptDes(byte[] Key, byte[] InData, ref byte[] OutData)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Mode = CipherMode.ECB;
            des.Padding = PaddingMode.Zeros;
            ICryptoTransform cDecryptor = des.CreateDecryptor(Key, IV);
            OutData = cDecryptor.TransformFinalBlock(InData, 0, InData.Length);
        }

        public byte[] TripleDesEnc(byte[] AnahtarDeger, byte[] KriptolanacakData, ref byte[] KriptolananData)
        {
            byte[] AnahtarDeger0007 = { AnahtarDeger[0], AnahtarDeger[1], AnahtarDeger[2], AnahtarDeger[3],
                                        AnahtarDeger[4], AnahtarDeger[5], AnahtarDeger[6], AnahtarDeger[7] };
            byte[] AnahtarDeger0815 = { AnahtarDeger[8], AnahtarDeger[9], AnahtarDeger[10], AnahtarDeger[11],
                                        AnahtarDeger[12], AnahtarDeger[13], AnahtarDeger[14], AnahtarDeger[15] };
            byte[] AnahtarDeger01523 = { AnahtarDeger[16], AnahtarDeger[17], AnahtarDeger[18], AnahtarDeger[19],
                                        AnahtarDeger[20], AnahtarDeger[21], AnahtarDeger[22], AnahtarDeger[23] };

            EncryptDes(AnahtarDeger0007, KriptolanacakData, ref KriptolananData);
            DecryptDes(AnahtarDeger0815, KriptolananData, ref KriptolanacakData);
            EncryptDes(AnahtarDeger01523, KriptolanacakData, ref KriptolananData);

            return KriptolananData;
        }

        public byte[] TripleDesDec(byte[] AnahtarDeger, byte[] KriptoluData, ref byte[] CozulmusData)
        {
            byte[] AnahtarDeger0007 = { AnahtarDeger[0], AnahtarDeger[1], AnahtarDeger[2], AnahtarDeger[3],
                                        AnahtarDeger[4], AnahtarDeger[5], AnahtarDeger[6], AnahtarDeger[7] };
            byte[] AnahtarDeger0815 = { AnahtarDeger[8], AnahtarDeger[9], AnahtarDeger[10], AnahtarDeger[11],
                                        AnahtarDeger[12], AnahtarDeger[13], AnahtarDeger[14], AnahtarDeger[15] };
            byte[] AnahtarDeger01523 = { AnahtarDeger[16], AnahtarDeger[17], AnahtarDeger[18], AnahtarDeger[19],
                                        AnahtarDeger[20], AnahtarDeger[21], AnahtarDeger[22], AnahtarDeger[23] };


            DecryptDes(AnahtarDeger01523, KriptoluData, ref CozulmusData);
            EncryptDes(AnahtarDeger0815, CozulmusData, ref KriptoluData);
            DecryptDes(AnahtarDeger0007, KriptoluData, ref CozulmusData);

            return CozulmusData;
        }

    }
}
