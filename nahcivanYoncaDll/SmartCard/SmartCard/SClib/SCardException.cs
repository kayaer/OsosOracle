using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace SCLibWin
{
    internal sealed class SCardException : ApplicationException
    {
        private SCardResponseCode m_nResponseCode;

        public SCardException(SCardResponseCode nResponseCode)
        {
            this.m_nResponseCode = nResponseCode;
        }

        internal static void RaiseWin32ResponseCode(uint rc)
        {
            if ((rc & 18446744073709486080L) == 18446744071563116544L)
            {
                SCardResponseCode nResponseCode = ((SCardResponseCode)rc) & ((SCardResponseCode)0xffff);
                throw new SCardException(nResponseCode);
            }
            if (rc > 0xffff)
            {
                Marshal.ThrowExceptionForHR((int)rc);
            }
            else
            {
                if (rc == 6)
                {
                    SCardResponseCode code2 = SCardResponseCode.InvalidHandle;
                    throw new SCardException(code2);
                }
                Marshal.ThrowExceptionForHR((int)(rc | 18446744071562526720L));
                throw new InvalidProgramException();
            }
        }

        public SCardResponseCode ResponseCode
        {
            get
            {
                return this.m_nResponseCode;
            }
        }
    }
}
