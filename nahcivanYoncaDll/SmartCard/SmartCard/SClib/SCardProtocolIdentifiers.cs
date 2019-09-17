using System;
using System.Collections.Generic;
using System.Text;

namespace SCLibWin
{
    [Flags]
    public enum SCardProtocolIdentifiers
    {
        Default = -2147483648,
        Optimal = 0,
        Raw = 0x10000,
        T0 = 1,
        T1 = 2,
        Undefined = 0,
        CL =3
    }
}
