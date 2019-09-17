using System;
using System.Collections.Generic;
using System.Text;

namespace SCLibWin
{
     [Flags]
    public enum SCardMechanicalCharacteristics
    {
        Captures = 4,
        Ejects = 2,
        None = 0,
        Swallows = 1
    }

}
