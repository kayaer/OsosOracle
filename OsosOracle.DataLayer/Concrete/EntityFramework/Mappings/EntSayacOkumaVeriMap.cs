using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Net.Security;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Mappings
{
    public class EntSayacOkumaVeriMap : EntityTypeConfiguration<EntSayacOkumaVeriEf>
    {
        public EntSayacOkumaVeriMap()
        {

            // Table & Column Mappings
            ToTable("ENTSAYACOKUMAVERI");
            // Primary Key
            HasKey(t => t.KayitNo);
            // Properties
            Property(t => t.KayitNo).HasColumnName("KAYITNO").IsRequired();
            Property(t => t.SayacId).HasColumnName("SAYACID");
            Property(t => t.OkumaTarih).HasColumnName("OKUMATARIH");
            Property(t => t.Ceza1).HasColumnName("CEZA1");
            Property(t => t.Ceza2).HasColumnName("CEZA2");
            Property(t => t.Ceza3).HasColumnName("CEZA3");
            Property(t => t.Ceza4).HasColumnName("CEZA4");
            Property(t => t.Magnet).HasColumnName("MAGNET");
            Property(t => t.Ariza).HasColumnName("ARIZA");
            Property(t => t.Vana).HasColumnName("VANA");
            Property(t => t.PulseHata).HasColumnName("PULSEHATA");
            Property(t => t.AnaPilZayif).HasColumnName("ANAPILZAYIF");
            Property(t => t.AnaPilBitti).HasColumnName("ANAPILBITTI");
            Property(t => t.MotorPilZayif).HasColumnName("MOTORPILZAYIF");
            Property(t => t.KrediAz).HasColumnName("KREDIAZ");
            Property(t => t.KrediBitti).HasColumnName("KREDIBITTI");
            Property(t => t.RtcHata).HasColumnName("RTCHATA");
            Property(t => t.AsiriTuketim).HasColumnName("ASIRITUKETIM");
            Property(t => t.Ceza4Iptal).HasColumnName("CEZA4IPTAL");
            Property(t => t.ArizaK).HasColumnName("ARIZAK");
            Property(t => t.ArizaP).HasColumnName("ARIZAP");
            Property(t => t.ArizaPil).HasColumnName("ARIZAPIL");
            Property(t => t.Borc).HasColumnName("BORC");
            Property(t => t.Cap).HasColumnName("CAP");
            Property(t => t.BaglantiSayisi).HasColumnName("BAGLANTISAYISI");
            Property(t => t.KritikKredi).HasColumnName("KRITIKKREDI");
            Property(t => t.Limit1).HasColumnName("LIMIT1");
            Property(t => t.Limit2).HasColumnName("LIMIT2");
            Property(t => t.Limit3).HasColumnName("LIMIT3");
            Property(t => t.Limit4).HasColumnName("LIMIT4");
            Property(t => t.Fiyat1).HasColumnName("FIYAT1");
            Property(t => t.Fiyat2).HasColumnName("FIYAT2");
            Property(t => t.Fiyat3).HasColumnName("FIYAT3");
            Property(t => t.Fiyat4).HasColumnName("FIYAT4");
            Property(t => t.Fiyat5).HasColumnName("FIYAT5");
            Property(t => t.Iterasyon).HasColumnName("ITERASYON");
            Property(t => t.PilAkim).HasColumnName("PILAKIM");
            Property(t => t.PilVoltaj).HasColumnName("PILVOLTAJ");
            Property(t => t.AboneNo).HasColumnName("ABONENO");
            Property(t => t.AboneTip).HasColumnName("ABONETIP");
            Property(t => t.IlkPulseTarih).HasColumnName("ILKPULSETARIH");
            Property(t => t.SonPulseTarih).HasColumnName("SONPULSETARIH");
            Property(t => t.BorcTarih).HasColumnName("BORCTARIH");
            Property(t => t.MaxDebi).HasColumnName("MAXDEBI");
            Property(t => t.MaxDebiSinir).HasColumnName("MAXDEBISINIR");
            Property(t => t.DonemGun).HasColumnName("DONEMGUN");
            Property(t => t.Donem).HasColumnName("DONEM");
            Property(t => t.VanaAcmaTarih).HasColumnName("VANAACMATARIH");
            Property(t => t.VanaKapamaTarih).HasColumnName("VANAKAPAMATARIH");
            Property(t => t.Sicaklik).HasColumnName("SICAKLIK");
            Property(t => t.MinSicaklik).HasColumnName("MINSICAKLIK");
            Property(t => t.MaxSicaklik).HasColumnName("MAXSICAKLIK");
            Property(t => t.YanginModu).HasColumnName("YANGINMODU");
            Property(t => t.SonYuklenenKrediTarih).HasColumnName("SONYUKLENENKREDITARIH");
            Property(t => t.SayacTarih).HasColumnName("SAYACTARIH");
            Property(t => t.HaftaninGunu).HasColumnName("HAFTANINGUNU");
            Property(t => t.Tuketim).HasColumnName("TUKETIM");
            Property(t => t.Tuketim1).HasColumnName("TUKETIM1");
            Property(t => t.Tuketim2).HasColumnName("TUKETIM2");
            Property(t => t.Tuketim3).HasColumnName("TUKETIM3");
            Property(t => t.Tuketim4).HasColumnName("TUKETIM4");
            Property(t => t.HarcananKredi).HasColumnName("HARCANANKREDI");
            Property(t => t.KalanKredi).HasColumnName("KALANKREDI");
            Property(t => t.KonsSeriNo).HasColumnName("KONSSERINO");
            Property(t => t.Ip).HasColumnName("IP");
            Property(t => t.Rssi).HasColumnName("RSSI");
            Property(t => t.ArizaA).HasColumnName("ARIZAA");
        }
    }
}
