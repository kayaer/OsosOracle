using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OsosOracle.MvcUI.Models.ENTSATISModels
{
    public class ElektrikOkunan
    {
        public ElektrikOkunan()
        {

        }
        public ElektrikOkunan(string hamdata)
        {
            string[] dataArray = hamdata.Split("|".ToCharArray());

            OkumaKontrol = dataArray[0];
            SayacSeriNo = Convert.ToInt64(dataArray[1]);
            AnaKredi = Convert.ToInt64(dataArray[2]);
            AnaKrediKontrol = dataArray[3] == "*" ? true : false;
            YedekKrediKontrol = dataArray[4] == "*" ? true : false;
            KartNo = Convert.ToInt64(dataArray[5]);
            KalanKredi = Convert.ToInt64(dataArray[6]);
            TuketilenKredi = Convert.ToInt64(dataArray[7]);
            SayacTarihi = dataArray[8];
            KlemensCeza = dataArray[9];
            Ariza = dataArray[10];
            DusukPilDurumu = dataArray[11];
            BitikPilDurumu = dataArray[12];
            BirOncekiDonemTuketim = Convert.ToInt64(dataArray[13]);
            IkiOncekiDonemTuketim = Convert.ToInt64(dataArray[14]);
            UcOncekiDonemTuketim = Convert.ToInt64(dataArray[15]);
            GercekTuketim = Convert.ToInt64(dataArray[16]);
            Ekim = Convert.ToInt64(dataArray[17]);
            Aralik = Convert.ToInt64(dataArray[18]);
            KademeBir = Convert.ToInt64(dataArray[19]);
            KademeIki = Convert.ToInt64(dataArray[20]);
            KademeUc = Convert.ToInt64(dataArray[21]);
            Limit1 = Convert.ToInt64(dataArray[22]);
            Limit2 = Convert.ToInt64(dataArray[23]);
            YuklemeLimiti = Convert.ToInt64(dataArray[24]);
            AksamSaati = Convert.ToDecimal(dataArray[25]);
            SabahSaati = Convert.ToDecimal(dataArray[26]);
            Kademe = Convert.ToInt64(dataArray[27]);
            HaftaSonuAksam = (dataArray[28]);
            FixCharge = Convert.ToInt64(dataArray[29]);
            TotalFixCharge = Convert.ToInt64(dataArray[30]);
            YedekKredi = Convert.ToInt64(dataArray[31]);
            KritikKredi = Convert.ToInt64(dataArray[32]);
            Tip = 1;

            if (AnaKrediKontrol == true)
            {
                OkunduAnaBilgi = "Used";// Dil.Yuklenmis;
            }
            else
            {
                OkunduAnaBilgi = "Not Used";// Dil.Yuklenmemis;
            }

            if (YedekKrediKontrol == true)
            {
                OkunduYedekBilgi = "Used";// Dil.Yuklenmis;
            }
            else
            {
                OkunduYedekBilgi = "Not Used";// Dil.Yuklenmemis;
            }
        }

        public string Hamdata { get; set; }

        public string OkumaKontrol { get; set; }

        public long SayacSeriNo { get; set; }

        public long AboneNo { get; set; }

        public long AnaKredi { get; set; }

        public bool AnaKrediKontrol { get; set; }

        public bool YedekKrediKontrol { get; set; }

        public long KartNo { get; set; }

        public long KalanKredi { get; set; }

        public long TuketilenKredi { get; set; }

        public string SayacTarihi { get; set; }

        public string KlemensCeza { get; set; }

        public string Ariza { get; set; }

        public string DusukPilDurumu { get; set; }

        public string BitikPilDurumu { get; set; }

        public long BirOncekiDonemTuketim { get; set; }

        public long IkiOncekiDonemTuketim { get; set; }

        public long UcOncekiDonemTuketim { get; set; }

        public long GercekTuketim { get; set; }

        public long Ekim { get; set; }

        public long Aralik { get; set; }

        public decimal Fiyat1 { get; set; }

        public decimal Fiyat2 { get; set; }

        public decimal Fiyat3 { get; set; }

        public long KademeBir { get; set; }

        public long KademeIki { get; set; }

        public long KademeUc { get; set; }

        public long Limit1 { get; set; }

        public long Limit2 { get; set; }

        public long YuklemeLimiti { get; set; }

        public decimal SabahSaati { get; set; }

        public decimal AksamSaati { get; set; }

        public long Kademe { get; set; }

        public string HaftaSonuAksam { get; set; }

        public long FixCharge { get; set; }

        public long TotalFixCharge { get; set; }

        public long YedekKredi { get; set; }

        public long KritikKredi { get; set; }

        public byte Tatil1Gun { get; set; }

        public byte Tatil1Ay { get; set; }

        public byte Tatil1Sure { get; set; }

        public byte Tatil2Gun { get; set; }

        public byte Tatil2Ay { get; set; }

        public byte Tatil2Sure { get; set; }

        public int Tip { get; set; }
        //public string Tip { get; set; }

        public decimal SayacId { get; set; }

        public string TarihBaslangic1 { get; set; }

        public string TarihBitis1 { get; set; }

        public string TarihBaslangic2 { get; set; }

        public string TarihBitis2 { get; set; }


        public string OkunduAnaBilgi { get; set; }


        public string OkunduYedekBilgi { get; set; }


        public long? KayitNo { get; set; }

        public string TarifeAdi { get; set; }
    }
}