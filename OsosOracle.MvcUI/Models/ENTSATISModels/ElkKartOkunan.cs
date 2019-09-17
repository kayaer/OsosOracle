using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OsosOracle.MvcUI.Models.ENTSATISModels
{
    public class ElkKartOkunan
    {

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