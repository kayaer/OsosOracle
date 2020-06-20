using Listener.Entities;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Listener.Helpers
{
    public class Paket
    {
        public HexCon hexCon = new HexCon();

        public byte[] sessionKey = new byte[24];
        public byte[] DesKey = new byte[8];
        public byte[] encRandom = new byte[8];

        #region Net

        public Header HeaderOku(NetworkStream stream)
        {
            byte[] message = PaketOku(stream, false, null);

            if (message == null) return null;
            else
            {
                Header konP = new Header(hexCon.BytesTostr(message));
                return konP;
            }
        }



        public byte[] PaketOku(NetworkStream stream, bool CryptoKontrol, byte[] Key)
        {
            byte[] buffer = new byte[16000];
            byte[] message;

            int paketUzunluk = 0;
            int bytesRead = 0;
            int kalan = 1;
            int okunan = 2;

            bytesRead = stream.Read(buffer, 0, 2);

            if (bytesRead > 0)
            {
                paketUzunluk = hexCon.ByteToDecimal(buffer, 2);
                if (paketUzunluk < 16000)
                {
                    message = new byte[paketUzunluk + 2];
                }
                else return null;
            }
            else return null;

            message[0] = buffer[0];
            message[1] = buffer[1];


            while (okunan < paketUzunluk + 2)
            {
                bytesRead = stream.Read(buffer, 0, kalan);

                if (bytesRead > 0)
                {
                    Buffer.BlockCopy(buffer, 0, message, okunan, bytesRead);
                    okunan += bytesRead;

                    kalan = (paketUzunluk + 2) - okunan;
                }
                else return null;
            }

            if (CryptoKontrol)
            {
                byte[] paket8 = new byte[8];

                for (int i = 2; i < paketUzunluk + 2; i += 8)
                {

                    Buffer.BlockCopy(message, i, paket8, 0, 8);
                    hexCon.des.DecryptDes(Key, paket8, ref paket8);
                    Buffer.BlockCopy(paket8, 0, message, i - 2, 8);
                }

                paketUzunluk = hexCon.ByteToDecimal(message, 2);

                /*
                UInt16 crcCalc, crcGelen;

                crcCalc = hexCon.ChecksumCalc(message, Convert.ToUInt16(encryptedPaketUzunluk));
                crcGelen = message[encryptedPaketUzunluk];
                crcGelen <<= 8;
                crcGelen += message[encryptedPaketUzunluk + 1];
                if (crcGelen != crcCalc)
                {
                    return null;
                }

                byte[] data = new byte[encryptedPaketUzunluk - 2];
                Buffer.BlockCopy(message, 2, data, 0, encryptedPaketUzunluk - 2);

                return data;
                */
            }

            UInt16 crcCalc, crcGelen;

            crcCalc = hexCon.ChecksumCalc(message, Convert.ToUInt16(paketUzunluk));
            crcGelen = message[paketUzunluk];
            crcGelen <<= 8;
            crcGelen += message[paketUzunluk + 1];

            //burası aktifleştirilecek
            if (crcGelen != crcCalc)
            {
                //return null;
            }

            byte[] data = new byte[paketUzunluk - 2];

            Buffer.BlockCopy(message, 2, data, 0, paketUzunluk - 2);

            return data;

        }
        //mode 0: Server,  1:Client 
        public bool Login(NetworkStream stream, UInt32 RandomSayi, int Mode, bool CryptoKontrol, ref byte[] oturumKey) // 0 : Listen Mode, 1: Client Mode
        {

            bool loginSonuc = false;

            byte[] randomDizi = new byte[8];
            MasterKey mkey = new MasterKey();

            byte[] gonderilecekRandom = new byte[8];


            if (Mode == 1)
            {
                UInt32 random2 = RandomSayi * 3564812797;

                randomDizi[0] = Convert.ToByte(RandomSayi & 0XFF);
                randomDizi[1] = Convert.ToByte((RandomSayi >> 8) & 0XFF);
                randomDizi[2] = Convert.ToByte((RandomSayi >> 16) & 0XFF);
                randomDizi[3] = Convert.ToByte((RandomSayi >> 24) & 0XFF);
                randomDizi[4] = Convert.ToByte(random2 & 0XFF);
                randomDizi[5] = Convert.ToByte((random2 >> 8) & 0XFF);
                randomDizi[6] = Convert.ToByte((random2 >> 16) & 0XFF);
                randomDizi[7] = Convert.ToByte((random2 >> 24) & 0XFF);
            }
            else
            {
                byte[] rnd = hexCon.GetRandom();

                Buffer.BlockCopy(rnd, 0, randomDizi, 0, 8);
                Buffer.BlockCopy(rnd, 0, gonderilecekRandom, 0, 8);

            }

            sessionKey = hexCon.CreateKey(mkey, randomDizi);

            hexCon.des.TripleDesEnc(sessionKey, randomDizi, ref encRandom);

            Buffer.BlockCopy(sessionKey, 16, DesKey, 0, 8);

            if (Mode == 1)
            {
                PaketGonder(stream, encRandom, false, null);

                byte[] loginAck = PaketOku(stream, CryptoKontrol, DesKey);

                if (loginAck[0] != 6)
                {
                    loginSonuc = false;
                }
                else
                {
                    oturumKey = sessionKey;
                    loginSonuc = true;
                }
            }
            else
            {

                PaketGonder(stream, gonderilecekRandom, false, null);

                byte[] gelenCrptedRandom = PaketOku(stream, false, null);

                loginSonuc = hexCon.Karsilastir(gelenCrptedRandom, encRandom);

                if (loginSonuc)
                {
                    Buffer.BlockCopy(sessionKey, 16, DesKey, 0, 8);

                    SendAck(stream, false, DesKey);//kontrol edilecek

                    try
                    {
                        // eğer alarm biti 1 ise ve enerji gitti ise paket oku çağrılmayacak
                        byte[] loginAck = PaketOku(stream, CryptoKontrol, DesKey);

                        if (loginAck != null)
                        {
                            if (loginAck[0] != 6)
                            {
                                return false;
                            }
                            else
                            {
                                oturumKey = sessionKey;
                                return true;
                            }
                        }

                    }
                    catch (Exception ex)
                    {

                    }

                }
                else
                {
                    return false;
                }

            }

            return loginSonuc;
        }

        public void SendAck(NetworkStream stream, bool CryptoKontrol, byte[] Key)
        {

            byte[] ack = new byte[] { 0X06 };

            //byte[] crpAck = new byte[10];
            //byte[] paket8 = new byte[8];

            //UInt16 crcAck = hexCon.ChecksumCalc(ack, 3);
            //Buffer.BlockCopy(ack, 0, crpAck, 2, 3);
            //crpAck[5] = (byte)(crcAck >> 8);
            //crpAck[6] = (byte)(crcAck);
            //Buffer.BlockCopy(crpAck, 2, paket8, 0, 5);
            //hexCon.des.EncryptDes(Key, paket8, ref paket8);
            //Buffer.BlockCopy(paket8, 0, crpAck, 2, 8);

            //crpAck[0] = 0;
            //crpAck[1] = 8;

            //stream.Write(crpAck, 0, crpAck.Length);

            PaketGonder(stream, ack, CryptoKontrol, Key);

        }

        public void PaketGonder(NetworkStream stream, byte[] paket, bool CryptoKontrol, byte[] Key)
        {

            byte[] paket8 = new byte[8];

            List<byte> buffer = new List<byte>();

            buffer.Add(Convert.ToByte((paket.Length + 2) >> 8));
            buffer.Add(Convert.ToByte((paket.Length + 2) & 0xFF));
            buffer.AddRange(paket);

            UInt16 crcPaket = hexCon.ChecksumCalc(buffer.ToArray(), Convert.ToUInt16(buffer.Count));

            buffer.Add((byte)(crcPaket >> 8));
            buffer.Add((byte)(crcPaket));

            if (CryptoKontrol)
            {

                int modkalan = buffer.Count % 8;
                modkalan = 8 - modkalan;

                for (int i = 0; i < modkalan; i++)
                {
                    buffer.Add(0);
                }

                byte[] crpBuffer = buffer.ToArray();

                for (int i = 0; i < crpBuffer.Length; i += 8)
                {
                    Buffer.BlockCopy(crpBuffer, i, paket8, 0, 8);
                    hexCon.des.EncryptDes(Key, paket8, ref paket8);
                    Buffer.BlockCopy(paket8, 0, crpBuffer, i, 8);
                }

                buffer.Clear();

                buffer.Add(Convert.ToByte((crpBuffer.Length) >> 8));
                buffer.Add(Convert.ToByte((crpBuffer.Length) & 0xFF));
                buffer.AddRange(crpBuffer);

            }

            stream.Write(buffer.ToArray(), 0, buffer.Count);
            //return buffer.ToArray();
        }

        public byte[] LoginPaketiOlustur(UInt32 RandomSayi, int Mode, bool CryptoKontrol) // 0 : Listen Mode, 1: Client Mode
        {

            byte[] randomDizi = new byte[8];
            byte[] gonderilecekRandom = new byte[8];
            MasterKey mkey = new MasterKey();


            if (Mode == 1)
            {
                UInt32 random2 = RandomSayi * 3564812797;

                randomDizi[0] = Convert.ToByte(RandomSayi & 0XFF);
                randomDizi[1] = Convert.ToByte((RandomSayi >> 8) & 0XFF);
                randomDizi[2] = Convert.ToByte((RandomSayi >> 16) & 0XFF);
                randomDizi[3] = Convert.ToByte((RandomSayi >> 24) & 0XFF);
                randomDizi[4] = Convert.ToByte(random2 & 0XFF);
                randomDizi[5] = Convert.ToByte((random2 >> 8) & 0XFF);
                randomDizi[6] = Convert.ToByte((random2 >> 16) & 0XFF);
                randomDizi[7] = Convert.ToByte((random2 >> 24) & 0XFF);
            }
            else
            {
                byte[] rnd = hexCon.GetRandom();

                Buffer.BlockCopy(rnd, 0, randomDizi, 0, 8);
                Buffer.BlockCopy(rnd, 0, gonderilecekRandom, 0, 8);

            }

            sessionKey = hexCon.CreateKey(mkey, randomDizi);

            hexCon.des.TripleDesEnc(sessionKey, randomDizi, ref encRandom);

            Buffer.BlockCopy(sessionKey, 16, DesKey, 0, 8);


            if (Mode == 1)
            {
                return encRandom;
                //PaketGonder(stream, encRandom, false, null);

                //byte[] loginAck = PaketOku(stream, CryptoKontrol, DesKey);

                //if (loginAck[0] != 6)
                //{
                //    return null;
                //}
                //else
                //{
                //    return sessionKey;
                //}

            }
            else
            {

                return gonderilecekRandom;
                //PaketGonder(stream, gonderilecekRandom, false, null);

                //byte[] gelenCrptedRandom = PaketOku(stream, false, null);

                //bool login = hexCon.Karsilastir(gelenCrptedRandom, encRandom);

                //if (login)
                //{
                //    Buffer.BlockCopy(sessionKey, 16, DesKey, 0, 8);
                //    SendAck(stream, true, DesKey);

                //    byte[] loginAck = PaketOku(stream, CryptoKontrol, DesKey);

                //    if (loginAck[0] != 6)
                //    {
                //        return null;
                //    }
                //    else
                //    {
                //        return sessionKey;
                //    }
                //}
                //else
                //{
                //    return null;
                //}

            }


        }

        public byte[] GonderilecekPaketOlustur(byte[] paket, bool CryptoKontrol, byte[] Key)
        {

            byte[] paket8 = new byte[8];

            List<byte> buffer = new List<byte>();

            buffer.Add(Convert.ToByte((paket.Length + 2) >> 8));
            buffer.Add(Convert.ToByte((paket.Length + 2) & 0xFF));
            buffer.AddRange(paket);

            UInt16 crcPaket = hexCon.ChecksumCalc(buffer.ToArray(), Convert.ToUInt16(buffer.Count));

            buffer.Add((byte)(crcPaket >> 8));
            buffer.Add((byte)(crcPaket));

            if (CryptoKontrol)
            {

                int modkalan = buffer.Count % 8;
                modkalan = 8 - modkalan;

                for (int i = 0; i < modkalan; i++)
                {
                    buffer.Add(0);
                }

                byte[] crpBuffer = buffer.ToArray();

                for (int i = 0; i < crpBuffer.Length; i += 8)
                {
                    Buffer.BlockCopy(crpBuffer, i, paket8, 0, 8);
                    hexCon.des.EncryptDes(Key, paket8, ref paket8);
                    Buffer.BlockCopy(paket8, 0, crpBuffer, i, 8);
                }

                buffer.Clear();

                buffer.Add(Convert.ToByte((crpBuffer.Length) >> 8));
                buffer.Add(Convert.ToByte((crpBuffer.Length) & 0xFF));
                buffer.AddRange(crpBuffer);

            }

            //stream.Write(buffer.ToArray(), 0, buffer.Count);
            return buffer.ToArray();
        }

        public byte[] PaketCozumle(int paketUzunluk, byte[] temp, bool CryptoKontrol, byte[] Key)
        {

            if (paketUzunluk > 10240) return null;

            byte[] message = new byte[paketUzunluk + 2];
            message[0] = Convert.ToByte(paketUzunluk >> 8);
            message[1] = Convert.ToByte(paketUzunluk & 0XFF);

            Buffer.BlockCopy(temp, 0, message, 2, paketUzunluk);

            if (CryptoKontrol)
            {
                byte[] paket8 = new byte[8];

                for (int i = 0; i < paketUzunluk; i += 8)
                {

                    Buffer.BlockCopy(message, i, paket8, 0, 8);
                    hexCon.des.DecryptDes(Key, paket8, ref paket8);
                    Buffer.BlockCopy(paket8, 0, message, i, 8);
                }

                paketUzunluk = hexCon.ByteToDecimal(message, 2);

            }

            UInt16 crcCalc, crcGelen;

            crcCalc = hexCon.ChecksumCalc(message, Convert.ToUInt16(paketUzunluk));
            crcGelen = message[paketUzunluk];
            crcGelen <<= 8;
            crcGelen += message[paketUzunluk + 1];
            if (crcGelen != crcCalc)
            {
                return null;
            }

            byte[] data = new byte[paketUzunluk - 2];

            Buffer.BlockCopy(message, 2, data, 0, paketUzunluk - 2);

            return data;

        }

        public bool CrcKontrol(byte[] buffer)
        {
            try
            {
                UInt16 crcCalc, crcGelen;
                int paketUzunluk = 0;

                paketUzunluk = hexCon.ByteToDecimal(buffer, 2);

                crcCalc = hexCon.ChecksumCalc(buffer, Convert.ToUInt16(paketUzunluk));
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

        #endregion

        //#region IO stream

        //public Header HeaderiOku(SerialPort stream)
        //{
        //    byte[] message = PaketOku(stream, false, null);

        //    if (message == null) return null;
        //    else
        //    {
        //        Header konP = new Header(hexCon.BytesTostr(message));
        //        return konP;
        //    }
        //}

        //public int ReadPort(SerialPort stream, ref byte[] buffer, int okunacak)
        //{
        //    byte[] t = new byte[1];

        //    t[0] = 40;
        //    try
        //    {
        //        stream.ReadTimeout = 60000;

        //        for (int i = 0; i < okunacak; i++)
        //        {
        //            if (stream.IsOpen)
        //            {
        //                int bytesRead = stream.Read(t, 0, 1);

        //                if (bytesRead > 0) stream.ReadTimeout = 500;
        //                else
        //                {
        //                    okunacak = i;
        //                    break;
        //                }
        //            }
        //            else
        //            {
        //                okunacak = i;
        //                break;
        //            }

        //            buffer[i] = t[0];
        //        }

        //        return okunacak;
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return okunacak;
        //}

        //public byte[] PaketOku(SerialPort stream, bool CryptoKontrol, byte[] Key)
        //{
        //    byte[] buffer = new byte[10240];
        //    byte[] message;

        //    int paketUzunluk = 0;
        //    int bytesRead = 0;
        //    int kalan = 1;
        //    int okunan = 2;

        //    bytesRead = ReadPort(stream, ref buffer, 2);

        //    if (bytesRead > 0)
        //    {
        //        paketUzunluk = hexCon.ByteToDecimal(buffer, 2);
        //        if (paketUzunluk < 10240)
        //        {
        //            message = new byte[paketUzunluk + 2];
        //        }
        //        else return null;
        //    }
        //    else return null;

        //    message[0] = buffer[0];
        //    message[1] = buffer[1];


        //    while (okunan < paketUzunluk + 2)
        //    {
        //        bytesRead = ReadPort(stream, ref buffer, kalan);

        //        if (bytesRead > 0)
        //        {
        //            Buffer.BlockCopy(buffer, 0, message, okunan, bytesRead);
        //            okunan += bytesRead;

        //            kalan = (paketUzunluk + 2) - okunan;
        //        }
        //        else return null;
        //    }

        //    if (CryptoKontrol)
        //    {
        //        byte[] paket8 = new byte[8];

        //        for (int i = 2; i < paketUzunluk + 2; i += 8)
        //        {

        //            Buffer.BlockCopy(message, i, paket8, 0, 8);
        //            hexCon.des.DecryptDes(Key, paket8, ref paket8);
        //            Buffer.BlockCopy(paket8, 0, message, i - 2, 8);
        //        }

        //        paketUzunluk = hexCon.ByteToDecimal(message, 2);

        //        /*
        //        UInt16 crcCalc, crcGelen;

        //        crcCalc = hexCon.ChecksumCalc(message, Convert.ToUInt16(encryptedPaketUzunluk));
        //        crcGelen = message[encryptedPaketUzunluk];
        //        crcGelen <<= 8;
        //        crcGelen += message[encryptedPaketUzunluk + 1];
        //        if (crcGelen != crcCalc)
        //        {
        //            return null;
        //        }

        //        byte[] data = new byte[encryptedPaketUzunluk - 2];
        //        Buffer.BlockCopy(message, 2, data, 0, encryptedPaketUzunluk - 2);

        //        return data;
        //        */
        //    }

        //    UInt16 crcCalc, crcGelen;

        //    crcCalc = hexCon.ChecksumCalc(message, Convert.ToUInt16(paketUzunluk));
        //    crcGelen = message[paketUzunluk];
        //    crcGelen <<= 8;
        //    crcGelen += message[paketUzunluk + 1];
        //    if (crcGelen != crcCalc)
        //    {
        //        return null;
        //    }

        //    byte[] data = new byte[paketUzunluk - 2];

        //    Buffer.BlockCopy(message, 2, data, 0, paketUzunluk - 2);

        //    return data;

        //}

        //public byte[] Login(SerialPort stream, UInt32 RandomSayi, int Mode, bool CryptoKontrol) // 0 : Listen Mode, 1: Client Mode
        //{
        //    byte[] sessionKey = new byte[24];
        //    byte[] DesKey = new byte[8];

        //    byte[] randomDizi = new byte[8];
        //    MasterKey mkey = new MasterKey();
        //    byte[] encRandom = new byte[8];
        //    byte[] gonderilecekRandom = new byte[8];


        //    if (Mode == 1)
        //    {
        //        UInt32 random2 = RandomSayi * 3564812797;

        //        randomDizi[0] = Convert.ToByte(RandomSayi & 0XFF);
        //        randomDizi[1] = Convert.ToByte((RandomSayi >> 8) & 0XFF);
        //        randomDizi[2] = Convert.ToByte((RandomSayi >> 16) & 0XFF);
        //        randomDizi[3] = Convert.ToByte((RandomSayi >> 24) & 0XFF);
        //        randomDizi[4] = Convert.ToByte(random2 & 0XFF);
        //        randomDizi[5] = Convert.ToByte((random2 >> 8) & 0XFF);
        //        randomDizi[6] = Convert.ToByte((random2 >> 16) & 0XFF);
        //        randomDizi[7] = Convert.ToByte((random2 >> 24) & 0XFF);
        //    }
        //    else
        //    {
        //        byte[] rnd = hexCon.GetRandom();

        //        Buffer.BlockCopy(rnd, 0, randomDizi, 0, 8);
        //        Buffer.BlockCopy(rnd, 0, gonderilecekRandom, 0, 8);

        //    }

        //    sessionKey = hexCon.CreateKey(mkey, randomDizi);

        //    hexCon.des.TripleDesEnc(sessionKey, randomDizi, ref encRandom);

        //    Buffer.BlockCopy(sessionKey, 16, DesKey, 0, 8);

        //    if (Mode == 1)
        //    {

        //        PaketGonder(stream, encRandom, false, null);

        //        byte[] loginAck = PaketOku(stream, CryptoKontrol, DesKey);

        //        if (loginAck[0] != 6)
        //        {
        //            return null;
        //        }
        //        else
        //        {
        //            return sessionKey;
        //        }

        //    }
        //    else
        //    {

        //        PaketGonder(stream, gonderilecekRandom, false, null);

        //        byte[] gelenCrptedRandom = PaketOku(stream, false, null);

        //        bool login = hexCon.Karsilastir(gelenCrptedRandom, encRandom);

        //        if (login)
        //        {
        //            Buffer.BlockCopy(sessionKey, 16, DesKey, 0, 8);

        //            SendAck(stream, true, DesKey);//kontrol edilecek

        //            byte[] loginAck = PaketOku(stream, CryptoKontrol, DesKey);

        //            if (loginAck[0] != 6)
        //            {
        //                return null;
        //            }
        //            else
        //            {
        //                return sessionKey;
        //            }
        //        }
        //        else
        //        {
        //            return null;
        //        }

        //    }


        //}

        //public void SendAck(SerialPort stream, bool CryptoKontrol, byte[] Key)
        //{

        //    byte[] ack = new byte[] { 0X06 };

        //    //byte[] crpAck = new byte[10];
        //    //byte[] paket8 = new byte[8];

        //    //UInt16 crcAck = hexCon.ChecksumCalc(ack, 3);
        //    //Buffer.BlockCopy(ack, 0, crpAck, 2, 3);
        //    //crpAck[5] = (byte)(crcAck >> 8);
        //    //crpAck[6] = (byte)(crcAck);
        //    //Buffer.BlockCopy(crpAck, 2, paket8, 0, 5);
        //    //hexCon.des.EncryptDes(Key, paket8, ref paket8);
        //    //Buffer.BlockCopy(paket8, 0, crpAck, 2, 8);

        //    //crpAck[0] = 0;
        //    //crpAck[1] = 8;

        //    //stream.Write(crpAck, 0, crpAck.Length);

        //    PaketGonder(stream, ack, CryptoKontrol, Key);

        //}

        //public void PaketGonder(SerialPort stream, byte[] paket, bool CryptoKontrol, byte[] Key)
        //{

        //    byte[] paket8 = new byte[8];

        //    List<byte> buffer = new List<byte>();

        //    buffer.Add(Convert.ToByte((paket.Length + 2) >> 8));
        //    buffer.Add(Convert.ToByte((paket.Length + 2) & 0xFF));
        //    buffer.AddRange(paket);

        //    UInt16 crcPaket = hexCon.ChecksumCalc(buffer.ToArray(), Convert.ToUInt16(buffer.Count));

        //    buffer.Add((byte)(crcPaket >> 8));
        //    buffer.Add((byte)(crcPaket));

        //    if (CryptoKontrol)
        //    {

        //        int modkalan = buffer.Count % 8;
        //        modkalan = 8 - modkalan;

        //        for (int i = 0; i < modkalan; i++)
        //        {
        //            buffer.Add(0);
        //        }

        //        byte[] crpBuffer = buffer.ToArray();

        //        for (int i = 0; i < crpBuffer.Length; i += 8)
        //        {
        //            Buffer.BlockCopy(crpBuffer, i, paket8, 0, 8);
        //            hexCon.des.EncryptDes(Key, paket8, ref paket8);
        //            Buffer.BlockCopy(paket8, 0, crpBuffer, i, 8);
        //        }

        //        buffer.Clear();

        //        buffer.Add(Convert.ToByte((crpBuffer.Length) >> 8));
        //        buffer.Add(Convert.ToByte((crpBuffer.Length) & 0xFF));
        //        buffer.AddRange(crpBuffer);

        //    }

        //    stream.Write(buffer.ToArray(), 0, buffer.Count);
        //    //return buffer.ToArray();
        //}

        //#endregion

        //#region pstn

        //public void SendPort(SerialPort stream, string data)
        //{
        //    stream.Write(data);
        //}
        //public void SendPort(SerialPort stream, byte[] data)
        //{
        //    stream.Write(data, 0, data.Length);
        //}

        //public bool InitModem(SerialPort stream)
        //{
        //    SendPort(stream, "ATE0\r");

        //    byte[] buffer = new byte[100];

        //    ReadPort(stream, ref buffer, 100);

        //    string con = hexCon.BytesTostr(buffer);

        //    // ReadPort(stream, ref buffer, 100);

        //    con = hexCon.BytesTostr(buffer);

        //    if (con.Contains("OK")) return true;
        //    else return false;
        //}

        //public bool CallModem(SerialPort stream, string tel)
        //{
        //    SendPort(stream, "ATD" + tel + "\r");

        //    byte[] buffer = new byte[100];

        //    ReadPort(stream, ref buffer, 100);

        //    string con = hexCon.BytesTostr(buffer);

        //    if (con.Contains("CONNECT")) return true;
        //    else return false;
        //}

        //public bool HangUpConnection(SerialPort stream)
        //{
        //    hexCon.Sleep(1500);

        //    SendPort(stream, "+++");

        //    byte[] buffer = new byte[100];

        //    ReadPort(stream, ref buffer, 100);

        //    string con = hexCon.BytesTostr(buffer);

        //    SendPort(stream, "ATH\r");

        //    ReadPort(stream, ref buffer, 100);

        //    con = hexCon.BytesTostr(buffer);

        //    if (con.Contains("OK")) return true;
        //    else return false;
        //}


        //#endregion
    }
}
