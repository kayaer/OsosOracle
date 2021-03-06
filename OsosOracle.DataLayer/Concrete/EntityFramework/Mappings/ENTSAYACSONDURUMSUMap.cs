using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Mappings
{
    public class ENTSAYACSONDURUMSUMap : EntityTypeConfiguration<ENTSAYACSONDURUMSUEf>
    {
        public ENTSAYACSONDURUMSUMap()
        {

            // Table & Column Mappings
            ToTable("ENTSAYACSONDURUMSU");
            // Primary Key
            HasKey(t => t.KAYITNO);
            // Properties
            Property(t => t.KAYITNO).HasColumnName("KAYITNO");
            Property(t => t.GUNCELLEYEN).HasColumnName("GUNCELLEYEN");
            Property(t => t.GUNCELLEMETARIH).HasColumnName("GUNCELLEMETARIH");
            Property(t => t.VERSIYON).HasColumnName("VERSIYON");
            Property(t => t.CAP).HasColumnName("CAP");
            Property(t => t.BAGLANTISAYISI).HasColumnName("BAGLANTISAYISI");
            Property(t => t.LIMIT1).HasColumnName("LIMIT1");
            Property(t => t.LIMIT2).HasColumnName("LIMIT2");
            Property(t => t.LIMIT3).HasColumnName("LIMIT3");
            Property(t => t.LIMIT4).HasColumnName("LIMIT4");
            Property(t => t.KRITIKKREDI).HasColumnName("KRITIKKREDI");
            Property(t => t.FIYAT1).HasColumnName("FIYAT1");
            Property(t => t.FIYAT2).HasColumnName("FIYAT2");
            Property(t => t.FIYAT3).HasColumnName("FIYAT3");
            Property(t => t.FIYAT4).HasColumnName("FIYAT4");
            Property(t => t.FIYAT5).HasColumnName("FIYAT5");
            Property(t => t.ARIZAA).HasColumnName("ARIZAA");
            Property(t => t.ARIZAK).HasColumnName("ARIZAK");
            Property(t => t.ARIZAP).HasColumnName("ARIZAP");
            Property(t => t.ARIZAPIL).HasColumnName("ARIZAPIL");
            Property(t => t.BORC).HasColumnName("BORC");
            Property(t => t.ITERASYON).HasColumnName("ITERASYON");
            Property(t => t.PILAKIM).HasColumnName("PILAKIM");
            Property(t => t.PILVOLTAJ).HasColumnName("PILVOLTAJ");
            Property(t => t.SONYUKLENENKREDITARIH).HasColumnName("SONYUKLENENKREDITARIH");
            Property(t => t.ABONENO).HasColumnName("ABONENO");
            Property(t => t.ABONETIP).HasColumnName("ABONETIP");
            Property(t => t.ILKPULSETARIH).HasColumnName("ILKPULSETARIH");
            Property(t => t.SONPULSETARIH).HasColumnName("SONPULSETARIH");
            Property(t => t.BORCTARIH).HasColumnName("BORCTARIH");
            Property(t => t.MAXDEBI).HasColumnName("MAXDEBI");
            Property(t => t.MAXDEBISINIR).HasColumnName("MAXDEBISINIR");
            Property(t => t.DONEMGUN).HasColumnName("DONEMGUN");
            Property(t => t.DONEM).HasColumnName("DONEM");
            Property(t => t.VANAACMATARIH).HasColumnName("VANAACMATARIH");
            Property(t => t.VANAKAPAMATARIH).HasColumnName("VANAKAPAMATARIH");
            Property(t => t.SICAKLIK).HasColumnName("SICAKLIK");
            Property(t => t.MINSICAKLIK).HasColumnName("MINSICAKLIK");
            Property(t => t.MAXSICAKLIK).HasColumnName("MAXSICAKLIK");
            Property(t => t.YANGINMODU).HasColumnName("YANGINMODU");
            Property(t => t.ASIRITUKETIM).HasColumnName("ASIRITUKETIM");
            Property(t => t.ARIZA).HasColumnName("ARIZA");
            Property(t => t.SONBILGILENDIRMEZAMANI).HasColumnName("SONBILGILENDIRMEZAMANI");
            Property(t => t.KAYITNO).HasColumnName("KAYITNO").IsRequired();
            Property(t => t.PARAMETRE).HasColumnName("PARAMETRE");
            Property(t => t.ACIKLAMA).HasColumnName("ACIKLAMA");
            Property(t => t.DURUM).HasColumnName("DURUM");
            Property(t => t.KREDIBITTI).HasColumnName("KREDIBITTI");
            Property(t => t.KREDIAZ).HasColumnName("KREDIAZ");
            Property(t => t.ANAPILBITTI).HasColumnName("ANAPILBITTI");
            Property(t => t.SONBAGLANTIZAMAN).HasColumnName("SONBAGLANTIZAMAN");
            Property(t => t.SONOKUMATARIH).HasColumnName("SONOKUMATARIH");
            Property(t => t.PULSEHATA).HasColumnName("PULSEHATA");
            Property(t => t.RTCHATA).HasColumnName("RTCHATA");
            Property(t => t.VANA).HasColumnName("VANA");
            Property(t => t.CEZA1).HasColumnName("CEZA1");
            Property(t => t.CEZA2).HasColumnName("CEZA2");
            Property(t => t.CEZA3).HasColumnName("CEZA3");
            Property(t => t.CEZA4).HasColumnName("CEZA4");
            Property(t => t.MAGNET).HasColumnName("MAGNET");
            Property(t => t.ANAPILZAYIF).HasColumnName("ANAPILZAYIF");
            Property(t => t.OLUSTURAN).HasColumnName("OLUSTURAN");
            Property(t => t.OLUSTURMATARIH).HasColumnName("OLUSTURMATARIH").IsRequired();
            // Relationships
            //HasOptional(t => t.CSTDURUMDURUMEf)
            //  .WithMany(t => t.DURUMENTSAYACSONDURUMSUEfCollection)
            //  .HasForeignKey(d => d.DURUM)
            //  .WillCascadeOnDelete(false);

            HasRequired(t => t.EntSayacEf)
              .WithOptional(t => t.EntSayacSonDurumSuEf);
             

        }
    }
}
