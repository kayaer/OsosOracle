using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Runtime.InteropServices;

namespace SCLibWin
{
    internal sealed class SCReader : IDisposable
    {
        private SCResMgr m_aResourceManager;
        private Version m_aVendorIfdVersion = null;
        private uint m_dwActiveProtocol = 0;
        private uint m_dwChannelID = 0;
        private uint m_dwMechanicalCharacteristics = 0;
        private bool m_fDisposed = false;
        private bool m_fInTransaction = false;
        private bool m_fIsConnected = false;
        private IntPtr m_hCard = IntPtr.Zero;
        private string m_sVendorIfdSerialNo = null;
        private string m_sVendorIfdType = null;
        private string m_sVendorName = null;
        public string m_ATR = null;

        public SCReader(SCResMgr aResourceManager)
        {
            if (aResourceManager == null)
            {
                throw new ArgumentNullException();
            }
            this.m_aResourceManager = aResourceManager;
        }

        private void _Dispose(bool fDisposing)
        {
            if (!this.m_fDisposed)
            {
                if (this.m_hCard != IntPtr.Zero)
                {
                    if (this.m_fInTransaction)
                    {
                        WinSCard.SCardEndTransaction(this.m_hCard, 0);
                        this.m_fInTransaction = false;
                    }
                    if (this.m_fIsConnected)
                    {
                        WinSCard.SCardDisconnect(this.m_hCard, 3);
                        this.m_fIsConnected = false;
                    }
                    this.m_hCard = IntPtr.Zero;
                }
                this.m_aResourceManager = null;
                this.m_dwActiveProtocol = 0;
                this.m_sVendorName = null;
                this.m_sVendorIfdType = null;
                this.m_aVendorIfdVersion = null;
                this.m_sVendorIfdSerialNo = null;
                this.m_dwChannelID = 0;
                this.m_dwMechanicalCharacteristics = 0;
                this.m_fDisposed = true;
            }
        }

        public void BeginTransaction()
        {
            if (this.m_fDisposed)
            {
                throw new ObjectDisposedException(base.GetType().FullName);
            }
            if ((this.m_hCard == IntPtr.Zero) || !this.m_fIsConnected)
            {
                throw new InvalidOperationException();
            }
            uint rc = WinSCard.SCardBeginTransaction(this.m_hCard);
            if (rc != 0)
            {
                SCardException.RaiseWin32ResponseCode(rc);
            }
            this.m_fInTransaction = true;
        }

        public bool Connect(string sReader, SCardAccessMode nAccessMode, SCardProtocolIdentifiers nPreferredProtocols)
        {
            if (this.m_fDisposed)
            {
                throw new ObjectDisposedException(base.GetType().FullName);
            }
            if (((this.m_hCard != IntPtr.Zero) || (this.m_aResourceManager.Context == IntPtr.Zero)) || this.m_fIsConnected)
            {
                throw new InvalidOperationException();
            }
            uint dwShareMode = (uint)nAccessMode;
            uint dwPreferredProtocols = (uint)nPreferredProtocols;
            uint rc = WinSCard.SCardConnect(this.m_aResourceManager.Context, sReader, dwShareMode, dwPreferredProtocols, out this.m_hCard, out this.m_dwActiveProtocol);
            switch (rc)
            {
                case 0:
                    this.m_fIsConnected = true;
                    return true;

                case 0x80100069:
                    return false;
            }
            SCardException.RaiseWin32ResponseCode(rc);
            throw new InvalidProgramException();
        }

