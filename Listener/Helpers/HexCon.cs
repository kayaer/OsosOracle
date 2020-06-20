using System;
using System.Collections.Generic;
using System.Text;

namespace Listener.Helpers
{
    public class HexCon
    {
        public TrDes des = new TrDes();
        public bool CrcKontrol(byte[] buffer)
        {
            try
            {
                UInt16 crcCalc, crcGelen;
                int paketUzunluk = 0;

                paketUzunluk = ByteToDecimal(buffer, 2);

                crcCalc = ChecksumCalc(buffer, Convert.ToUInt16(paketUzunluk));
                crcGelen = buffer[paketUzunluk];
                crcGelen <<= 8;
                crcGelen += buffer[paketUzunluk + 1];

                if (crcGelen != crcCalc)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public UInt16 ChecksumCalc(byte[] cp, UInt16 len)
        {
            UInt16 fcs = 0;
            int i = 0;
            int index = 0;

            while (len != 0)
            {
                index = (fcs ^ cp[i++]) & 0xff;
                fcs = (UInt16)((fcs >> 8) ^ fcstab[index]);
                len--;
            }
            return fcs;
        }

        public Int32 ByteToDecimal(byte[] InBytes, int okunacak)
        {

            string StringOut = "";
            for (int i = 0; i <= okunacak - 1; i++)
            {
                StringOut = StringOut + String.Format("{0:X2}", InBytes[i]);
            }

            return Int32.Parse(StringOut, System.Globalization.NumberStyles.HexNumber);
        }

        public string BytesTostr(byte[] data)
        {
            string str = string.Empty;

            foreach (byte b in data)
            {
                if (b == 0)
                {
                    continue;
                }

                str += Convert.ToChar(b).ToString();
            }

            return str;
        }

        public string BytesTostr(byte[] data, int StarIndex, int adet)
        {
            string str = string.Empty;

            for (int i = StarIndex; i < adet; i++)
            {
                str += Convert.ToChar(data[i]).ToString();
            }

            return str;
        }

        public string BytesTostr(byte[] data, int adet)
        {
            string str = string.Empty;

            for (int i = 0; i < adet; i++)
            {
                str += Convert.ToChar(data[i]).ToString();
            }

            return str;
        }

        public string BytesTostr(byte[] data, int adet, byte VeriByte)
        {
            string str = string.Empty;

            for (int i = 0; i < adet; i++)
            {
                str += Convert.ToChar(data[i] & VeriByte).ToString();
            }

            return str;
        }

        public byte[] GetRandom()
        {
            UInt32 sId = (UInt32)((DateTime.Now.Ticks / 100000) % 0xffffffff);
            byte b1 = (byte)(sId % (256 * 256 * 256));
            sId = sId / 256;
            byte b2 = (byte)(sId % (256 * 256));
            sId = sId / 256;
            byte b3 = (byte)(sId % 256);
            byte b4 = (byte)(sId / 256);
            UInt32 sId1 = (UInt32)((DateTime.Now.Ticks / 100000) % 0xffffffff);
            byte b5 = (byte)(sId1 % (256 * 256 * 256));
            sId1 = sId1 / 256;
            byte b6 = (byte)(sId1 % (256 * 256));
            sId1 = sId1 / 256;
            byte b7 = (byte)(sId1 % 256);
            byte b8 = (byte)(sId1 / 256);
            byte[] random = new byte[8] { b1, b2, b3, b4, b8, b7, b6, b5 };

            return random;
        }
        public byte[] CreateKey(MasterKey mkey, byte[] random)
        {

            //MasterKey mkey = new MasterKey();

            //byte[] random = GetRandom();
            //byte[] random = new byte[] { 0X0B, 0X26, 0X43, 0X66, 0XDB, 0X54, 0X30, 0XD8 };

            byte[] k = mkey.getKey();
            byte[] encData1 = XOR(random, mkey.Mkey1);
            byte[] Key1 = new byte[8];
            des.TripleDesDec(k, encData1, ref Key1);

            byte[] encData2 = XOR(Key1, mkey.Mkey2);
            byte[] Key2 = new byte[8];
            des.TripleDesEnc(mkey.getKey(Key1), encData2, ref Key2);

            byte[] encData3 = XOR(Key2, mkey.Mkey3);
            byte[] Key3 = new byte[8];
            des.TripleDesDec(mkey.getKey(Key1, Key2), encData3, ref Key3);

            byte[] sessionKey = new byte[24];
            Buffer.BlockCopy(Key1, 0, sessionKey, 0, 8);
            Buffer.BlockCopy(Key2, 0, sessionKey, 8, 8);
            Buffer.BlockCopy(Key3, 0, sessionKey, 16, 8);

            return sessionKey;
        }
        public byte[] XOR(byte[] kaynak, byte[] hedef)
        {
            byte[] dizi = new byte[8];
            Buffer.BlockCopy(kaynak, 0, dizi, 0, 8);

            for (int i = 0; i < kaynak.Length; i++)
            {
                dizi[i] ^= hedef[i];
            }
            return dizi;
        }
        public bool Karsilastir(byte[] sayi1, byte[] sayi2)
        {
            for (int i = 0; i < sayi1.Length; i++)
            {
                if (sayi1[i] != sayi2[i]) return false;
            }
            return true;
        }
        public bool IsNumeric(string sayi)
        {
            System.Text.RegularExpressions.Regex allow = new System.Text.RegularExpressions.Regex("^[0-9.]*$");
            if (!allow.IsMatch(sayi)) return false;
            else return true;
        }
        #region checksum tablosu
        UInt16[] fcstab = new UInt16[256]  {
                0x0000, 0x1189, 0x2312, 0x329b, 0x4624, 0x57ad, 0x6536, 0x74bf,
                0x8c48, 0x9dc1, 0xaf5a, 0xbed3, 0xca6c, 0xdbe5, 0xe97e, 0xf8f7,
                0x1081, 0x0108, 0x3393, 0x221a, 0x56a5, 0x472c, 0x75b7, 0x643e,
                0x9cc9, 0x8d40, 0xbfdb, 0xae52, 0xdaed, 0xcb64, 0xf9ff, 0xe876,
                0x2102, 0x308b, 0x0210, 0x1399, 0x6726, 0x76af, 0x4434, 0x55bd,
                0xad4a, 0xbcc3, 0x8e58, 0x9fd1, 0xeb6e, 0xfae7, 0xc87c, 0xd9f5,
                0x3183, 0x200a, 0x1291, 0x0318, 0x77a7, 0x662e, 0x54b5, 0x453c,
                0xbdcb, 0xac42, 0x9ed9, 0x8f50, 0xfbef, 0xea66, 0xd8fd, 0xc974,
                0x4204, 0x538d, 0x6116, 0x709f, 0x0420, 0x15a9, 0x2732, 0x36bb,
                0xce4c, 0xdfc5, 0xed5e, 0xfcd7, 0x8868, 0x99e1, 0xab7a, 0xbaf3,
                0x5285, 0x430c, 0x7197, 0x601e, 0x14a1, 0x0528, 0x37b3, 0x263a,
                0xdecd, 0xcf44, 0xfddf, 0xec56, 0x98e9, 0x8960, 0xbbfb, 0xaa72,
                0x6306, 0x728f, 0x4014, 0x519d, 0x2522, 0x34ab, 0x0630, 0x17b9,
                0xef4e, 0xfec7, 0xcc5c, 0xddd5, 0xa96a, 0xb8e3, 0x8a78, 0x9bf1,
                0x7387, 0x620e, 0x5095, 0x411c, 0x35a3, 0x242a, 0x16b1, 0x0738,
                0xffcf, 0xee46, 0xdcdd, 0xcd54, 0xb9eb, 0xa862, 0x9af9, 0x8b70,
                0x8408, 0x9581, 0xa71a, 0xb693, 0xc22c, 0xd3a5, 0xe13e, 0xf0b7,
                0x0840, 0x19c9, 0x2b52, 0x3adb, 0x4e64, 0x5fed, 0x6d76, 0x7cff,
                0x9489, 0x8500, 0xb79b, 0xa612, 0xd2ad, 0xc324, 0xf1bf, 0xe036,
                0x18c1, 0x0948, 0x3bd3, 0x2a5a, 0x5ee5, 0x4f6c, 0x7df7, 0x6c7e,
                0xa50a, 0xb483, 0x8618, 0x9791, 0xe32e, 0xf2a7, 0xc03c, 0xd1b5,
                0x2942, 0x38cb, 0x0a50, 0x1bd9, 0x6f66, 0x7eef, 0x4c74, 0x5dfd,
                0xb58b, 0xa402, 0x9699, 0x8710, 0xf3af, 0xe226, 0xd0bd, 0xc134,
                0x39c3, 0x284a, 0x1ad1, 0x0b58, 0x7fe7, 0x6e6e, 0x5cf5, 0x4d7c,
                0xc60c, 0xd785, 0xe51e, 0xf497, 0x8028, 0x91a1, 0xa33a, 0xb2b3,
                0x4a44, 0x5bcd, 0x6956, 0x78df, 0x0c60, 0x1de9, 0x2f72, 0x3efb,
                0xd68d, 0xc704, 0xf59f, 0xe416, 0x90a9, 0x8120, 0xb3bb, 0xa232,
                0x5ac5, 0x4b4c, 0x79d7, 0x685e, 0x1ce1, 0x0d68, 0x3ff3, 0x2e7a,
                0xe70e, 0xf687, 0xc41c, 0xd595, 0xa12a, 0xb0a3, 0x8238, 0x93b1,
                0x6b46, 0x7acf, 0x4854, 0x59dd, 0x2d62, 0x3ceb, 0x0e70, 0x1ff9,
                0xf78f, 0xe606, 0xd49d, 0xc514, 0xb1ab, 0xa022, 0x92b9, 0x8330,
                0x7bc7, 0x6a4e, 0x58d5, 0x495c, 0x3de3, 0x2c6a, 0x1ef1, 0x0f78
                };
        #endregion
    }
}
