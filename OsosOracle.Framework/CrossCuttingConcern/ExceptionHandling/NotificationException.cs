using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling
{
    public class NotificationException : Exception
    {
        public NotificationException(string mesaj, int kod = 0, string redirectUrl = "")
            : base($"{mesaj}|NotificationException|{kod}")
        {
            if (redirectUrl != "")
                base.HelpLink = redirectUrl;
        }

        public NotificationException(string mesaj, Exception innerException, int kod = 0)
            : base($"{mesaj}|NotificationException|{kod}", innerException)
        {

        }
    }
}
