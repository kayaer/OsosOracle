using System;
using System.Collections.Generic;
using System.Text;

namespace SCLibWin
{
    public enum SCardResponseCode
    {
        Cancelled = 2,
        InsufficientBuffer = 8,
        InvalidHandle = 3,
        InvalidParameter = 4,
        InvalidValue = 0x11,
        NoMemory = 6,
        NoService = 0x1d,
        NoSmartCard = 12,
        NotReady = 0x10,
        NotTransacted = 0x16,
        ProtoMismatch = 15,
        ReaderUnavailable = 0x17,
        RemovedCard = 0x69,
        ResetCard = 0x68,
        ServiceStopped = 30,
        SharingViolation = 11,
        SystemCancelled = 0x12,
        Timeout = 10,
        UnknownCard = 13,
        UnknownReader = 9,
        UnpoweredCard = 0x67,
        UnresponsiveCard = 0x66,
        UnsupportedCard = 0x65
    }
}
