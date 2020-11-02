using System;
using System.ComponentModel.DataAnnotations.Schema;
using OsosOracle.Framework.Entities;
namespace OsosOracle.Entities.Concrete
{
    public class EntSayacOkumaVeri
    {

        public int KayitNo { get; set; }
        public string SayacId { get; set; }
        public DateTime? OkumaTarih { get; set; }
        public string Ceza1 { get; set; }
        public string Ceza2 { get; set; }
        public string Ceza3 { get; set; }
        public string Ceza4 { get; set; }
        public string Magnet { get; set; }
        public string Ariza { get; set; }
        public string Vana { get; set; }
        public string PulseHata { get; set; }

        public string AnaPilZayif { get; set; }
        public string AnaPilBitti { get; set; }
        public string MotorPilZayif { get; set; }
        public string KrediAz { get; set; }
        public string KrediBitti { get; set; }
        public string RtcHata { get; set; }
        public string AsiriTuketim { get; set; }
        public string Ceza4Iptal { get; set; }
        public string ArizaA { get; set; }
        public string ArizaK { get; set; }
        public string ArizaP { get; set; }
        public string ArizaPil { get; set; }
        public string Borc { get; set; }
        public string Cap { get; set; }
        public string BaglantiSayisi { get; set; }
        public string KritikKredi { get; set; }
        public string Limit1 { get; set; }
        public string Limit2 { get; set; }
        public string Limit3 { get; set; }
        public string Limit4 { get; set; }
        public string Fiyat1 { get; set; }
        public string Fiyat2 { get; set; }
        public string Fiyat3 { get; set; }
        public string Fiyat4 { get; set; }
        public string Fiyat5 { get; set; }
        public string Iterasyon { get; set; }
        public string PilAkim { get; set; }
        public string PilVoltaj { get; set; }
        public string AboneNo { get; set; }
        public string AboneTip { get; set; }
        public string IlkPulseTarih { get; set; }
        public string SonPulseTarih { get; set; }
        public string BorcTarih { get; set; }
        public string MaxDebi { get; set; }
        public string MaxDebiSinir { get; set; }
        public string DonemGun { get; set; }
        public string Donem { get; set; }
        public string VanaAcmaTarih { get; set; }
        public string VanaKapamaTarih { get; set; }
        public string Sicaklik { get; set; }
        public string MinSicaklik { get; set; }
        public string MaxSicaklik { get; set; }
        public string YanginModu { get; set; }
        public string SonYuklenenKrediTarih { get; set; }
        //Tüketim Verileri
        public DateTime? SayacTarih { get; set; }
        public string HaftaninGunu { get; set; }
        public decimal Tuketim { get; set; } 
        public decimal Tuketim1 { get; set; }
        public decimal Tuketim2 { get; set; }
        public decimal Tuketim3 { get; set; }
        public decimal Tuketim4 { get; set; }
        public decimal HarcananKredi { get; set; }
        public decimal KalanKredi { get; set; }

        public string KonsSeriNo { get; set; }
        public string Ip { get; set; }
        public string Rssi { get; set; }
    }
}
