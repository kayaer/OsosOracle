using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Runtime.InteropServices;

namespace SCLibWin
{
    internal sealed class SCResMgr : IDisposable
    {
        private bool m_fDisposed = false;
        private IntPtr m_hContext = IntPtr.Zero;

        private void _Dispose(bool fDisposing)
        {
            if (!this.m_fDisposed)
            {
                if (this.m_hContext != IntPtr.Zero)
                {
                    WinSCard.SCardCancel(this.m_hContext);
                    WinSCard.SCardReleaseContext(this.m_hContext);
                    this.m_hContext = IntPtr.Zero;
                }
                this.m_fDisposed = true;
            }
        }

        public void Dispose()
        {
            this._Dispose(true);
            GC.SuppressFinalize(this);
        }

        private int _SplitMultiString(string msz, IList aList)
        {
            int num;
            int num2 = 0;
            for (int i = 0; msz[i] != '\0'; i = num + 1)
            {
                num = i;
                while (msz[num] != '\0')
                {
                    num++;
                }
                aList.Add(msz.Substring(i, num - i));
                num2++;
            }
            return num2;
        }

        public bool EstablishContext(SCardContextScope nScope)
        {
            if (this.m_fDisposed)
            {
                throw new ObjectDisposedException(base.GetType().FullName);
            }
            if (this.m_hContext != IntPtr.Zero)
            {
                throw new InvalidOperationException();
            }
            uint num = WinSCard.SCardEstablishContext((uint)nScope, IntPtr.Zero, IntPtr.Zero, out this.m_hContext);
            if (num == 0x8010001d)
            {
                //MessageBox.Show("The ensure that SCardSvr Service is running. PC/SC routines won't be available.");
                return false;
            }
            if (num != 0)
            {
                //MessageBox.Show(string.Format("SCardEstablishContext failed: 0x{0:X8}", num));
                SCardException.RaiseWin32ResponseCode(num);
            }
            return true;
        }

        public int ListReaders(IList aReadersList)
        {
            return this.ListReaders((string[])null, aReadersList);
        }

        public int ListReaders(string[] vsGroups, IList aReadersList)
        {
            if (this.m_fDisposed)
            {
                throw new ObjectDisposedException(base.GetType().FullName);
            }
            if (this.m_hContext == IntPtr.Zero)
            {
                throw new InvalidOperationException();
            }
            int num = 0;
            string mszGroups = null;
            if ((vsGroups != null) && (vsGroups.Length > 0))
            {
                foreach (string str2 in vsGroups)
                {
                    mszGroups = mszGroups + str2 + '\0';
                }
                mszGroups = mszGroups + '\0';
            }
            IntPtr zero = IntPtr.Zero;
            uint maxValue = uint.MaxValue;
            uint rc = WinSCard.SCardListReaders(this.m_hContext, mszGroups, ref zero, ref maxValue);
            switch (rc)
            {
                case 0:
                    {
                        string msz = Marshal.PtrToStringUni(zero, (int)maxValue);
                        num = this._SplitMultiString(msz, aReadersList);
                        rc = WinSCard.SCardFreeMemory(this.m_hContext, zero);
                        return num;
                    }
                case 0x8010002e:
                    return 0;
            }
            SCardException.RaiseWin32ResponseCode(rc);
            return num;
        }

        public int ListReaders(string sGroup, IList aReadersList)
        {
            if ((sGroup == null) || (sGroup.Length == 0))
            {
                throw new ArgumentNullException();
            }
            string[] vsGroups = new string[] { sGroup };
            return this.ListReaders(vsGroups, aReadersList);
        }

        public void ReleaseContext()
        {
            if (this.m_fDisposed)
            {
                throw new ObjectDisposedException(base.GetType().FullName);
            }
            if (this.m_hContext == IntPtr.Zero)
            {
                throw new InvalidOperationException();
            }
            uint rc = WinSCard.SCardReleaseContext(this.m_hContext);
            this.m_hContext = IntPtr.Zero;
            if (rc != 0)
            {
                SCardException.RaiseWin32ResponseCode(rc);
            }
        }

        public IntPtr Context
        {
            get
            {
                return this.m_hContext;
            }
        }
    }
}
