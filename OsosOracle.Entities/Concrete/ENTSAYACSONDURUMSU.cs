using System;
using OsosOracle.Framework.Entities;
namespace OsosOracle.Entities.Concrete
{
    public class ENTSAYACSONDURUMSU : IEntity
    {
        public int? GUNCELLEYEN { get; set; }
        public DateTime? GUNCELLEMETARIH { get; set; }
        public int VERSIYON { get; set; }
        public int? CAP { get; set; }
        public int? BAGLANTISAYISI { get; set; }
        public int? LIMIT1 { get; set; }
        public int? LIMIT2 { get; set; }
        public int? LIMIT3 { get; set; }
        public int? LIMIT4 { get; set; }
        public int? KRITIKKREDI { get; set; }
        public int? FIYAT1 { get; set; }
        public int? FIYAT2 { get; set; }
        public int? FIYAT3 { get; set; }
        public int? FIYAT4 { get; set; }
        public int? FIYAT5 { get; set; }
        public string ARIZAA { get; set; }
        public string ARIZAK { get; set; }
        public string ARIZAP { get; set; }
        public string ARIZAPIL { get; set; }
        public string BORC { get; set; }
        public string ITERASYON { get; set; }
        public string PILAKIM { get; set; }
        public string PILVOLTAJ { get; set; }
        public string SONYUKLENENKREDITARIH { get; set; }
        public string ABONENO { get; set; }
        public string ABONETIP { get; set; }
        public string ILKPULSETARIH { get; set; }
        public string SONPULSETARIH { get; set; }
        public string BORCTARIH { get; set; }
        public int? MAXDEBI { get; set; }
        public int? MAXDEBISINIR { get; set; }
        public int? DONEMGUN { get; set; }
        public int? DONEM { get; set; }
        public string VANAACMATARIH { get; set; }
        public string VANAKAPAMATARIH { get; set; }
        public int? SICAKLIK { get; set; }
        public int? MINSICAKLIK { get; set; }
        public int? MAXSICAKLIK { get; set; }
        public int? YANGINMODU { get; set; }
        public string ASIRITUKETIM { get; set; }
        public string ARIZA { get; set; }
        public DateTime? SONBILGILENDIRMEZAMANI { get; set; }
        public int KAYITNO { get; set; }
        public string PARAMETRE { get; set; }
        public string ACIKLAMA { get; set; }
        public int? DURUM { get; set; }
        public string KREDIBITTI { get; set; }
        public string KREDIAZ { get; set; }
        public string ANAPILBITTI { get; set; }
        public DateTime? SONBAGLANTIZAMAN { get; set; }
        public DateTime? SONOKUMATARIH { get; set; }
        public string PULSEHATA { get; set; }
        public string RTCHATA { get; set; }
        public string VANA { get; set; }
        public string CEZA1 { get; set; }
        public string CEZA2 { get; set; }
        public string CEZA3 { get; set; }
        public string CEZA4 { get; set; }
        public string MAGNET { get; set; }
        public string ANAPILZAYIF { get; set; }
        public int OLUSTURAN { get; set; }
        public DateTime OLUSTURMATARIH { get; set; }
    }
}
