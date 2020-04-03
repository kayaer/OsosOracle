using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCard
{
    internal class GlobalAyarlar
    {
        private static GlobalAyarlar _instance = new GlobalAyarlar();
       // private string _Issuer = "AE";//AE ASKI ve DEFAULT
        //private string _Issuer = "TS";//TS TEKSAN
       // private string _Issuer = "EM";//EM Libya
        private string _Issuer = "P0";//Mısır

        static GlobalAyarlar()
        {
        }

        public static GlobalAyarlar GetInstance()
        {
            if (_instance == null) _instance = new GlobalAyarlar();
            return _instance;
        }
        public string Issuer
        {
            get { return _Issuer; }
            set { _Issuer = value; }
        }

    }
}
