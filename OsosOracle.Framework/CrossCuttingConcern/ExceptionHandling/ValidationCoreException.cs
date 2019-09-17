using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling
{
    public class ValidationCoreException : NotificationException
    {
        public ValidationCoreException(string mesaj)
            : base(mesaj)
        {

        }

        public ValidationCoreException(Exception hata, string mesaj)
            : base(mesaj, hata)
        {

        }
    }
}
