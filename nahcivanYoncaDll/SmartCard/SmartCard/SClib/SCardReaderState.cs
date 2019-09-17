using System;
using System.Collections.Generic;
using System.Text;

namespace SCLibWin
{
    public enum SCardReaderState
    {
        Unknown,
        Absent,
        Present,
        Swallowed,
        Powered,
        Negotiable,
        Specific
    }
}
