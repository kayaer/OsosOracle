using System;
using System.Collections.Generic;
using System.Text;

namespace SCLibWin
{
    public enum SCardReaderCapability
    {
        ATRString = 0x90303,
        ChannelID = 0x20110,
        Characteristics = 0x60150,
        CurrentBWT = 0x80209,
        CurrentCLK = 0x80202,
        CurrentCWT = 0x8020a,
        CurrentD = 0x80204,
        CurrentEBCEncoding = 0x8020b,
        CurrentF = 0x80203,
        CurrentIFSC = 0x80207,
        CurrentIFSD = 0x80208,
        CurrentIOState = 0x90302,
        CurrentN = 0x80205,
        CurrentProtocolType = 0x80201,
        CurrentW = 0x80206,
        DeviceFriendlyName = 0x7fff0003,
        DeviceInUse = 0x7fff0002,
        DeviceSystemName = 0x7fff0004,
        DeviceUnit = 0x7fff0001,
        EscAuthRequest = 0x7a005,
        EscCancel = 0x7a003,
        EscReset = 0x7a000,
        ExtendedBWT = 0x8020c,
        ICCInterfaceStatus = 0x90301,
        ICCPresence = 0x90300,
        ICCTypePerATR = 0x90304,
        MaxInput = 0x7a007,
        PerfBytesTransmitted = 0x7ffe0002,
        PerfNumTransmissions = 0x7ffe0001,
        PerfTransmissionTime = 0x7ffe0003,
        PowerMgmtSupport = 0x40131,
        ProtocolDefaultCLK = 0x30121,
        ProtocolDefaultDataRate = 0x30123,
        ProtocolMaxCLK = 0x30122,
        ProtocolMaxDataRate = 0x30124,
        ProtocolMaxIFSD = 0x30125,
        ProtocolTypes = 0x30120,
        SuppressT1IFSRequest = 0x7fff0007,
        UserAuthInputDevice = 0x50142,
        UserToCardAuthDevice = 0x50140,
        VendorIFDSerialNo = 0x10103,
        VendorIFDType = 0x10101,
        VendorIFDVersion = 0x10102,
        VendorName = 0x10100
    }
}
