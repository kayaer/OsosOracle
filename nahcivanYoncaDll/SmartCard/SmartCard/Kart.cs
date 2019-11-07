using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace SmartCard
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class Kart : IKart
    {
        static SerialPort port = new SerialPort();

        Util util = new Util();

        public string PortAd = "";
        public Int32 zone;
        private string sprtr = "|";


        #region Bufferlar
        byte[] inBuf = new byte[128];
        byte[] outBuf = new byte[128];
        byte[] buffer = new byte[100];



        public Kart(string Port, int Zone)
        {
            PortAd = Port;

            ClosePort();

            InitPort();
            OpenPort();

            zone = 84;       //zone = Zone;
        }

        private void Sleep(int time)
        {
            System.Threading.Thread.Sleep(time);
        }

        private void InitPort()
        {
            //port = new SerialPort();

            port.PortName = PortAd;
            port.DataBits = 8;
            port.BaudRate = 4800;
            port.Parity = Parity.None;
            port.DtrEnable = true;
            port.RtsEnable = false;

            port.ReadTimeout = 5000;
        }

        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int bytes = port.BytesToRead;
            int i = port.Read(inBuf, 0, bytes);
            char ccc = (char)inBuf[0];
        }

        #region Port işlemleri
        private void OpenPort()
        {
            try
            {
                if (!port.IsOpen)
                {
                    port.Open();
                    // port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
                    port.DiscardOutBuffer();
                    port.DiscardInBuffer();
                }
            }
            catch (Exception ex)
            {
                if (port == null)
                {
                    MessageBox.Show("null geldi");
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show(ex.ToString());
                }
            }
        }

        private void ClosePort()
        {
            try
            {
                if ( port != null )
                {
                    if (port.IsOpen) port.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private void SendPort(byte[] data)
        {
            try
            {
                if (port.IsOpen) port.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {

            }

        }
        private void SendPort(byte[] data, int uzunluk)
        {

            byte[] t = new byte[1];

            try
            {
                for (int i = 0; i < uzunluk; i++)
                {
                    t[0] = data[i];
                    if (port.IsOpen) port.Write(t, 0, 1);
                }

            }
            catch (Exception ex)
            {

            }

        }
        private void SendPort(char[] data)
        {
            try
            {
                if (port.IsOpen) port.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {

            }

        }
        private void ReadPort(ref byte[] b, int okunacak)
        {
            byte[] t = new byte[1];

            t[0] = 40;
            try
            {
                for (int i = 0; i < okunacak; i++)
                {
                    if (port.IsOpen)
                        port.Read(t, 0, 1);
                    else
                        break;

                    b[i] = t[0];
                }

                // if (port.IsOpen) port.Read(b, 0, okunacak);
            }
            catch (Exception ex)
            {
                string st = "";
            }

        }
        #endregion


        #region  Kart Okuma Yazma Fonksyonları

        private int InitCard()
        {
            try
            {

                Sleep(10);
                SendPort(Hexcon.CharToByteArray('C'));
                Sleep(10);
                ReadPort(ref buffer, 1);
                if (((char)buffer[0] != 'c') || ((char)buffer[0] == '0')) return 0;

                SendPort(Hexcon.CharToByteArray('A'));
                Sleep(10);
                ReadPort(ref buffer, 1);
                if (((char)buffer[0] != 'a') || ((char)buffer[0] == '0')) return 0;

                SendPort(Hexcon.CharToByteArray('K'));
                Sleep(10);
                ReadPort(ref buffer, 1);
                if ((char)buffer[0] == '0')
                {
                    return 0;
                }
                if ((char)buffer[0] == 'Q')
                {
                    return 5;
                }
                else if ((char)buffer[0] == 'k')
                {
                    Sleep(10);
                    ReadPort(ref buffer, 1);
                    if ((char)buffer[0] == '0') return 0;
                }
                else
                {
                    return 0;
                }

                if ((char)buffer[0] == 'b') return 1;
                else return 0;


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private int FinishCard()
        {
            Sleep(10);

            SendPort(Hexcon.CharToByteArray('C'));
            ReadPort(ref buffer, 1);
            if ((buffer[0] != 99) || ((char)buffer[0] == '0')) return 0;

            SendPort(Hexcon.CharToByteArray('A'));
            ReadPort(ref buffer, 1);
            if (((char)buffer[0] != 'a') || ((char)buffer[0] == '0')) return 0;

            SendPort(Hexcon.CharToByteArray('J'));
            ReadPort(ref buffer, 1);
            if ((char)buffer[0] == '0') return 0;
            if ((char)buffer[0] == 'Q') return 5;
            else if ((char)buffer[0] == 'j')
            {
                ReadPort(ref buffer, 1);
                if ((char)buffer[0] == '0') return 0;
            }
            else return 0;

            if ((char)buffer[0] == 'b') return 1;
            else return 0;

        }

        private int ReadCard(byte adres)
        {

            Sleep(10);
            SendPort(Hexcon.CharToByteArray('C'));
            ReadPort(ref buffer, 1);
            if ((buffer[0] != 99) || ((char)buffer[0] == '0')) return 0;

            SendPort(Hexcon.CharToByteArray('A'));
            ReadPort(ref buffer, 1);
            if (((char)buffer[0] != 'a') || ((char)buffer[0] == '0')) return 0;

            SendPort(Hexcon.CharToByteArray('F'));
            ReadPort(ref buffer, 1);
            if ((char)buffer[0] == '0') return 0;

            else if ((char)buffer[0] == 'f')
            {
                SendPort(Hexcon.ByteToByteArray(adres));
            }
            else return 0;

            ReadPort(ref buffer, 1);
            if (buffer[0] != adres) return 0;

            ReadPort(ref buffer, 1);

            if ((char)buffer[0] == 'b')
            {
                ReadPort(ref inBuf, 4);
                return 1;
            }
            else if ((char)buffer[0] == 'Q') return 5;
            else return 0;

        }

        private UInt32 SendAboneCsc(UInt32 dn, UInt32 alfa)
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

        private int VerifyCard(byte adres)
        {
            Sleep(10);
            SendPort(Hexcon.CharToByteArray('C'));
            ReadPort(ref buffer, 1);
            if ((buffer[0] != 99) || ((char)buffer[0] == '0')) return 0;

            SendPort(Hexcon.CharToByteArray('A'));
            ReadPort(ref buffer, 1);
            if (((char)buffer[0] != 'a') || ((char)buffer[0] == '0')) return 0;

            SendPort(Hexcon.CharToByteArray('H'));
            ReadPort(ref buffer, 1);
            if ((char)buffer[0] == 'Q') return 5;
            else if ((char)buffer[0] == 'h')
            {
                SendPort(Hexcon.ByteToByteArray(adres));
            }
            else return 0;


            ReadPort(ref buffer, 1);
            if (buffer[0] != adres) return 0;

            SendPort(outBuf, 4); //SendPort abone csc den sonra outbufın ilk 4 byte ı dolduruluyor

            buffer[0] = 65;
            buffer[1] = 65;

            ReadPort(ref buffer, 2);

            if ((char)buffer[0] != 'Q')
            {
                if ((buffer[0] == 0X90) && (buffer[1] == 0X0)) return 1;
                else return 0;
            }
            else return 5;

        }

        private int UpdateCard(byte adres)
        {
            Sleep(10);
            SendPort(Hexcon.CharToByteArray('C'));
            ReadPort(ref buffer, 1);
            if ((buffer[0] != 99) || ((char)buffer[0] == '0')) return 0;

            SendPort(Hexcon.CharToByteArray('A'));
            ReadPort(ref buffer, 1);
            if (((char)buffer[0] != 'a') || ((char)buffer[0] == '0')) return 0;

            SendPort(Hexcon.CharToByteArray('G'));
            ReadPort(ref buffer, 1);
            if ((char)buffer[0] == 'Q') return 5;
            else if ((char)buffer[0] == 'g')
            {
                SendPort(Hexcon.ByteToByteArray(adres));
            }
            else return 0;


            ReadPort(ref buffer, 1);
            if (buffer[0] != adres) return 0;

            SendPort(outBuf); //Sendabonecsc den sonra outbufın ilk 4 byte ı dolduruluyor
            ReadPort(ref buffer, 2);

            if ((char)buffer[0] != 'Q')
            {
                if ((buffer[0] == 0X90) && (buffer[1] == 0X0)) return 1;
                else return 0;
            }
            else return 5;

        }

        private int KartCikart()
        {
            Sleep(10);
            SendPort(Hexcon.CharToByteArray('C'));
            ReadPort(ref buffer, 1);
            if ((buffer[0] != 99) || ((char)buffer[0] == '0')) return 0;

            SendPort(Hexcon.CharToByteArray('A'));
            ReadPort(ref buffer, 1);
            if (((char)buffer[0] != 'a') || ((char)buffer[0] == '0')) return 0;

            SendPort(Hexcon.CharToByteArray('P'));

            return 1;
        }

        #endregion

        private byte[] GetIssuer(Int32 zone)
        {
            byte[] issue_area = new byte[2];
            //48 = 0, 65 = A, 66 = B, 67 = C, 49 = 1, 

            if (zone == 1)
            {
                issue_area[0] = (byte)'A';
                issue_area[1] = (byte)'A';
            }
            else
                if (zone < 11)
                {
                    issue_area[0] = (byte)'A';
                    issue_area[1] = (byte)(49 + zone - 2);
                }
                else
                    if (zone < 36)
                    {
                        issue_area[0] = (byte)'A';
                        issue_area[1] = (byte)(66 + zone - 11); //'B';
                    }
                    else
                        if (zone < 72)
                        {
                            issue_area[0] = (byte)'B';
                            if (zone < 46)
                                issue_area[1] = (byte)(48 + zone - 36);
                            else
                                issue_area[1] = (byte)(65 + zone - 46);

                        }
                        else
                            if (zone < 108)
                            {
                                issue_area[0] = (byte)'C';
                                if (zone < 82)
                                    issue_area[1] = (byte)(48 + zone - 72);
                                else
                                    issue_area[1] = (byte)(65 + zone - 82);
                            }
                            else
                                if (zone < 144)
                                {
                                    issue_area[0] = (byte)'D';
                                    if (zone < 118)
                                        issue_area[1] = (byte)(48 + zone - 108);
                                    else
                                        issue_area[1] = (byte)(65 + zone - 118);
                                }

            return issue_area;
        }

        #region Abone Kartı Fonksyonları

        public string AboneOku()
        {
            string str = "";
            byte[] issue_area = GetIssuer(zone);
            int i = 0;

            #region Init

            //InitPort();
            //OpenPort();
            i = InitCard();
            //Sleep(10);
            if (i == 5) return "5";
            else if (i == 0) return "0";
            i = ReadCard(1);
            //Sleep(10);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'A') || (inBuf[3] != ' '))
            {
                ClosePort();
                return "0";
            }

            #endregion

            #region Csc

            i = ReadCard(0X30);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            UInt32 devNo = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);
            Sleep(10);

            UInt32 sr = SendAboneCsc(devNo, 0X3D3D);
            outBuf[0] = (byte)(sr / 256);
            outBuf[1] = (byte)(sr % 256);
            Sleep(10);
            sr = SendAboneCsc(devNo, 0X5A5A);
            outBuf[2] = (byte)(sr / 256);
            outBuf[3] = (byte)(sr % 256);
            Sleep(10);
            i = VerifyCard(7); // rat counter adresi
            if (i == 5) return "5";
            else if (i == 0) return "0";

            sr = SendAboneCsc(devNo, 0X2F2F);
            outBuf[0] = (byte)(sr / 256);
            outBuf[1] = (byte)(sr % 256);
            Sleep(10);
            sr = SendAboneCsc(devNo, 0X1515);
            outBuf[2] = (byte)(sr / 256);
            outBuf[3] = (byte)(sr % 256);
            Sleep(10);
            i = VerifyCard(0X39); // rat counter adresi
            if (i == 5) return "5";
            else if (i == 0) return "0";


            sr = SendAboneCsc(devNo, 0XABAB);
            outBuf[0] = (byte)(sr / 256);
            outBuf[1] = (byte)(sr % 256);
            Sleep(10);
            sr = SendAboneCsc(devNo, 0XC2C2);
            outBuf[2] = (byte)(sr / 256);
            outBuf[3] = (byte)(sr % 256);
            Sleep(10);
            i = VerifyCard(0X3B); // rat counter adresi
            if (i == 5) return "5";
            else if (i == 0) return "0";

            #endregion

            #region Kart Bilgileri

            i = ReadCard(0X10);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            UInt32 kad1 = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X11);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            UInt32 kad2 = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X12);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            UInt32 kad3 = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X13);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            UInt32 lim1 = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X14);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            UInt32 lim2 = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X15);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            UInt32 loadLim = (UInt32)inBuf[0];
            UInt32 aksam = (UInt32)inBuf[1];
            UInt32 sabah = (UInt32)inBuf[2];
            UInt32 kademe = (UInt32)inBuf[3];

            i = ReadCard(0X16);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            UInt32 hSonuAksam = (UInt32)inBuf[0];

            i = ReadCard(0X17);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            UInt32 fixCharge = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X18);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            UInt32 totalFixCharge = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);


            i = ReadCard(0X35);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            UInt32 elkAna = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X36);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            UInt32 elkKalan = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X29);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            UInt32 elkHarcanan = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X37);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            UInt32 k1Tuk = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X28);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            UInt32 k2Tuk = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X2A);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            UInt32 k3Tuk = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X31);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            UInt32 gerTuk = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X02);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            UInt32 elkEkim = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X03);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            UInt32 elkAralik = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X1E);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            string elkTarih = Hexcon.TarihOlustur(Hexcon.ByteDiziOlustur(inBuf, 0, 2));

            i = ReadCard(0X3F);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            string ako = "";
            string yko = "";

            if (((int)inBuf[0] & (int)0X80) == 0) ako = "*"; else ako = "b";
            if (((int)inBuf[0] & (int)0X40) == 0) yko = "*"; else yko = "b";


            string KlemensCeza = "";
            string Ariza = "";
            string PilZayif = "";
            string PilBitik = "";

            Hexcon.ByteToBit state = new Hexcon.ByteToBit(inBuf[1]);
            KlemensCeza = state.Bit1.ToString();
            Ariza = state.Bit2.ToString();
            PilZayif = state.Bit4.ToString();
            PilBitik = state.Bit5.ToString();

            string yeniKart = "";
            if (((int)inBuf[2] & (int)0X40) == 0) yeniKart = "0"; else yeniKart = "1";


            i = ReadCard(0X34);
            if (i == 5) return "5";
            else if (i == 0) return "0";
            UInt32 yedekkredi = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X2E);
            if (i == 5) return "5";
            else if (i == 0) return "0";
            UInt32 kritikkredi = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            #endregion

            str += devNo + sprtr + elkAna + sprtr + ako + sprtr + yko + sprtr + yeniKart + sprtr + elkKalan + sprtr + elkHarcanan + sprtr + elkTarih + sprtr +
                   KlemensCeza + sprtr + Ariza + sprtr + PilZayif + sprtr + PilBitik + sprtr + k1Tuk + sprtr + k2Tuk + sprtr + k3Tuk + sprtr + gerTuk + sprtr +
                   elkEkim + sprtr + elkAralik + sprtr + kad1 + sprtr + kad2 + sprtr + kad3 + sprtr + lim1 + sprtr + lim2 + sprtr + loadLim + sprtr + aksam + sprtr +
                   sabah + sprtr + kademe + sprtr + hSonuAksam + sprtr + fixCharge + sprtr + totalFixCharge + sprtr + yedekkredi + sprtr + kritikkredi;

            ClosePort();
            return "1" + sprtr + str;
        }

        public string KrediYaz(uint devNo, uint AnaKredi, uint YedekKredi, ITarife trf, byte Bayram1Gunn, byte Bayram1Ayy, byte Bayram1Suree, byte Bayram2Gunn, byte Bayram2Ayy, byte Bayram2Suree, uint KritikKredi)
        {
            byte[] issue_area = GetIssuer(zone);
            int i = 0;

            if (((trf.Fiyat1 == trf.Fiyat2) && (trf.Fiyat2 == trf.Fiyat3)) || (trf.Lim1 >= 5000000) || (trf.Lim2 >= 5000000))
            {
                trf.Lim1 = 10000;
                trf.Lim2 = 10000;

                trf.Fiyat1 = 1;
                trf.Fiyat2 = 1;
                trf.Fiyat3 = 1;
            }

            #region Init
            //InitPort();
            //OpenPort();

            i = InitCard();
            if (i == 5) return "5";
            else if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'A'))
            {
                ClosePort();
                return "0";
            }

            #endregion

            #region csc

            UInt32 sr = SendAboneCsc(devNo, 0X3D3D);
            outBuf[0] = (byte)(sr / 256);
            outBuf[1] = (byte)(sr % 256);
            //Sleep(10);
            sr = SendAboneCsc(devNo, 0X5A5A);
            outBuf[2] = (byte)(sr / 256);
            outBuf[3] = (byte)(sr % 256);
            //Sleep(10);
            i = VerifyCard(7); // rat counter adresi
            if (i == 5) return "5";
            else if (i == 0) return "0";

            sr = SendAboneCsc(devNo, 0X2F2F);
            outBuf[0] = (byte)(sr / 256);
            outBuf[1] = (byte)(sr % 256);
            //Sleep(10);
            sr = SendAboneCsc(devNo, 0X1515);
            outBuf[2] = (byte)(sr / 256);
            outBuf[3] = (byte)(sr % 256);
            //Sleep(10);
            i = VerifyCard(0X39); // rat counter adresi
            if (i == 5) return "5";
            else if (i == 0) return "0";
            sr = SendAboneCsc(devNo, 0XABAB);
            outBuf[0] = (byte)(sr / 256);
            outBuf[1] = (byte)(sr % 256);
            //Sleep(10);
            sr = SendAboneCsc(devNo, 0XC2C2);
            outBuf[2] = (byte)(sr / 256);
            outBuf[3] = (byte)(sr % 256);
            //Sleep(10);
            i = VerifyCard(0X3B); // rat counter adresi
            if (i == 5) return "5";
            else if (i == 0) return "0";



            #endregion

            #region işlem ve yeni kart kontrolü

            i = ReadCard(0X3F);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.ByteToBit learnKart = new Hexcon.ByteToBit(inBuf[2]);
            UInt32 islem = (UInt32)inBuf[3];

            if (((int)inBuf[0] & (0X80)) == 0) islem++;

            if (learnKart.Bit7 != 1) // yeni kart değilse artır
            {
                if (((int)inBuf[0] & (0X40)) == 0) islem++;
            }
            learnKart.Deger = Hexcon.SetBitZero(learnKart.Deger); // hata set
            learnKart.Deger = Hexcon.SetBitBir(learnKart.Deger, 0X30); // major set
            outBuf[0] = 0;
            outBuf[1] = 0;
            outBuf[2] = 0;
            outBuf[3] = 0;

            i = UpdateCard(0X36);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            i = UpdateCard(0X37);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            i = UpdateCard(0X28);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            i = UpdateCard(0X2A);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            i = UpdateCard(0X2B);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            i = UpdateCard(0X31);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            i = UpdateCard(0X29);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            UInt32 Kad1 = 10000;
            UInt32 Kad2 = 10000 * trf.Fiyat2 / trf.Fiyat1;
            UInt32 Kad3 = 10000 * trf.Fiyat3 / trf.Fiyat1;

            Hexcon.UInt32toByte4(Kad1, outBuf);
            i = UpdateCard(0X10);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(Kad2, outBuf);
            i = UpdateCard(0X11);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(Kad3, outBuf);
            i = UpdateCard(0X12);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(trf.Lim1, outBuf);
            i = UpdateCard(0X13);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(trf.Lim2, outBuf);
            i = UpdateCard(0X14);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(0, outBuf);
            outBuf[0] = (byte)trf.LoadLimit;
            outBuf[1] = (byte)trf.Aksam;
            outBuf[2] = (byte)trf.Sabah;
            i = UpdateCard(0X15);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(0, outBuf);
            outBuf[0] = (byte)trf.HaftaSonuAksam;
            i = UpdateCard(0X16);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(trf.FixCharge, outBuf);
            i = UpdateCard(0X17);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            UInt16 Bayram1, Bayram2;

            Bayram1 = Bayram1Suree;
            Bayram1 <<= 5;
            Bayram1 |= Bayram1Gunn;
            Bayram1 <<= 4;
            Bayram1 |= Bayram1Ayy;

            Bayram2 = Bayram2Suree;
            Bayram2 <<= 5;
            Bayram2 |= Bayram2Gunn;
            Bayram2 <<= 4;
            Bayram2 |= Bayram2Ayy;

            Integer2Byte Bayram1Deger = new Integer2Byte(Bayram1);
            Integer2Byte Bayram2Deger = new Integer2Byte(Bayram2);

            outBuf[0] = Bayram1Deger.bir;
            outBuf[1] = Bayram1Deger.iki;
            outBuf[2] = Bayram2Deger.bir;
            outBuf[3] = Bayram2Deger.iki;

            i = UpdateCard(0X2C);
            if (i == 5) return "5";
            else if (i == 0) return "0";


            if (learnKart.Bit7 == 1)
            {
                AnaKredi = AnaKredi + YedekKredi;
                outBuf[0] = 0XBF;
            }
            else outBuf[0] = 0XC0;
            outBuf[1] = 0XFF;

            outBuf[2] = (byte)learnKart.Deger;
            outBuf[3] = (byte)islem;

            i = UpdateCard(0X3F);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(AnaKredi, outBuf);
            i = UpdateCard(0X35);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(YedekKredi, outBuf);
            i = UpdateCard(0X34);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(KritikKredi, outBuf);
            i = UpdateCard(0X2E);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            #endregion

            ClosePort();
            return "1";
        }

        public string KrediOku()
        {
            string str = "";
            byte[] issue_area = GetIssuer(zone);
            int i = 0;

            #region Init

            //InitPort();
            //OpenPort();

            i = InitCard();
            //Sleep(10);
            if (i == 5) return "5";
            else if (i == 0) return "0";
            i = ReadCard(1);
            //Sleep(10);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'A') || (inBuf[3] != ' '))
            {
                ClosePort();
                return "0";
            }

            #endregion

            #region Csc

            i = ReadCard(0X30);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            UInt32 devNo = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);
            str += devNo.ToString();
            Sleep(10);

            UInt32 sr = SendAboneCsc(devNo, 0X3D3D);
            outBuf[0] = (byte)(sr / 256);
            outBuf[1] = (byte)(sr % 256);
            Sleep(10);
            sr = SendAboneCsc(devNo, 0X5A5A);
            outBuf[2] = (byte)(sr / 256);
            outBuf[3] = (byte)(sr % 256);
            Sleep(10);
            i = VerifyCard(7); // rat counter adresi
            if (i == 5) return "5";
            else if (i == 0) return "0";

            sr = SendAboneCsc(devNo, 0X2F2F);
            outBuf[0] = (byte)(sr / 256);
            outBuf[1] = (byte)(sr % 256);
            Sleep(10);
            sr = SendAboneCsc(devNo, 0X1515);
            outBuf[2] = (byte)(sr / 256);
            outBuf[3] = (byte)(sr % 256);
            Sleep(10);
            i = VerifyCard(0X39); // rat counter adresi
            if (i == 5) return "5";
            else if (i == 0) return "0";


            sr = SendAboneCsc(devNo, 0XABAB);
            outBuf[0] = (byte)(sr / 256);
            outBuf[1] = (byte)(sr % 256);
            Sleep(10);
            sr = SendAboneCsc(devNo, 0XC2C2);
            outBuf[2] = (byte)(sr / 256);
            outBuf[3] = (byte)(sr % 256);
            Sleep(10);
            i = VerifyCard(0X3B); // rat counter adresi
            if (i == 5) return "5";
            else if (i == 0) return "0";

            #endregion

            #region Kart Bilgileri
            i = ReadCard(0X35);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            UInt32 elkAna = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);
            str += sprtr + elkAna;


            i = ReadCard(0X3F);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            string ako = "";
            string yko = "";

            if (((int)inBuf[0] & (int)0X80) == 0) ako = "*"; else ako = "b";
            if (((int)inBuf[0] & (int)0X40) == 0) yko = "*"; else yko = "b";

            str += sprtr + ako + sprtr + yko;

            string yeniKart = "";
            if (((int)inBuf[2] & (int)0X40) == 0) yeniKart = "0"; else yeniKart = "1";
            str += sprtr + yeniKart;

            i = ReadCard(0X17);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            UInt32 fixCharge = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);
            str += sprtr + fixCharge;

            i = ReadCard(0X18);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            UInt32 totalFixCharge = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);
            str += sprtr + totalFixCharge;

            #endregion

            ClosePort();
            return "1" + sprtr + str;

        }

        public string AboneYap(UInt32 devNo, ITarife trf, byte Bayram1Gunn, byte Bayram1Ayy, byte Bayram1Suree, byte Bayram2Gunn, byte Bayram2Ayy, byte Bayram2Suree, uint KritikKredi)
        {
            byte[] issue_area = GetIssuer(zone);
            int i = 0;

            if (((trf.Fiyat1 == trf.Fiyat2) && (trf.Fiyat2 == trf.Fiyat3)) || (trf.Lim1 >= 5000000) || (trf.Lim2 >= 5000000))
            {
                trf.Lim1 = 10000;
                trf.Lim2 = 10000;

                trf.Fiyat1 = 1;
                trf.Fiyat2 = 1;
                trf.Fiyat3 = 1;
            }

            #region Init
            //InitPort();
            //OpenPort();

            i = InitCard();
            if (i == 5) return "5";
            else if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            outBuf[0] = 0X79;
            outBuf[1] = 0X81;
            outBuf[2] = 0X17;
            outBuf[3] = 0X2F;

            i = VerifyCard(7); // rat counter adresi
            if (i == 5) return "5";
            else if (i == 0) return "0";

            outBuf[0] = 0X2E;
            outBuf[1] = 0X4C;
            outBuf[2] = 0X16;
            outBuf[3] = 0X41;

            i = VerifyCard(0X39); // rat counter adresi
            if (i == 5) return "5";
            else if (i == 0) return "0";

            outBuf[0] = 0X22;
            outBuf[1] = 0X22;
            outBuf[2] = 0X22;
            outBuf[3] = 0X22;

            i = VerifyCard(0X3B); // rat counter adresi
            if (i == 5) return "5";
            else if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'A') || (inBuf[3] != ' '))
            {
                outBuf[0] = issue_area[0];
                outBuf[1] = issue_area[1];
                outBuf[2] = (byte)'A';
                outBuf[3] = (byte)' ';

                i = UpdateCard(1);
                if (i == 5) return "5";
                else if (i == 0) return "0";
            }

            #endregion

            #region csc

            UInt32 sr = SendAboneCsc(devNo, 0X3D3D);
            outBuf[0] = (byte)(sr / 256);
            outBuf[1] = (byte)(sr % 256);

            sr = SendAboneCsc(devNo, 0X5A5A);
            outBuf[2] = (byte)(sr / 256);
            outBuf[3] = (byte)(sr % 256);

            i = UpdateCard(6);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            sr = SendAboneCsc(devNo, 0X2F2F);
            outBuf[0] = (byte)(sr / 256);
            outBuf[1] = (byte)(sr % 256);

            sr = SendAboneCsc(devNo, 0X1515);
            outBuf[2] = (byte)(sr / 256);
            outBuf[3] = (byte)(sr % 256);

            i = UpdateCard(0X38);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            sr = SendAboneCsc(devNo, 0XABAB);
            outBuf[0] = (byte)(sr / 256);
            outBuf[1] = (byte)(sr % 256);

            sr = SendAboneCsc(devNo, 0XC2C2);
            outBuf[2] = (byte)(sr / 256);
            outBuf[3] = (byte)(sr % 256);

            i = UpdateCard(0X3A);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            #endregion

            #region  Karta yazma

            Hexcon.UInt32toByte4(devNo, outBuf);
            i = UpdateCard(0X30);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            UInt32 kad1 = 10000;
            UInt32 kad2 = 10000 * trf.Fiyat2 / trf.Fiyat1;
            UInt32 kad3 = 10000 * trf.Fiyat3 / trf.Fiyat1;

            Hexcon.UInt32toByte4(kad1, outBuf);
            i = UpdateCard(0X10);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(kad2, outBuf);
            i = UpdateCard(0X11);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(kad3, outBuf);
            i = UpdateCard(0X12);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(trf.Lim1, outBuf);
            i = UpdateCard(0X13);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(trf.Lim2, outBuf);
            i = UpdateCard(0X14);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(0, outBuf);
            outBuf[0] = (byte)trf.LoadLimit;
            outBuf[1] = (byte)trf.Aksam;
            outBuf[2] = (byte)trf.Sabah;

            i = UpdateCard(0X15);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(0, outBuf);
            outBuf[0] = (byte)trf.HaftaSonuAksam;

            i = UpdateCard(0X16);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(trf.FixCharge, outBuf);
            i = UpdateCard(0X17);
            if (i == 5) return "5";
            else if (i == 0) return "0";



            UInt16 Bayram1, Bayram2;

            Bayram1 = Bayram1Suree;
            Bayram1 <<= 5;
            Bayram1 |= Bayram1Gunn;
            Bayram1 <<= 4;
            Bayram1 |= Bayram1Ayy;

            Bayram2 = Bayram2Suree;
            Bayram2 <<= 5;
            Bayram2 |= Bayram2Gunn;
            Bayram2 <<= 4;
            Bayram2 |= Bayram2Ayy;

            Integer2Byte Bayram1Deger = new Integer2Byte(Bayram1);
            Integer2Byte Bayram2Deger = new Integer2Byte(Bayram2);

            outBuf[0] = Bayram1Deger.bir;
            outBuf[1] = Bayram1Deger.iki;
            outBuf[2] = Bayram2Deger.bir;
            outBuf[3] = Bayram2Deger.iki;

            i = UpdateCard(0X2C);
            if (i == 5) return "5";
            else if (i == 0) return "0";


            int ekarno = 0;
            ekarno &= 0X0F;
            ekarno |= 0X70;
            outBuf[0] = 0XBF;  //  ff idi yeni kart hazirlarken okunmadi-okundu olarak hazirla
            outBuf[1] = 0XFF; // sayaç state
            outBuf[2] = (byte)ekarno;
            outBuf[3] = 0;

            i = UpdateCard(0X3F);
            if (i == 5) return "5";
            else if (i == 0) return "0";


            Hexcon.UInt32toByte4(KritikKredi, outBuf);
            i = UpdateCard(0X2E);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            //outBuf[0] = 0;
            //outBuf[1] = 0;
            //outBuf[2] = 0;
            //outBuf[3] = 0;
            //for (int t = 0X19; t < 0X20; t++)
            //{
            //    i = UpdateCard((byte)t);
            //    if (i == 5) return "5";
            //    else if (i == 0) return "0";
            //}


            //for (int j = 0X2B; j < 0X38; j++)
            //{
            //    if (j == 0x30) j = 0x31;
            //    else if (j == 0x2c) j = 0x2d;
            //    else if (j == 0x2f) j = 0x31;

            //    i = UpdateCard((byte)j);
            //    if (i == 5) return "5";
            //    else if (i == 0) return "0";
            //}


            #endregion

            ClosePort();

            return "1";
        }

        public string AboneBosalt()
        {
            string str = "";
            byte[] issue_area = GetIssuer(zone);
            int i = 0;

            #region Init

            //InitPort();
            //OpenPort();

            i = InitCard();
            //Sleep(10);
            if (i == 5) return "5";
            else if (i == 0) return "0";
            i = ReadCard(1);
            //Sleep(10);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'A'))
            {
                ClosePort();
                return "0";
            }

            #endregion

            #region Csc

            i = ReadCard(0X30);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            UInt32 devNo = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);
            str += devNo.ToString();
            Sleep(10);

            UInt32 sr = SendAboneCsc(devNo, 0X3D3D);
            outBuf[0] = (byte)(sr / 256);
            outBuf[1] = (byte)(sr % 256);
            Sleep(10);
            sr = SendAboneCsc(devNo, 0X5A5A);
            outBuf[2] = (byte)(sr / 256);
            outBuf[3] = (byte)(sr % 256);
            Sleep(10);
            i = VerifyCard(7); // rat counter adresi
            if (i == 5) return "5";
            else if (i == 0) return "0";

            sr = SendAboneCsc(devNo, 0X2F2F);
            outBuf[0] = (byte)(sr / 256);
            outBuf[1] = (byte)(sr % 256);
            Sleep(10);
            sr = SendAboneCsc(devNo, 0X1515);
            outBuf[2] = (byte)(sr / 256);
            outBuf[3] = (byte)(sr % 256);
            Sleep(10);
            i = VerifyCard(0X39); // rat counter adresi
            if (i == 5) return "5";
            else if (i == 0) return "0";


            sr = SendAboneCsc(devNo, 0XABAB);
            outBuf[0] = (byte)(sr / 256);
            outBuf[1] = (byte)(sr % 256);
            Sleep(10);
            sr = SendAboneCsc(devNo, 0XC2C2);
            outBuf[2] = (byte)(sr / 256);
            outBuf[3] = (byte)(sr % 256);
            Sleep(10);
            i = VerifyCard(0X3B); // rat counter adresi
            if (i == 5) return "5";
            else if (i == 0) return "0";

            #endregion

            #region Bosaltma işlemleri

            outBuf[0] = 0x00;
            outBuf[1] = 0x00;
            outBuf[2] = 0x00;
            outBuf[3] = 0x00;

            for (int j = 0x10; j < 0x20; j++)
            {
                i = UpdateCard((byte)j);
                if (i == 5) return "5";
                else if (i == 0) return "0";
            }

            for (int j = 0x28; j < 0x40; j++)
            {
                if (j == 0x38) j = 0x3C;
                i = UpdateCard((byte)j);
                if (i == 5) return "5";
                else if (i == 0) return "0";
            }

            i = UpdateCard(0X01);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            outBuf[0] = 0x79;
            outBuf[1] = 0x81;
            outBuf[2] = 0x17;
            outBuf[3] = 0x2F;

            i = UpdateCard(6);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            outBuf[0] = 0x2E;
            outBuf[1] = 0x4C;
            outBuf[2] = 0x16;
            outBuf[3] = 0x41;

            i = UpdateCard(0X38);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            outBuf[0] = 0x22;
            outBuf[1] = 0x22;
            outBuf[2] = 0x22;
            outBuf[3] = 0x22;

            i = UpdateCard(0X3A);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            #endregion

            ClosePort();
            return "1";

        }

        public string SifreExeHazirla()
        {
            string str = "";
            byte[] issue_area = GetIssuer(zone);
            int i = 0;
                      

            //InitPort();
            //OpenPort();
            //Sleep(1000);
            i = InitCard();
            //Sleep(1000);
            if (i == 5) return "5";
            else if (i == 0) return "0";
            i = ReadCard(1);
            //Sleep(1000);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            if ((inBuf[0] != 0) || (inBuf[1] != 0) || (inBuf[2] != 0))
            {
                ClosePort();
                return "0";
            }

            outBuf[0] = 0XAA;
            outBuf[1] = 0XAA;
            outBuf[2] = 0XAA;
            outBuf[3] = 0XAA;

            i = VerifyCard(0X07);

            if (i == 1)
            {
                outBuf[0] = 0X11;
                outBuf[1] = 0X11;
                outBuf[2] = 0X11;
                outBuf[3] = 0X11;

                i = VerifyCard(0X39);

                if (i == 1)
                {
                    outBuf[0] = 0X79;
                    outBuf[1] = 0X81;
                    outBuf[2] = 0X17;
                    outBuf[3] = 0X2F;

                    i = UpdateCard(6);

                    if (i == 1)
                    {
                        outBuf[0] = 0X2E;
                        outBuf[1] = 0X4C;
                        outBuf[2] = 0X16;
                        outBuf[3] = 0X41;

                        i = UpdateCard(0X38);

                        FinishCard();
                        return i.ToString();
                    }
                }
            }

            //Sleep(1000);
            FinishCard();
            return "0";
        }

        public string KartSifreBosalt()
        {
            int i = 0;

            i = InitCard();
            if (i == 0) { FinishCard(); return "0"; }

            i = ReadCard(1);
            if (i == 0) { FinishCard(); return "0"; }
            if ((inBuf[0] != 0) || (inBuf[1] != 0) || (inBuf[2] != 0))
            {
                FinishCard();
                return "0";
            }

            outBuf[0] = 0X79;
            outBuf[1] = 0X81;
            outBuf[2] = 0X17;
            outBuf[3] = 0X2F;

            i = VerifyCard(7);
            if (i == 0) { FinishCard(); return "0"; }

            outBuf[0] = 0X2E;
            outBuf[1] = 0X4C;
            outBuf[2] = 0X16;
            outBuf[3] = 0X41;

            i = VerifyCard(0X39);
            if (i == 0) { FinishCard(); return "0"; }

            outBuf[0] = 0XAA;
            outBuf[1] = 0XAA;
            outBuf[2] = 0XAA;
            outBuf[3] = 0XAA;

            i = UpdateCard(6);
            if (i == 0) { FinishCard(); return "0"; }

            outBuf[0] = 0X11;
            outBuf[1] = 0X11;
            outBuf[2] = 0X11;
            outBuf[3] = 0X11;

            i = UpdateCard(0X38);
            if (i == 0) { FinishCard(); return "0"; }

            FinishCard();

            return "1";
        }

        public string KartTipi()
        {
            int i = 0;

            i = InitCard();
            Sleep(10);
            if (i == 5) return "5";
            else if (i == 0) return "0";            

            i = ReadCard(1);
            if (i == 0) { FinishCard(); return "0"; }

            //FinishCard();

            string stIssuer = Convert.ToChar(inBuf[0]).ToString() + Convert.ToChar(inBuf[1]).ToString() + Convert.ToChar(inBuf[2]).ToString() + Convert.ToChar(inBuf[3]).ToString();

            if ((inBuf[0] == 0) || (inBuf[1] == 0) || (inBuf[2] == 0))
            {
                stIssuer = "Bos Kart";
            }           

            return stIssuer;
        }

        public string Eject()
        {
            //InitPort();
            //OpenPort();

            string str = KartCikart().ToString();
            ClosePort();

            return str;
        }



        #endregion

        #region Yetki Kartı Fonksyonları
        public string FormYet()
        {
            byte[] issue_area = GetIssuer(zone);
            int i = 0;


            #region Init

            //InitPort();
            //OpenPort();

            i = InitCard();
            Sleep(10);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            i = ReadCard(1);
            Sleep(10);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            if ((inBuf[0] != 0 ) || (inBuf[1] != 0 ) || (inBuf[2] != 0 ))
            {
                ClosePort();
                return "0";
            }

            #endregion

            #region Formyet işlemleri

            outBuf[0] = 0xAA;
            outBuf[1] = 0xAA;
            outBuf[2] = 0xAA;
            outBuf[3] = 0xAA;

            i = VerifyCard(7);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            outBuf[0] = issue_area[0];
            outBuf[1] = issue_area[1];
            outBuf[2] = (byte)'Y';
            outBuf[3] = (byte)'A';

            i = UpdateCard(1);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            outBuf[0] = 0x3A;
            outBuf[1] = 0xDF;
            outBuf[2] = 0x1D;
            outBuf[3] = 0x80;

            i = UpdateCard(6);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            #endregion

            ClosePort();
            return "1";
        }

        public string YetkiSaat()
        {
            byte[] issue_area = GetIssuer(zone);
            int i = 0;


            #region Init

            //InitPort();
            //OpenPort();

            i = InitCard();
            Sleep(10);
            if (i == 5) return "5";
            else if (i == 0) return "0";
            i = ReadCard(1);
            Sleep(10);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'Y'))
            {
                ClosePort();
                return "0";
            }

            #endregion

            #region Saat kartı işlemleri

            outBuf[0] = 0x3A;
            outBuf[1] = 0xDF;
            outBuf[2] = 0x1D;
            outBuf[3] = 0x80;

            i = VerifyCard(7);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            outBuf[0] = issue_area[0];
            outBuf[1] = issue_area[1];
            outBuf[2] = (byte)'Y';
            outBuf[3] = (byte)'S';

            i = UpdateCard(1);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            DateTime tar = DateTime.Now;

            TarihAl tarih = new TarihAl(tar);
            outBuf = tarih.TarihDondur4Byte(tar);

            i = UpdateCard(0x10);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            outBuf[0] = 0;
            outBuf[1] = 0;
            outBuf[2] = 0;
            outBuf[3] = 0;

            i = UpdateCard(0X12);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            i = ReadCard(0X3D);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            DayOfWeek dow = tar.DayOfWeek;

            outBuf[0] = (byte)dow;
            outBuf[1] = inBuf[1];
            outBuf[2] = 0;
            outBuf[3] = inBuf[3];

            i = UpdateCard(0x3D);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            #endregion

            ClosePort();
            return "1";
        }

        public string YetkiSaat(DateTime date)
        {
            byte[] issue_area = GetIssuer(zone);
            int i = 0;


            #region Init

            //InitPort();
            //OpenPort();

            i = InitCard();
            Sleep(10);
            if (i == 5) return "5";
            else if (i == 0) return "0";
            i = ReadCard(1);
            Sleep(10);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'Y'))
            {
                ClosePort();
                return "0";
            }

            #endregion

            #region Saat kartı işlemleri

            outBuf[0] = 0x3A;
            outBuf[1] = 0xDF;
            outBuf[2] = 0x1D;
            outBuf[3] = 0x80;

            i = VerifyCard(7);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            outBuf[0] = issue_area[0];
            outBuf[1] = issue_area[1];
            outBuf[2] = (byte)'Y';
            outBuf[3] = (byte)'S';

            i = UpdateCard(1);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            DateTime tar = date;

            TarihAl tarih = new TarihAl(tar);
            outBuf = tarih.TarihDondur4Byte(tar);

            i = UpdateCard(0x10);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            outBuf[0] = 0;
            outBuf[1] = 0;
            outBuf[2] = 0;
            outBuf[3] = 0;

            i = UpdateCard(0X12);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            i = ReadCard(0X3D);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            DayOfWeek dow = tar.DayOfWeek;

            outBuf[0] = (byte)dow;
            outBuf[1] = inBuf[1];
            outBuf[2] = 0;
            outBuf[3] = inBuf[3];

            i = UpdateCard(0x3D);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            #endregion

            ClosePort();
            return "1";
        }

        public string YetkiAc(UInt32 devNo)
        {

            byte[] issue_area = GetIssuer(zone);
            int i = 0;


            #region Init

            //InitPort();
            //OpenPort();

            i = InitCard();
            Sleep(10);
            if (i == 5) return "5";
            else if (i == 0) return "0";
            i = ReadCard(1);
            Sleep(10);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'Y'))
            {
                ClosePort();
                return "0";
            }

            #endregion

            #region Açma işlemleri

            outBuf[0] = 0x3A;
            outBuf[1] = 0xDF;
            outBuf[2] = 0x1D;
            outBuf[3] = 0x80;

            i = VerifyCard(7);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            outBuf[0] = issue_area[0];
            outBuf[1] = issue_area[1];
            outBuf[2] = (byte)'Y';
            outBuf[3] = (byte)'A';

            i = UpdateCard(1);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(devNo, outBuf);
            i = UpdateCard(0X3C);
            if (i == 5) return "5";
            else if (i == 0) return "0";


            outBuf[0] = 0X0;
            outBuf[1] = 0X0;
            outBuf[2] = 0X0;
            outBuf[3] = 0X0;


            i = UpdateCard(0X3D);
            if (i == 5) return "5";
            else if (i == 0) return "0";


            i = UpdateCard(0x10);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            #endregion

            ClosePort();
            return "1";


        }

        public string YetkiKapat(UInt32 devNo)
        {

            byte[] issue_area = GetIssuer(zone);
            int i = 0;


            #region Init

            //InitPort();
            //OpenPort();
            
            i = InitCard();
            Sleep(10);
            if (i == 5) return "5";
            else if (i == 0) return "0";
            i = ReadCard(1);
            Sleep(10);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'Y'))
            {
                ClosePort();
                return "0";
            }

            #endregion

            #region Kapatma işlemleri

            outBuf[0] = 0x3A;
            outBuf[1] = 0xDF;
            outBuf[2] = 0x1D;
            outBuf[3] = 0x80;

            i = VerifyCard(7);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            outBuf[0] = issue_area[0];
            outBuf[1] = issue_area[1];
            outBuf[2] = (byte)'Y';
            outBuf[3] = (byte)'K';

            i = UpdateCard(1);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(devNo, outBuf);
            i = UpdateCard(0X3C);
            if (i == 5) return "5";
            else if (i == 0) return "0";


            outBuf[0] = 0X0;
            outBuf[1] = 0X0;
            outBuf[2] = 0X0;
            outBuf[3] = 0X0;


            i = UpdateCard(0x10);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            #endregion

            ClosePort();
            return "1";

        }

        public string YetkiBilgiYap()
        {
            byte[] issue_area = GetIssuer(zone);
            int i = 0;


            #region Init

            //InitPort();
            //OpenPort();

            i = InitCard();
            Sleep(10);
            if (i == 5) return "5";
            else if (i == 0) return "0";
            i = ReadCard(1);
            Sleep(10);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'Y'))
            {
                ClosePort();
                return "0";
            }

            #endregion

            #region Bilgi yap işlemleri

            outBuf[0] = 0x3A;
            outBuf[1] = 0xDF;
            outBuf[2] = 0x1D;
            outBuf[3] = 0x80;

            i = VerifyCard(7);
            if (i == 5) return "5";
            else if (i == 0) return "0";


            outBuf[0] = issue_area[0];
            outBuf[1] = issue_area[1];
            outBuf[2] = (byte)'Y';
            outBuf[3] = (byte)'E';

            i = UpdateCard(1);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            outBuf[0] = 0XFF;
            outBuf[1] = 0XFF;
            outBuf[2] = 0XFF;
            outBuf[3] = 0XFF;

            i = UpdateCard(0X3D);
            if (i == 5) return "5";
            else if (i == 0) return "0";


            outBuf[0] = 0X0;
            outBuf[1] = 0X0;
            outBuf[2] = 0X0;
            outBuf[3] = 0X0;


            i = UpdateCard(0x3E);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            i = UpdateCard(0x10);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            i = UpdateCard(0x11);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            i = UpdateCard(0x12);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            i = UpdateCard(0x13);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            i = UpdateCard(0x14);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            i = UpdateCard(0x15);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            i = UpdateCard(0x16);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            i = UpdateCard(0x17);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            i = UpdateCard(0x18);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            i = UpdateCard(0x19);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            i = UpdateCard(0x3C);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            #endregion

            ClosePort();
            return "1";

        }

        public string YetkiBilgiOku()
        {

            TarihAl bilgi;
            TarihAl bilgionkap, bilgisonp, bilgiarzt, bilgikr;

            string str = "";
            byte[] issue_area = GetIssuer(zone);
            int i = 0;

            #region Init

            //InitPort();
            //OpenPort();

            i = InitCard();
            Sleep(10);
            if (i == 5) return "5";
            else if (i == 0) return "0";
            i = ReadCard(1);
            Sleep(10);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'Y') || (inBuf[3] != 'E'))
            {
                ClosePort();
                return "0";
            }

            #endregion

            #region Csc

            outBuf[0] = 0x3A;
            outBuf[1] = 0xDF;
            outBuf[2] = 0x1D;
            outBuf[3] = 0x80;

            i = VerifyCard(7);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            i = ReadCard(0X3C);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            if (inBuf[0] != 0x05)
            {
                ClosePort();
                return "3";
            }

            #endregion

            byte[] temp = new byte[2];

            #region Kart Bilgileri



            i = ReadCard(0X10);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Array.Copy(inBuf, 0, temp, 0, 2);
            bilgiarzt = new TarihAl(temp);

            buffer[0] = inBuf[2];
            buffer[1] = inBuf[3];

            Array.Copy(buffer, 0, temp, 0, 2);
            bilgisonp = new TarihAl(temp);

            i = ReadCard(0X11);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Array.Copy(inBuf, 0, temp, 0, 2);
            bilgionkap = new TarihAl(temp);

            buffer[0] = inBuf[2];
            buffer[1] = inBuf[3];

            Array.Copy(buffer, 0, temp, 0, 2);
            bilgikr = new TarihAl(temp);

            i = ReadCard(0X12);
            if (i == 5) return "5";
            else if (i == 0) return "0";
            UInt32 lim1 = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X14);
            if (i == 5) return "5";
            else if (i == 0) return "0";
            UInt32 lim2 = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);


            i = ReadCard(0X15);
            if (i == 5) return "5";
            else if (i == 0) return "0";
            UInt32 kad1 = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X16);
            if (i == 5) return "5";
            else if (i == 0) return "0";
            UInt32 kad2 = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X17);
            if (i == 5) return "5";
            else if (i == 0) return "0";
            UInt32 kad3 = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);


            i = ReadCard(0X18);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            UInt32 kademe = (UInt32)inBuf[0];
            UInt32 loadLim = (UInt32)inBuf[1];
            UInt32 aksam = (UInt32)inBuf[2];
            UInt32 sabah = (UInt32)inBuf[3];


            i = ReadCard(0X1A);
            if (i == 5) return "5";
            else if (i == 0) return "0";
            UInt32 hSonuAksam = (UInt32)inBuf[0];

            i = ReadCard(0X1B);
            if (i == 5) return "5";
            else if (i == 0) return "0";
            UInt32 fixCharge = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X1C);
            if (i == 5) return "5";
            else if (i == 0) return "0";
            UInt32 totalFixCharge = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X29);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            string KlemensCeza = "";
            string Ariza = "";
            string PilZayif = "";
            string PilBitik = "";

            Hexcon.ByteToBit state = new Hexcon.ByteToBit(inBuf[3]);
            KlemensCeza = state.Bit1.ToString();
            Ariza = state.Bit2.ToString();
            PilZayif = state.Bit4.ToString();
            PilBitik = state.Bit5.ToString();

            i = ReadCard(0X2D);
            if (i == 0) return "0";
            UInt32 kritikkredi = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);    

            i = ReadCard(0X2E);
            if (i == 5) return "5";
            else if (i == 0) return "0";
            bilgi = new TarihAl(inBuf);

            i = ReadCard(0X2F);
            if (i == 5) return "5";
            else if (i == 0) return "0";
            UInt32 gerTuk = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X30);
            if (i == 5) return "5";
            else if (i == 0) return "0";
            UInt32 elkHarcanan = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X31);
            if (i == 5) return "5";
            else if (i == 0) return "0";
            UInt32 elkKalan = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X32);
            if (i == 5) return "5";
            else if (i == 0) return "0";
            UInt32 devNo = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X33);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Int32 kartNo = inBuf[0];
            Int32 islem = inBuf[1];
            Int32 hangigun = inBuf[3];

            double pilsev = inBuf[2];
            double pilsev1 = pilsev;
            pilsev1 = pilsev1 / 51;
            pilsev1.ToString("F03");


            i = ReadCard(0X35);
            if (i == 5) return "5";
            else if (i == 0) return "0";
            UInt32 elkEkim = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X036);
            if (i == 5) return "5";
            else if (i == 0) return "0";
            UInt32 elkAralik = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);


            i = ReadCard(0X37);
            if (i == 5) return "5";
            else if (i == 0) return "0";
            UInt32 k1Tuk = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);


            i = ReadCard(0X3D);
            if (i == 5) return "5";
            else if (i == 0) return "0";
            UInt32 k2Tuk = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);


            i = ReadCard(0X3E);
            if (i == 5) return "5";
            else if (i == 0) return "0";
            UInt32 k3Tuk = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            i = ReadCard(0X19);
            if (i == 5) return "5";
            else if (i == 0) return "0";
            UInt32 donemTuk = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);


            #endregion

            str += devNo + sprtr + elkKalan + sprtr + elkHarcanan + sprtr + k1Tuk + sprtr + k2Tuk + sprtr + k3Tuk + sprtr + gerTuk + sprtr + bilgi.gun + "/" + bilgi.ay + "/" + bilgi.yil + " " +
                    bilgi.saat + ":" + bilgi.dakika + sprtr + bilgionkap.gun + "/" + bilgionkap.ay + "/" + bilgionkap.yil + sprtr + bilgisonp.gun + "/" + bilgisonp.ay + "/" + bilgisonp.yil + sprtr +
                    bilgiarzt.gun + "/" + bilgiarzt.ay + "/" + bilgiarzt.yil + sprtr + bilgikr.gun + "/" + bilgikr.ay + "/" + bilgikr.yil + sprtr + hangigun + sprtr +
                    kad1 + sprtr + kad2 + sprtr + kad3 + sprtr + lim1 + sprtr + lim2 + sprtr + "0" + sprtr + loadLim + sprtr + aksam + sprtr + sabah + sprtr + kademe + sprtr +
                    KlemensCeza + sprtr + Ariza + sprtr + PilZayif + sprtr + PilBitik + sprtr + elkEkim + sprtr + elkAralik + sprtr +
                    hSonuAksam + sprtr + fixCharge + sprtr + totalFixCharge + sprtr + kartNo + sprtr + donemTuk + sprtr + islem + sprtr + kritikkredi;


            ClosePort();
            return "1" + sprtr + str;
        }

        public string YetkiIadeYap(UInt32 devNo)
        {

            byte[] issue_area = GetIssuer(zone);
            int i = 0;


            #region Init

            //InitPort();
            //OpenPort();

            i = InitCard();
            Sleep(10);
            if (i == 5) return "5";
            else if (i == 0) return "0";
            i = ReadCard(1);
            Sleep(10);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'Y'))
            {
                ClosePort();
                return "0";
            }

            #endregion

            #region İade işlemleri

            outBuf[0] = 0x3A;
            outBuf[1] = 0xDF;
            outBuf[2] = 0x1D;
            outBuf[3] = 0x80;

            i = VerifyCard(7);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            outBuf[0] = issue_area[0];
            outBuf[1] = issue_area[1];
            outBuf[2] = (byte)'Y';
            outBuf[3] = (byte)'I';

            i = UpdateCard(1);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(devNo, outBuf);
            i = UpdateCard(0X3C);
            if (i == 5) return "5";
            else if (i == 0) return "0";


            outBuf[0] = 0XFF;
            outBuf[1] = 0XFF;
            outBuf[2] = 0XFF;
            outBuf[3] = 0XFF;


            i = UpdateCard(0X3D);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            outBuf[0] = 0X0;
            outBuf[1] = 0X0;
            outBuf[2] = 0X0;
            outBuf[3] = 0X0;

            i = UpdateCard(0X3E);
            if (i == 5) return "5";
            else if (i == 0) return "0";


            i = UpdateCard(0x3F);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            #endregion

            ClosePort();
            return "1";

        }

        public string YetkiIadeOku()
        {
            byte[] issue_area = GetIssuer(zone);
            int i = 0;


            #region Init

            //InitPort();
            //OpenPort();

            i = InitCard();
            if (i == 5) return "5";
            else if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'Y') || (inBuf[3] != 'I'))
            {
                ClosePort();
                return "0";
            }

            #endregion

            #region İade okuma işlemleri

            outBuf[0] = 0x3A;
            outBuf[1] = 0xDF;
            outBuf[2] = 0x1D;
            outBuf[3] = 0x80;

            i = VerifyCard(7);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            i = ReadCard(2);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            int sayacTip = inBuf[0];

            i = ReadCard(0X3C);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            UInt32 devNo = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);


            i = ReadCard(0X3D);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            int takilmismi;

            if (inBuf[3] != 0X0) takilmismi = 0;
            else takilmismi = 1;

            i = ReadCard(0X3E);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            UInt32 kredi = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);


            i = ReadCard(0X3F);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            UInt32 harcanan = (UInt32)Hexcon.ByteToDecimal(inBuf, 4);

            if (takilmismi == 0)
            {
                kredi = 0;
                harcanan = 0;
            }

            #endregion

            ClosePort();
            return "1" + sprtr + takilmismi + sprtr + devNo + sprtr + kredi + sprtr + harcanan;

        }

        public string YetkiIptal(UInt32 devNo)
        {
            byte[] issue_area = GetIssuer(zone);
            int i = 0;


            #region Init

            //InitPort();
            //OpenPort();

            i = InitCard();
            if (i == 5) return "5";
            else if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'Y'))
            {
                ClosePort();
                return "0";
            }

            #endregion

            #region İptal işlemleri

            outBuf[0] = 0x3A;
            outBuf[1] = 0xDF;
            outBuf[2] = 0x1D;
            outBuf[3] = 0x80;

            i = VerifyCard(7);
            if (i == 5) return "5";
            else if (i == 0) return "0";


            outBuf[0] = issue_area[0];
            outBuf[1] = issue_area[1];
            outBuf[2] = (byte)'Y';
            outBuf[3] = (byte)'C';

            i = UpdateCard(1);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(devNo, outBuf);
            i = UpdateCard(0X3C);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            #endregion

            ClosePort();
            return "1";

        }

        public string EDegis(UInt32 devNo, DateTime kapamaTarih, UInt32 gerTuk, UInt32 kalan, UInt32 harcanan, byte kartNo, byte islemNo,
                             byte kademe, byte loadLimit, byte aksam, byte sabah, byte haftasonuAksam, UInt32 donemTuketim,
                             UInt32 fixCharge, UInt32 totalFixCharge, UInt32 k1Tuk, UInt32 k2Tuk, UInt32 k3Tuk, UInt32 Lim1, UInt32 Lim2,
                             UInt32 kad1, UInt32 kad2, UInt32 kad3, UInt32 kritikKredi)
        {
            byte[] issue_area = GetIssuer(zone);
            int i = 0;


            #region Init

            //InitPort();
            //OpenPort();

            i = InitCard();
            Sleep(10);
            if (i == 5) return "5";
            else if (i == 0) return "0";
            i = ReadCard(1);
            Sleep(10);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'Y'))
            {
                ClosePort();
                return "0";
            }

            #endregion

            #region Etrans işlemleri

            outBuf[0] = 0x3A;
            outBuf[1] = 0xDF;
            outBuf[2] = 0x1D;
            outBuf[3] = 0x80;

            i = VerifyCard(7);
            if (i == 5) return "5";
            else if (i == 0) return "0";


            outBuf[0] = issue_area[0];
            outBuf[1] = issue_area[1];
            outBuf[2] = (byte)'Y';
            outBuf[3] = (byte)'E';

            i = UpdateCard(1);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            outBuf[0] = 0X0A;  // Edeğiş için 10  // Etrans için 12
            outBuf[1] = 0X00;
            outBuf[2] = 0X00;
            outBuf[3] = 0X00;

            i = UpdateCard(0x3C);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            outBuf[0] = 0X0;
            outBuf[1] = 0X0;
            outBuf[2] = 0X0;
            outBuf[3] = 0X0;

            i = UpdateCard(0x13);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            //TarihAl kapTarih = new TarihAl(kapamaTarih);
            //outBuf = kapTarih.TarihDondur4Byte(kapamaTarih);

            //i = UpdateCard(0x29);
            //if (i == 5) return "5";
            //else if (i == 0) return "0";


            Hexcon.UInt32toByte4(gerTuk, outBuf);
            i = UpdateCard(0X2F);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(kalan, outBuf);
            i = UpdateCard(0X31);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(harcanan, outBuf);
            i = UpdateCard(0X30);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(devNo, outBuf);
            i = UpdateCard(0X32);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            i = ReadCard(0X33);
            if (i == 0) return "0";

            outBuf[0] = kartNo;
            outBuf[1] = islemNo;
            outBuf[2] = inBuf[2];
            outBuf[3] = inBuf[3];

            i = UpdateCard(0X33);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            //outBuf[0] = role;
            //outBuf[1] = tsaat;
            //outBuf[2] = 0X0;
            //outBuf[3] = testreel;
            ////0X34 adres role operasyon sayısı,	tsaat degeri,	bos,	test reel ??
            //i = UpdateCard(0X34);
            //if (i == 5) return "5";
            //else if (i == 0) return "0";


            outBuf[0] = kademe;
            outBuf[1] = loadLimit;
            outBuf[2] = aksam;
            outBuf[3] = sabah;

            i = UpdateCard(0X18);
            if (i == 5) return "5";
            else if (i == 0) return "0";


            outBuf[0] = haftasonuAksam;
            outBuf[1] = 0X0;
            outBuf[2] = 0X0;
            outBuf[3] = 0X0;

            i = UpdateCard(0X1A);
            if (i == 5) return "5";
            else if (i == 0) return "0";


            //Hexcon.UInt32toByte4(donemTuketim, outBuf);
            //i = UpdateCard(0X19);
            //if (i == 5) return "5";
            //else if (i == 0) return "0";

            Hexcon.UInt32toByte4(fixCharge, outBuf);
            i = UpdateCard(0X1B);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(totalFixCharge, outBuf);
            i = UpdateCard(0X1C);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(k1Tuk, outBuf);
            i = UpdateCard(0X37);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(k2Tuk, outBuf);
            i = UpdateCard(0X3D);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(k3Tuk, outBuf);
            i = UpdateCard(0X3E);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(Lim1, outBuf);
            i = UpdateCard(0X12);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(Lim2, outBuf);
            i = UpdateCard(0X14);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(kad1, outBuf);
            i = UpdateCard(0X15);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(kad2, outBuf);
            i = UpdateCard(0X16);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(kad3, outBuf);
            i = UpdateCard(0X17);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(kritikKredi, outBuf);
            i = UpdateCard(0X2D);
            if (i == 5) return "5";
            else if (i == 0) return "0";


            #endregion

            ClosePort();
            return "1";
        }
        public string ETrans(UInt32 devNo, DateTime kapamaTarih, UInt32 gerTuk, UInt32 kalan, UInt32 harcanan, byte kartNo, byte islemNo,
                             byte kademe, byte loadLimit, byte aksam, byte sabah, byte haftasonuAksam, UInt32 donemTuketim,
                             UInt32 fixCharge, UInt32 totalFixCharge, UInt32 k1Tuk, UInt32 k2Tuk, UInt32 k3Tuk, UInt32 Lim1, UInt32 Lim2,
                             UInt32 kad1, UInt32 kad2, UInt32 kad3)
        {
            byte[] issue_area = GetIssuer(zone);
            int i = 0;


            #region Init

            //InitPort();
            //OpenPort();

            i = InitCard();
            Sleep(10);
            if (i == 5) return "5";
            else if (i == 0) return "0";
            i = ReadCard(1);
            Sleep(10);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'Y'))
            {
                ClosePort();
                return "0";
            }

            #endregion

            #region Etrans işlemleri

            outBuf[0] = 0x3A;
            outBuf[1] = 0xDF;
            outBuf[2] = 0x1D;
            outBuf[3] = 0x80;

            i = VerifyCard(7);
            if (i == 5) return "5";
            else if (i == 0) return "0";


            outBuf[0] = issue_area[0];
            outBuf[1] = issue_area[1];
            outBuf[2] = (byte)'Y';
            outBuf[3] = (byte)'E';

            i = UpdateCard(1);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            outBuf[0] = 0X0C;
            outBuf[1] = 0X0;
            outBuf[2] = 0X0;
            outBuf[3] = 0X0;

            i = UpdateCard(0x3C);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            outBuf[0] = 0X0;
            outBuf[1] = 0X0;
            outBuf[2] = 0X0;
            outBuf[3] = 0X0;

            i = UpdateCard(0x13);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            TarihAl kapTarih = new TarihAl(kapamaTarih);
            outBuf = kapTarih.TarihDondur4Byte(kapamaTarih);

            i = UpdateCard(0x29);
            if (i == 5) return "5";
            else if (i == 0) return "0";


            Hexcon.UInt32toByte4(gerTuk, outBuf);
            i = UpdateCard(0X2F);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(kalan, outBuf);
            i = UpdateCard(0X31);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(harcanan, outBuf);
            i = UpdateCard(0X30);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(devNo, outBuf);
            i = UpdateCard(0X32);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            outBuf[0] = kartNo;
            outBuf[1] = islemNo;

            i = UpdateCard(0X33);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            //outBuf[0] = role;
            //outBuf[1] = tsaat;
            //outBuf[2] = 0X0;
            //outBuf[3] = testreel;
            ////0X34 adres role operasyon sayısı,	tsaat degeri,	bos,	test reel ??
            //i = UpdateCard(0X34);
            //if (i == 5) return "5";
            //else if (i == 0) return "0";


            outBuf[0] = kademe;
            outBuf[1] = loadLimit;
            outBuf[2] = aksam;
            outBuf[3] = sabah;

            i = UpdateCard(0X18);
            if (i == 5) return "5";
            else if (i == 0) return "0";


            outBuf[0] = haftasonuAksam;
            outBuf[1] = 0X0;
            outBuf[2] = 0X0;
            outBuf[3] = 0X0;

            i = UpdateCard(0X1A);
            if (i == 5) return "5";
            else if (i == 0) return "0";


            //Hexcon.UInt32toByte4(donemTuketim, outBuf);
            //i = UpdateCard(0X19);
            //if (i == 5) return "5";
            //else if (i == 0) return "0";

            Hexcon.UInt32toByte4(fixCharge, outBuf);
            i = UpdateCard(0X1B);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(totalFixCharge, outBuf);
            i = UpdateCard(0X1C);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(k1Tuk, outBuf);
            i = UpdateCard(0X37);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(k2Tuk, outBuf);
            i = UpdateCard(0X3D);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(k3Tuk, outBuf);
            i = UpdateCard(0X3E);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(Lim1, outBuf);
            i = UpdateCard(0X12);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(Lim2, outBuf);
            i = UpdateCard(0X14);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(kad1, outBuf);
            i = UpdateCard(0X15);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(kad2, outBuf);
            i = UpdateCard(0X16);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(kad3, outBuf);
            i = UpdateCard(0X17);
            if (i == 5) return "5";
            else if (i == 0) return "0";


            #endregion

            ClosePort();
            return "1";
        }

        public string YetkiAvans(UInt32 devNo, UInt32 _Avans_Limiti)
        {

            byte[] issue_area = GetIssuer(zone);
            int i = 0;


            #region Init

            //InitPort();
            //OpenPort();

            i = InitCard();
            Sleep(10);
            if (i == 5) return "5";
            else if (i == 0) return "0";
            i = ReadCard(1);
            Sleep(10);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'Y'))
            {
                ClosePort();
                return "0";
            }

            #endregion

            #region Açma işlemleri

            outBuf[0] = 0x3A;
            outBuf[1] = 0xDF;
            outBuf[2] = 0x1D;
            outBuf[3] = 0x80;

            i = VerifyCard(7);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            outBuf[0] = issue_area[0];
            outBuf[1] = issue_area[1];
            outBuf[2] = (byte)'Y';
            outBuf[3] = (byte)'V';

            i = UpdateCard(1);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(devNo, outBuf);
            i = UpdateCard(0X3C);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(_Avans_Limiti, outBuf);
            i = UpdateCard(0x10);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            #endregion

            ClosePort();
            return "1";


        }

        public string YetkiBosalt()
        {
            byte[] issue_area = GetIssuer(zone);
            int i = 0;


            #region Init

            //InitPort();
            //OpenPort();

            i = InitCard();
            Sleep(10);
            if (i == 5) return "5";
            else if (i == 0) return "0";
            
            i = ReadCard(1);
            Sleep(10);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            if ((inBuf[0] != issue_area[0]) || (inBuf[1] != issue_area[1]) || (inBuf[2] != 'Y'))
            {
                ClosePort();
                return "0";
            }

            #endregion

            #region Formyet işlemleri
            outBuf[0] = 0x3A;
            outBuf[1] = 0xDF;
            outBuf[2] = 0x1D;
            outBuf[3] = 0x80;
            
            i = VerifyCard(7);
            if (i == 5) return "5";
            else if (i == 0) return "0";


            i = ReadCard(1);
            //Sleep(10);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            if ((inBuf[2] != 'Y'))
            {
                ClosePort();
                return "0";
            }

            #endregion


           #region Bosaltma işlemleri

            outBuf[0] = 0x00;
            outBuf[1] = 0x00;
            outBuf[2] = 0x00;
            outBuf[3] = 0x00;

            for (int j = 0x10; j < 0x20; j++)
            {
                i = UpdateCard((byte)j);
                if (i == 5) return "5";
                else if (i == 0) return "0";
            }

            for (int j = 0x28; j < 0x40; j++)
            {
                if (j == 0x38) j = 0x3C;
                i = UpdateCard((byte)j);
                if (i == 5) return "5";
                else if (i == 0) return "0";
            }

            i = UpdateCard(0X01);
            if (i == 5) return "5";
            else if (i == 0) return "0";


            outBuf[0] = 0xAA;
            outBuf[1] = 0xAA;
            outBuf[2] = 0xAA;
            outBuf[3] = 0xAA;

            i = UpdateCard(6);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            #endregion

            ClosePort();
            return "1";
        }

       
        #endregion

        #region Üretim Fonksyonları

        public string FormUret()
        {
            Sleep(10);

            int i = 0;


            #region FormUret işlemleri

            //InitPort();
            //OpenPort();
            
            i = InitCard();
            if (i == 5) return "5";
            else if (i == 0) return "0";

            if ((inBuf[0] != 0 ) || (inBuf[1] != 0 ) || (inBuf[2] != 0 ))
            {
                return "0";
            }

            outBuf[0] = 0xAA;
            outBuf[1] = 0xAA;
            outBuf[2] = 0xAA;
            outBuf[3] = 0xAA;

            i = VerifyCard(7);
            if (i == 5) return "5";
            else if (i == 0) return "0";


            outBuf[0] = (byte)'K';
            outBuf[1] = (byte)'I';
            outBuf[2] = (byte)'U';
            outBuf[3] = (byte)'A';

            i = UpdateCard(1);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            outBuf[0] = 0x7B;
            outBuf[1] = 0x8A;
            outBuf[2] = 0x13;
            outBuf[3] = 0xEC;


            i = UpdateCard(6);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            i = FinishCard();
            if (i == 5) return "5";
            else if (i == 0) return "0";

            #endregion


            ClosePort();
            return "1";
        }

        public string UretimBosalt()
        {
            int i = 0;
            
            #region Init

            //InitPort();
            //OpenPort();

            i = InitCard();
            Sleep(10);
            if (i == 5) return "5";
            else if (i == 0) return "0";
            i = ReadCard(1);
            Sleep(10);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            if ((inBuf[0] != 'K') || (inBuf[1] != 'I') || (inBuf[2] != 'U'))
            {
                ClosePort();
                return "0";
            }

            #endregion

            #region Formyet işlemleri
            outBuf[0] = 0x7B;
            outBuf[1] = 0x8A;
            outBuf[2] = 0x13;
            outBuf[3] = 0xEC;

            i = VerifyCard(7);
            if (i == 5) return "5";
            else if (i == 0) return "0";


            i = ReadCard(1);
            //Sleep(10);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            if ((inBuf[2] != 'U'))
            {
                ClosePort();
                return "0";
            }

            #endregion


            #region Bosaltma işlemleri

            i = ReadCard(4);
            if (i == 0) { return "0"; }
            outBuf[0] = 0;
            outBuf[1] = 0;
            outBuf[2] = 0;
            outBuf[3] = inBuf[3];
            i = UpdateCard(0x04);
            if (i == 0) { return "0"; }

            outBuf[0] = 0;
            outBuf[1] = 0;
            outBuf[2] = 0;
            outBuf[3] = 0;

            for (int j = 0x02; j <= 0x03; j++)      //for (int j = 0x01; j <= 0x03; j++)        // Issuer bosaltmayalım sorun oluyor.
            {
                i = UpdateCard((byte)j);
                if (i == 0) { return "0"; }
            }

            i = UpdateCard(0x05);
            if (i == 0) { return "0"; }


            outBuf[0] = 0x00;
            outBuf[1] = 0x00;
            outBuf[2] = 0x00;
            outBuf[3] = 0x00;

            for (int j = 0x10; j < 0x20; j++)
            {
                i = UpdateCard((byte)j);
                if (i == 5) return "5";
                else if (i == 0) return "0";
            }

            for (int j = 0x28; j < 0x40; j++)
            {
                if (j == 0x38) j = 0x3C;
                i = UpdateCard((byte)j);
                if (i == 5) return "5";
                else if (i == 0) return "0";
            }

            i = UpdateCard(0X01);
            if (i == 5) return "5";
            else if (i == 0) return "0";


            outBuf[0] = 0xAA;
            outBuf[1] = 0xAA;
            outBuf[2] = 0xAA;
            outBuf[3] = 0xAA;

            i = UpdateCard(6);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            #endregion

            ClosePort();
            return "1";
        }

        public string Format(UInt32 devNo, UInt32 KritikKredi, UInt32 Kat1, UInt32 Kat2, UInt32 Kat3, UInt32 Lim1, UInt32 Lim2, UInt32 OverLim)
        {
            Sleep(10);

            int i = 0;

            #region Init
          
            i = InitCard();
            if (i == 5) return "5";
            else if (i == 0) return "0";
            
            i = ReadCard(1);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            if ((inBuf[0] != 'K') || (inBuf[1] != 'I') || (inBuf[2] != 'U'))
            {
                ClosePort();
                return "0";
            }

            #endregion

            #region Format işlemleri

            outBuf[0] = 0x7B;
            outBuf[1] = 0x8A;
            outBuf[2] = 0x13;
            outBuf[3] = 0xEC;

            i = VerifyCard(7);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            outBuf[0] = (byte)'K';
            outBuf[1] = (byte)'I';
            outBuf[2] = (byte)'U';
            outBuf[3] = (byte)'F';

            i = UpdateCard(1);
            if (i == 5) return "5";
            else if (i == 0) return "0";


            Hexcon.UInt32toByte4(devNo, outBuf);
            i = UpdateCard(0X10);
            if (i == 5) return "5";
            else if (i == 0) return "0";


            DateTime tar = DateTime.Now;

            TarihAl tarih = new TarihAl(tar);
            outBuf = tarih.TarihDondur4Byte(tar);

            i = UpdateCard(0x12);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            DayOfWeek dow = tar.DayOfWeek;

            outBuf[0] = (byte)dow;
            outBuf[1] = 0;
            outBuf[2] = (byte)dow;
            outBuf[3] = 0;

            i = UpdateCard(0x11);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Integer4Byte krit = new Integer4Byte(KritikKredi);

            outBuf[0] = krit.bir;
            outBuf[1] = krit.iki;
            outBuf[2] = 0;
            outBuf[3] = 0;

            i = UpdateCard(0x13);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(Kat1, outBuf);
            i = UpdateCard(0X16);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(Kat2, outBuf);
            i = UpdateCard(0X17);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(Kat3, outBuf);
            i = UpdateCard(0X18);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(Lim1, outBuf);
            i = UpdateCard(0X19);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(Lim2, outBuf);
            i = UpdateCard(0X1A);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            outBuf[0] = 0X0;
            outBuf[1] = 0X0;
            outBuf[2] = 0X0;
            outBuf[3] = 0X0;

            outBuf[0] = Convert.ToByte(OverLim);

            i = UpdateCard(0X1B);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            i = FinishCard();
            if (i == 5) return "5";
            else if (i == 0) return "0";

            #endregion


            ClosePort();
            return "1";
        }

        public string ReelMod()
        {
            Sleep(10);

            int i = 0;

            #region Init

            //InitPort();
            //OpenPort();
            
            i = InitCard();
            if (i == 5) return "5";
            else if (i == 0) return "0";
            
            i = ReadCard(1);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            if ((inBuf[0] != 'K') || (inBuf[1] != 'I') || (inBuf[2] != 'U'))
            {
                ClosePort();
                return "0";
            }

            #endregion

            #region Reel Kart İşlemleri

            outBuf[0] = 0x7B;
            outBuf[1] = 0x8A;
            outBuf[2] = 0x13;
            outBuf[3] = 0xEC;

            i = VerifyCard(7);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            outBuf[0] = (byte)'K';
            outBuf[1] = (byte)'I';
            outBuf[2] = (byte)'U';
            outBuf[3] = (byte)'R';

            i = UpdateCard(1);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            i = FinishCard();
            if (i == 5) return "5";
            else if (i == 0) return "0";

            #endregion

            ClosePort();
            return "1";
        }

        public string TestMod(UInt32 devNo)
        {
            Sleep(10);

            int i = 0;

            #region Init

            //InitPort();
            //OpenPort();

            i = InitCard();
            if (i == 5) return "5";
            else if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            if ((inBuf[0] != 'K') || (inBuf[1] != 'I') || (inBuf[2] != 'U'))
            {
                ClosePort();
                return "0";
            }

            #endregion

            #region Test Mod Kart İşlemleri

            outBuf[0] = 0x7B;
            outBuf[1] = 0x8A;
            outBuf[2] = 0x13;
            outBuf[3] = 0xEC;

            i = VerifyCard(7);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            outBuf[0] = (byte)'K';
            outBuf[1] = (byte)'I';
            outBuf[2] = (byte)'U';
            outBuf[3] = (byte)'T';

            i = UpdateCard(1);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(devNo, outBuf);
            i = UpdateCard(0X3C);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            outBuf[0] = 0xFF;
            outBuf[1] = 0xFF;
            outBuf[2] = 0xFF;
            outBuf[3] = 0xFF;

            i = UpdateCard(0X3D);
            if (i == 5) return "5";
            else if (i == 0) return "0";


            i = FinishCard();
            if (i == 5) return "5";
            else if (i == 0) return "0";

            #endregion

            ClosePort();
            return "1";
        }

        public string CihazNo(UInt32 devNo, UInt32 KritikKredi, UInt16 CokUcuz, UInt16 Ucuz, UInt16 Normal, UInt16 Pahali)
        {
            Sleep(10);

            int i = 0;


            #region Init

            //InitPort();
            //OpenPort();
            
            i = InitCard();
            if (i == 5) return "5";
            else if (i == 0) return "0";
           
            i = ReadCard(1);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            if ((inBuf[0] != 'K') || (inBuf[1] != 'I') || (inBuf[2] != 'U'))
            {
                ClosePort();
                return "0";
            }

            #endregion

            #region Format işlemleri

            outBuf[0] = 0x7B;
            outBuf[1] = 0x8A;
            outBuf[2] = 0x13;
            outBuf[3] = 0xEC;

            i = VerifyCard(7);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            outBuf[0] = (byte)'K';
            outBuf[1] = (byte)'I';
            outBuf[2] = (byte)'U';
            outBuf[3] = (byte)'C';

            i = UpdateCard(1);
            if (i == 5) return "5";
            else if (i == 0) return "0";


            Hexcon.UInt32toByte4(devNo, outBuf);
            i = UpdateCard(0X10);
            if (i == 5) return "5";
            else if (i == 0) return "0";


            DateTime tar = DateTime.Now;

            TarihAl tarih = new TarihAl(tar);
            outBuf = tarih.TarihDondur4Byte(tar);

            i = UpdateCard(0x12);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            DayOfWeek dow = tar.DayOfWeek;

            outBuf[0] = (byte)dow;
            outBuf[1] = 0;
            outBuf[2] = (byte)dow;
            outBuf[3] = 0;

            i = UpdateCard(0x11);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Integer4Byte krit = new Integer4Byte(KritikKredi);

            outBuf[0] = krit.bir;
            outBuf[1] = krit.iki;
            outBuf[2] = 0;
            outBuf[3] = 0;

            i = UpdateCard(0x13);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Integer2Byte cUcuz = new Integer2Byte(CokUcuz);
            Integer2Byte ucuzz = new Integer2Byte(Ucuz);
            Integer2Byte normall = new Integer2Byte(Normal);
            Integer2Byte pahalii = new Integer2Byte(Pahali);

            outBuf[0] = cUcuz.bir;
            outBuf[1] = cUcuz.iki;
            outBuf[2] = ucuzz.bir;
            outBuf[3] = ucuzz.iki;

            i = UpdateCard(0x14);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            outBuf[0] = normall.bir;
            outBuf[1] = normall.iki;
            outBuf[2] = pahalii.bir;
            outBuf[3] = pahalii.iki;

            i = UpdateCard(0x15);
            if (i == 5) return "5";
            else if (i == 0) return "0";


            i = FinishCard();
            if (i == 5) return "5";
            else if (i == 0) return "0";

            #endregion


            ClosePort();
            return "1";

        }

        public string UretimAc(Int32 Acma)
        {

            Sleep(10);

            int i = 0;


            #region Init

            //InitPort();
            //OpenPort();
           
            i = InitCard();
            if (i == 5) return "5";
            else if (i == 0) return "0";
            
            i = ReadCard(1);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            if ((inBuf[0] != 'K') || (inBuf[1] != 'I') || (inBuf[2] != 'U'))
            {
                ClosePort();
                return "0";
            }

            #endregion

            #region Format işlemleri

            outBuf[0] = 0x7B;
            outBuf[1] = 0x8A;
            outBuf[2] = 0x13;
            outBuf[3] = 0xEC;

            i = VerifyCard(7);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            outBuf[0] = (byte)'K';
            outBuf[1] = (byte)'I';
            outBuf[2] = (byte)'U';
            outBuf[3] = (byte)'A';

            i = UpdateCard(1);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            if (Acma==2)
                outBuf[0] = 0X66;
            else
                outBuf[0] = 0X00;

            outBuf[1] = 0X00;
            outBuf[2] = 0X00;
            outBuf[3] = 0X00;

            i = UpdateCard(0X3C);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            i = FinishCard();
            if (i == 5) return "5";
            else if (i == 0) return "0";
            #endregion


            ClosePort();
            return "1";
        }

        public string FormIssuer(UInt32 devNo)
        {
            Sleep(10);

            int i = 0;


            #region Init

            //InitPort();
            //OpenPort();

            i = InitCard();
            if (i == 5) return "5";
            else if (i == 0) return "0";

            i = ReadCard(1);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            if ((inBuf[0] != 0 ) || (inBuf[1] != 0 ) || (inBuf[2] != 0 ))
            {
                ClosePort();
                return "0";
            }

            outBuf[0] = 0xAA;
            outBuf[1] = 0xAA;
            outBuf[2] = 0xAA;
            outBuf[3] = 0xAA;

            i = VerifyCard(7);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            outBuf[0] = (byte)'K';
            outBuf[1] = (byte)'K';
            outBuf[2] = (byte)'X';

            i = UpdateCard(1);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            outBuf[0] = 0x3A;
            outBuf[1] = 0xDF;
            outBuf[2] = 0x1D;
            outBuf[3] = 0x80;

            i = UpdateCard(6);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            Hexcon.UInt32toByte4(devNo, outBuf);
            i = UpdateCard(0X10);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            i = UpdateCard(2);
            if (i == 5) return "5";
            else if (i == 0) return "0";


            i = FinishCard();
            if (i == 5) return "5";
            else if (i == 0) return "0";


            #endregion

            ClosePort();
            return "1";
        }

        public string Issuer()
        {
            Sleep(10);

            int i = 0;

            byte[] issue_area = GetIssuer(zone);


            #region Init

            //InitPort();
            //OpenPort();

            i = InitCard();
            if (i == 5) return "5";
            else if (i == 0) return "0";

            #endregion

            i = ReadCard(1);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            if ((inBuf[0] != 'K') || (inBuf[1] != 'K') || (inBuf[2] != 'X'))
            {
                ClosePort();
                return "0";
            }

            outBuf[0] = 0x3A;
            outBuf[1] = 0xDF;
            outBuf[2] = 0x1D;
            outBuf[3] = 0x80;

            i = VerifyCard(7);
            if (i == 5) return "5";
            else if (i == 0) return "0";
            
            //int Issuer1,Issuer2;

            //int.TryParse(Issuer.Substring(0, 1), out Issuer1);
            //int.TryParse(Issuer.Substring(1, 1), out Issuer2);


            outBuf[0] = issue_area[0];
            outBuf[1] = issue_area[1];
            

            i = UpdateCard(0X3C);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            outBuf[0] = 0xFF;
            outBuf[1] = 0xFF;
            outBuf[2] = 0xFF;
            outBuf[3] = 0xFF;

            i = UpdateCard(0X3d);
            if (i == 5) return "5";
            else if (i == 0) return "0";

            outBuf[0] = 0x0;
            outBuf[1] = 0x0;
            outBuf[2] = 0x0;
            outBuf[3] = 0x0;

            i = FinishCard();
            if (i == 5) return "5";
            else if (i == 0) return "0";            

            ClosePort();
            return "1";
        }

   

        #endregion

    }

        #endregion
}
