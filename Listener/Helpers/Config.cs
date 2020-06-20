using System;
using System.Collections.Generic;
using System.Text;

namespace Listener.Helpers
{
    [Serializable]
    public class Config
    {
        //2322207456,126,0,115-3,119-2,121-PASIVE,117-80,
        public string ConfigStr = "";
        public string ReadoutPeriyot = "";
        public string ConnectionPeriyot = "";
        public string ClientMode = "";
        public string ListenPort = "";
        public string TuketimSinir = "";
        public string Sms = "";
        public string Apn = "";

        public Config() { }

        public Config(string cfgStr)
        {
            try
            {
                ConfigStr = cfgStr;

                string[] cfg = cfgStr.Split(',');

                foreach (string s in cfg)
                {
                    string[] cfgBilgi = s.Split('-');

                    #region params

                    switch (Convert.ToInt32(cfgBilgi[0]))
                    {
                        case CommandCode.CMD_READOUT_PERIOD:

                            ReadoutPeriyot = cfgBilgi[1] + "-";

                            if (cfgBilgi[1] == "1") ReadoutPeriyot += "15 dk";
                            else if (cfgBilgi[1] == "2") ReadoutPeriyot += "30 dk";
                            else if (cfgBilgi[1] == "3") ReadoutPeriyot += "60 dk";
                            else if (cfgBilgi[1] == "4") ReadoutPeriyot += "24 saat";
                            else ReadoutPeriyot = cfgBilgi[1] + " dk";
                            break;
                        case CommandCode.CMD_CONNECTION_PERIOD:

                            ConnectionPeriyot = cfgBilgi[1] + "-";

                            if (cfgBilgi[1] == "1") ConnectionPeriyot += "15 dk";
                            else if (cfgBilgi[1] == "2") ConnectionPeriyot += "30 dk";
                            else if (cfgBilgi[1] == "3") ConnectionPeriyot += "60 dk";
                            else if (cfgBilgi[1] == "4") ConnectionPeriyot += "24 saat";
                            else ConnectionPeriyot = cfgBilgi[1] + " dk";

                            break;

                        case CommandCode.CMD_CLIENT_MODE:
                            ClientMode = cfgBilgi[1];
                            break;

                        case CommandCode.CMD_SET_LISTEN_PORT:
                            ListenPort = cfgBilgi[1];
                            break;

                        case CommandCode.CMD_TUKETIM_SINIRI:
                            TuketimSinir = cfgBilgi[1];
                            break;

                        case CommandCode.CMD_SMS_CONTROL:
                            Sms = cfgBilgi[1];
                            break;
                        case CommandCode.CMD_SET_APN:
                            Apn = cfgBilgi[1];
                            break;

                        default:
                            break;
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                ConfigStr = cfgStr;
            }


        }

        public override string ToString()
        {
            if (ReadoutPeriyot != "")
            {
                return "ReadoutPeriyot:" + ReadoutPeriyot + "\r\n" +
                       "ConnectionPeriyot:" + ConnectionPeriyot + "\r\n" +
                       "ClientMode:" + ClientMode + "\r\n" +
                       "ListenPort:" + ListenPort + "\r\n" +
                       "TuketimSinir:" + TuketimSinir + "\r\n" +
                       "Sms:" + Sms + "\r\n" +
                       "Apn:" + Apn + "\r\n";
            }
            else return ConfigStr;

        }
    }
}