        public void Control(int nControlCode, byte[] vbInBuffer, out byte[] vbOutBuffer)
        {
            if (this.m_fDisposed)
            {
                throw new ObjectDisposedException(base.GetType().FullName);
            }
            if ((this.m_hCard == IntPtr.Zero) || !this.m_fIsConnected)
            {
                throw new InvalidOperationException();
            }
            byte[] lpOutBuffer = new byte[0x10002];
            int length = lpOutBuffer.Length;
            uint lpBytesReturned = 0;
            uint rc = WinSCard.SCardControl(this.m_hCard, (uint)nControlCode, vbInBuffer, (uint)vbInBuffer.Length, lpOutBuffer, (uint)lpOutBuffer.Length, out lpBytesReturned);
            if (rc == 0)
            {
                if (lpBytesReturned > 0)
                {
                    vbOutBuffer = new byte[lpBytesReturned];
                    Buffer.BlockCopy(lpOutBuffer, 0, vbOutBuffer, 0, (int)lpBytesReturned);
                }
                else
                {
                    vbOutBuffer = null;
                }
            }
            else
            {
                SCardException.RaiseWin32ResponseCode(rc);
                throw new InvalidProgramException();
            }
        }

        public void Disconnect(SCardDisposition nDisposition)
        {
            if (this.m_fDisposed)
            {
                throw new ObjectDisposedException(base.GetType().FullName);
            }
            if ((this.m_hCard == IntPtr.Zero) || !this.m_fIsConnected)
            {
                throw new InvalidOperationException();
            }
            uint rc = WinSCard.SCardDisconnect(this.m_hCard, (uint)nDisposition);
            this.m_hCard = IntPtr.Zero;
            this.m_dwActiveProtocol = 0;
            this.m_fIsConnected = false;
            this.m_fInTransaction = false;
            if (rc != 0)
            {
                SCardException.RaiseWin32ResponseCode(rc);
            }
        }

