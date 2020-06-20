using System;
using System.Collections.Generic;
using System.Text;

namespace Listener.Helpers
{
    public class ConnectionEventArgs : EventArgs
    {
        public string KonsIp;
        public string KonsSeriNo;
        public string Info;

        public ConnectionEventArgs(string SeriNo, string Bilgi)
        {
            this.KonsSeriNo = SeriNo;
            this.Info = Bilgi;
        }

        public ConnectionEventArgs(string KonsantratorIp, string SeriNo, string Bilgi)
        {
            this.KonsIp = KonsantratorIp;
            this.KonsSeriNo = SeriNo;
            this.Info = Bilgi;
        }
    }
}
