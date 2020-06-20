using System;
using System.Collections.Generic;
using System.Text;

namespace Listener.Helpers
{
    [Serializable]
    public class AmiCommand
    {
        public string Id;
        public int Kod;
        public string Params;
        public int DenemeSayisi;

        public AmiCommand() { }

        public AmiCommand(string CommandId, int ServisKod, string Parametreler)
        {
            Id = CommandId;
            Kod = ServisKod;
            Params = Parametreler;
        }

        public AmiCommand(string CommandId, int KomutKodu, string Parametreler, int Deneme)
        {
            Id = CommandId;
            Kod = KomutKodu;
            Params = Parametreler;
            DenemeSayisi = Deneme;
        }

        public override string ToString()
        {
            return Id + "," + Kod + "," + Params;
        }
    }
}