        public void Dispose()
        {
            this._Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void EndTransaction(SCardDisposition nDisposition)
        {
            if (this.m_fDisposed)
            {
                throw new ObjectDisposedException(base.GetType().FullName);
            }
            if ((this.m_hCard == IntPtr.Zero) || !this.m_fIsConnected)
            {
                throw new InvalidOperationException();
            }
            if (!this.m_fInTransaction)
            {
                throw new InvalidOperationException();
            }
            uint rc = WinSCard.SCardEndTransaction(this.m_hCard, (uint)nDisposition);
            if (rc != 0)
            {
                SCardException.RaiseWin32ResponseCode(rc);
            }
            this.m_fInTransaction = false;
        }

        ~SCReader()
        {
            this._Dispose(false);
        }

        public bool GetReaderCapabilities(SCardReaderCapability nTag, out byte[] vbValue)
        {
            if (this.m_fDisposed)
            {
                throw new ObjectDisposedException(base.GetType().FullName);
            }
            if ((this.m_hCard == IntPtr.Zero) || !this.m_fIsConnected)
            {
                throw new InvalidOperationException();
            }
            IntPtr zero = IntPtr.Zero;
            uint maxValue = uint.MaxValue;
            uint rc = WinSCard.SCardGetAttrib(this.m_hCard, (uint)nTag, ref zero, ref maxValue);
            switch (rc)
            {
                case 0:
                    vbValue = new byte[maxValue];
                    Marshal.Copy(zero, vbValue, 0, (int)maxValue);
                    rc = WinSCard.SCardFreeMemory(this.m_aResourceManager.Context, zero);
                    return true;

                case 50:
                case 0x16:
                    vbValue = null;
                    return false;
            }
            SCardException.RaiseWin32ResponseCode(rc);
            throw new InvalidProgramException();
        }

        public int GetReaderCapabilitiesByte(SCardReaderCapability nTag)
        {
            byte[] buffer;
            if (!this.GetReaderCapabilities(nTag, out buffer))
            {
                return -1;
            }
            if (buffer.Length < 1)
            {
                throw new SCardException((SCardResponseCode)0);
            }
            return buffer[0];
        }

        public int GetReaderCapabilitiesInteger(SCardReaderCapability nTag, int nDefaultValue)
        {
            byte[] buffer;
            if (!this.GetReaderCapabilities(nTag, out buffer))
            {
                return nDefaultValue;
            }
            if (buffer.Length < 4)
            {
                throw new SCardException((SCardResponseCode)0);
            }
            return BitConverter.ToInt32(buffer, 0);
        }

        public string GetReaderCapabilitiesString(SCardReaderCapability nTag)
        {
            byte[] buffer;
            if (!this.GetReaderCapabilities(nTag, out buffer))
            {
                return null;
            }
            int length = buffer.Length;
            if ((length > 0) && (buffer[length - 1] == 0))
            {
                length--;
            }
            return Encoding.GetEncoding(0x4e4).GetString(buffer, 0, length);
        }

        public bool Reconnect(SCardAccessMode nAccessMode, SCardProtocolIdentifiers nPreferredProtocols, SCardDisposition nInitialization)
        {
            if (this.m_fDisposed)
            {
                throw new ObjectDisposedException(base.GetType().FullName);
            }
            if ((this.m_hCard == IntPtr.Zero) || !this.m_fIsConnected)
            {
                throw new InvalidOperationException();
            }
            if (nInitialization == SCardDisposition.EjectCard)
            {
                throw new ArgumentException();
            }
            this.m_dwActiveProtocol = 0;
            uint rc = WinSCard.SCardReconnect(this.m_hCard, (uint)nAccessMode, (uint)nPreferredProtocols, (uint)nInitialization, out this.m_dwActiveProtocol);
            switch (rc)
            {
                case 0:
                    return true;

                case 0x80100069:
                    this.m_fIsConnected = false;
                    return false;
            }
            SCardException.RaiseWin32ResponseCode(rc);
            throw new InvalidProgramException();
        }

        public static int SCardCtlCode(int nFunctionCode)
        {
            return (0x310000 | (nFunctionCode << 2));
        }

        public static string SelectReader()
        {
            SCResMgr mgr = new SCResMgr();
            mgr.EstablishContext(SCardContextScope.System);
            ArrayList aReadersList = new ArrayList();
            mgr.ListReaders(aReadersList);
            mgr.ReleaseContext();
            return null;
        }

        public bool SetReaderCapabilities(SCardReaderCapability nTag, byte[] vbValue)
        {
            if (this.m_fDisposed)
            {
                throw new ObjectDisposedException(base.GetType().FullName);
            }
            if ((this.m_hCard == IntPtr.Zero) || !this.m_fIsConnected)
            {
                throw new InvalidOperationException();
            }
            uint rc = WinSCard.SCardSetAttrib(this.m_hCard, (uint)nTag, vbValue, (uint)vbValue.Length);
            switch (rc)
            {
                case 0:
                    return true;

                case 50:
                    return false;
            }
            SCardException.RaiseWin32ResponseCode(rc);
            throw new InvalidProgramException();
        }

        public SCardReaderState Status()
        {
            uint num;
            uint num2;
            if (this.m_fDisposed)
            {
                throw new ObjectDisposedException(base.GetType().FullName);
            }
            if (this.m_hCard == IntPtr.Zero)
            {
                throw new InvalidOperationException();
            }
            StringBuilder szReaderName = new StringBuilder(0x100);
            uint capacity = (uint)szReaderName.Capacity;
            byte[] pbAtr = new byte[0x20];
            uint length = (uint)pbAtr.Length;
            uint rc = WinSCard.SCardStatus(this.m_hCard, szReaderName, ref capacity, out num, out num2, pbAtr, ref length);
            if (rc != 0)
            {
                SCardException.RaiseWin32ResponseCode(rc);
            }
            string tmpStr = "";
            for (int indx = 0; indx <= length - 1; indx++)
            {

                tmpStr = tmpStr + " " + string.Format("{0:X2}", pbAtr[indx]);

            }

            m_ATR = tmpStr;

            return (SCardReaderState)num;
        }

        public void Transmit(byte[] vbSendBuffer, out byte[] vbRecvBuffer)
        {
            if (this.m_fDisposed)
            {
                throw new ObjectDisposedException(base.GetType().FullName);
            }
            if (((this.m_hCard == IntPtr.Zero) || !this.m_fIsConnected) || (this.m_dwActiveProtocol == 0))
            {
                throw new InvalidOperationException();
            }
            WinSCard.SCARD_IO_REQUEST structure = new WinSCard.SCARD_IO_REQUEST();
            structure.dwProtocol = this.m_dwActiveProtocol;
            structure.cbPciLength = (uint)Marshal.SizeOf(structure);
            byte[] pbRecvBuffer = new byte[0x10002];
            uint length = (uint)pbRecvBuffer.Length;
            uint rc = WinSCard.SCardTransmit(this.m_hCard, structure, vbSendBuffer, (uint)vbSendBuffer.Length, null, pbRecvBuffer, ref length);
            if (rc == 0)
            {
                vbRecvBuffer = new byte[length];
                Buffer.BlockCopy(pbRecvBuffer, 0, vbRecvBuffer, 0, (int)length);
            }
            else
            {
                SCardException.RaiseWin32ResponseCode(rc);
                throw new InvalidProgramException();
            }
        }

        public SCardProtocolIdentifiers ActiveProtocol
        {
            get
            {
                return (SCardProtocolIdentifiers)this.m_dwActiveProtocol;
            }
        }

        public int ChannelNumber
        {
            get
            {
                if (this.m_dwChannelID == 0)
                {
                    this.m_dwChannelID = (uint)this.GetReaderCapabilitiesInteger(SCardReaderCapability.ChannelID, 0);
                }
                return (((int)this.m_dwChannelID) & 0xffff);
            }
        }

        public SCardChannelType ChannelType
        {
            get
            {
                if (this.m_dwChannelID == 0)
                {
                    this.m_dwChannelID = (uint)this.GetReaderCapabilitiesInteger(SCardReaderCapability.ChannelID, 0);
                }
                return (SCardChannelType)(this.m_dwChannelID >> 0x10);
            }
        }

        public bool InTransaction
        {
            get
            {
                return this.m_fInTransaction;
            }
        }

        public bool IsConnected
        {
            get
            {
                return this.m_fIsConnected;
            }
        }

        public SCardMechanicalCharacteristics MechanicalCharacteristics
        {
            get
            {
                if (this.m_dwMechanicalCharacteristics == 0)
                {
                    this.m_dwMechanicalCharacteristics = (uint)this.GetReaderCapabilitiesInteger(SCardReaderCapability.Characteristics, 0);
                }
                return (SCardMechanicalCharacteristics)this.m_dwMechanicalCharacteristics;
            }
        }

        public string VendorIFDSerialNo
        {
            get
            {
                if (this.m_sVendorIfdSerialNo == null)
                {
                    this.m_sVendorIfdSerialNo = this.GetReaderCapabilitiesString(SCardReaderCapability.VendorIFDSerialNo);
                }
                return this.m_sVendorIfdSerialNo;
            }
        }

        public string VendorIFDType
        {
            get
            {
                if (this.m_sVendorIfdType == null)
                {
                    this.m_sVendorIfdType = this.GetReaderCapabilitiesString(SCardReaderCapability.VendorIFDType);
                }
                return this.m_sVendorIfdType;
            }
        }

        public Version VendorIFDVersion
        {
            get
            {
                if (this.m_aVendorIfdVersion == null)
                {
                    int readerCapabilitiesInteger = this.GetReaderCapabilitiesInteger(SCardReaderCapability.VendorIFDVersion, -1);
                    if (readerCapabilitiesInteger > 0)
                    {
                        int major = (readerCapabilitiesInteger >> 0x18) & 0xff;
                        int minor = (readerCapabilitiesInteger >> 0x10) & 0xff;
                        int build = readerCapabilitiesInteger & 0xffff;
                        this.m_aVendorIfdVersion = new Version(major, minor, build);
                    }
                }
                return this.m_aVendorIfdVersion;
            }
        }

        public string VendorName
        {
            get
            {
                if (this.m_sVendorName == null)
                {
                    this.m_sVendorName = this.GetReaderCapabilitiesString(SCardReaderCapability.VendorName);
                }
                return this.m_sVendorName;
            }
        }
    }
}
