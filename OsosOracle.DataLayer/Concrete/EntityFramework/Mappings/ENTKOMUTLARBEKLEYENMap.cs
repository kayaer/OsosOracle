using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Mappings
{
    public class ENTKOMUTLARBEKLEYENMap : EntityTypeConfiguration<ENTKOMUTLARBEKLEYENEf>
    {
        public ENTKOMUTLARBEKLEYENMap()
        {

            // Table & Column Mappings
            ToTable("ENTKOMUTLARBEKLEYEN");
            // Primary Key
            HasKey(t => t.GUIDID);
            // Properties
            Property(t => t.KOMUTID).HasColumnName("KOMUTID");
            Property(t => t.OLUSTURAN).HasColumnName("OLUSTURAN");
            Property(t => t.OLUSTURMATARIH).HasColumnName("OLUSTURMATARIH").IsRequired();
            Property(t => t.GUNCELLEYEN).HasColumnName("GUNCELLEYEN");
            Property(t => t.GUNCELLEMETARIH).HasColumnName("GUNCELLEMETARIH");
            Property(t => t.VERSIYON).HasColumnName("VERSIYON");
            Property(t => t.BAGLANTIDENEMESAYISI).HasColumnName("BAGLANTIDENEMESAYISI");
            Property(t => t.ACIKLAMA).HasColumnName("ACIKLAMA");
            Property(t => t.GUIDID).HasColumnName("GUIDID").IsRequired();
            Property(t => t.KONSSERINO).HasColumnName("KONSSERINO");
            Property(t => t.KOMUTKODU).HasColumnName("KOMUTKODU");
            Property(t => t.KOMUT).HasColumnName("KOMUT");
            Property(t => t.ISLEMTARIH).HasColumnName("ISLEMTARIH");
            // Relationships
            //HasOptional(t => t.ENTHABERLESMEUNITESIKONSSERINOEf)
            //  .WithMany(t => t.KONSSERINOENTKOMUTLARBEKLEYENEfCollection)
            //  .HasForeignKey(d => d.KONSSERINO)
            //  .WillCascadeOnDelete(false);

        }
    }
}
