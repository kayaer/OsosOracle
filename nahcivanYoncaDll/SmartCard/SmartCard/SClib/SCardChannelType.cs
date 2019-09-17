using System;
using System.Collections.Generic;
using System.Text;

namespace SCLibWin
{
    public enum SCardChannelType
    {
        IDE = 0x10,
        Keyboard = 4,
        Parallel = 2,
        PCMCIA = 0x40,
        SCSI = 8,
        Serial = 1,
        Unknown = 0,
        USB = 0x20,
        Vendor = 240
    }
}
