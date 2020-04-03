using SCLibWin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace SmartCard
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class ContactReader : IKartOkuyucu
    {
        Util util = new Util();

        private byte[] inBuf = new byte[4];
        private byte[] outBuf = new byte[4];

        SCResMgr mng;
        SCReader rd;
        string rdName;

        private string _ATR = "";
        SCardReaderState state;

        public ContactReader()
        {
        }

        public string ATR
        {
            get { return _ATR; }
        }

        #region IReader Members

        public byte[] InBuf
        {
            get
            {
                if (inBuf == null) return new byte[4];
                else return inBuf;
            }
            set
            {
                inBuf = value;
            }
        }

        public byte[] OutBuf
        {
            get
            {
                if (outBuf == null) return new byte[4];
                else return outBuf;
            }
            set
            {
                outBuf = value;
            }
        }

        public int InitCard()
        {
            try
            {
                mng = new SCResMgr();
                mng.EstablishContext(SCardContextScope.System);
                rd = new SCReader(mng);
                ArrayList al = new ArrayList();
                mng.ListReaders(al);


                foreach (string st in al)
                {
                    rdName = st;
                    if (!rd.IsConnected) rd.Connect(rdName, SCardAccessMode.Shared, SCardProtocolIdentifiers.T0);
                    else break;
                }

                //rdName = al[0].ToString();
                //rd.Connect(rdName, SCardAccessMode.Shared, SCardProtocolIdentifiers.T0);
                if (rd.IsConnected)
                {
                    state = rd.Status();
                    _ATR = rd.m_ATR;

                    rd.BeginTransaction();
                }

                return Convert.ToInt32(rd.IsConnected);
            }
            catch (Exception ex)
            {
                return Convert.ToInt32(rd.IsConnected);
            }
        }

        public string FinishCard()
        {

            try
            {
                if (rd.IsConnected)
                {
                    rd.EndTransaction(SCardDisposition.UnpowerCard);
                    rd.Disconnect(SCardDisposition.UnpowerCard);
                    mng.ReleaseContext();
                }

            }
            catch (Exception ex)
            {
                return "0";
            }

            return "0";

            //if (rd.IsConnected) rd.EndTransaction(SCardDisposition.UnpowerCard);
            //return "0";
        }

        public int ReadCard(UInt16 Adres, byte Adet)
        {
            int sonuc = 0;

            Integer2Byte index = new Integer2Byte(Adres);

            try
            {
                byte[] command = new byte[5];
                byte[] response = new byte[6];

                command[0] = 0X80;
                command[1] = 0XBE;
                command[2] = index.iki;
                command[3] = index.bir; //Read adres
                command[4] = 0X04;

                rd.Transmit(command, out response);
                if (response[4] == 0X90 && response[5] == 0X00)
                {
                    Array.Copy(response, 0, inBuf, 0, 4);
                    sonuc = 1;
                }
                else sonuc = 0;
            }
            catch (Exception ex)
            {
                sonuc = 0;
            }

            return sonuc;

        }

        public int VerifyCard(byte Adres)
        {
            int sonuc = 0;

            try
            {
                byte[] command = new byte[9];
                byte[] response = new byte[2];

                command[0] = 0X00;
                command[1] = 0X20;
                command[2] = 0X00;
                command[3] = Adres;  //verify  adres  = 7
                command[4] = 0X04;
                Array.Copy(outBuf, 0, command, 5, 4);

                rd.Transmit(command, out response);
                if (response[0] == 0X90 && response[1] == 0X00) sonuc = 1;
                else sonuc = 0;
            }
            catch (Exception ex)
            {
                sonuc = 0;
            }

            return sonuc;
        }

        public int UpdateCard(UInt16 Adres)
        {
            int sonuc = 0;

            Integer2Byte index = new Integer2Byte(Adres);

            try
            {
                byte[] command = new byte[9];
                byte[] response = new byte[2];

                command[0] = 0X80;
                command[1] = 0XDE;
                command[2] = index.iki;
                command[3] = index.bir;  //update adres
                command[4] = 0X04;
                Array.Copy(outBuf, 0, command, 5, 4);

                rd.Transmit(command, out response);
                if (response[0] == 0X90 && response[1] == 0X00) sonuc = 1;
                else sonuc = 0;
            }
            catch (Exception ex)
            {
                sonuc = 0;
            }

            return sonuc;
        }

        public int Eject()
        {
            return 1;
        }


        #endregion
    }
}
