using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Mappings
{
    public class ENTKREDIKOMUTTAKIPMap : EntityTypeConfiguration<ENTKREDIKOMUTTAKIPEf>
    {
        public ENTKREDIKOMUTTAKIPMap()
        {

            // Table & Column Mappings
            ToTable("ENTKREDIKOMUTTAKIP");
            // Primary Key
            HasKey(t => t.GUIDID);
            // Properties
            Property(t => t.GUIDID).HasColumnName("GUIDID").IsRequired();
            Property(t => t.KONSSERINO).HasColumnName("KONSSERINO");
            Property(t => t.SATISKAYITNO).HasColumnName("SATISKAYITNO");
            Property(t => t.ISLEMID).HasColumnName("ISLEMID");
            Property(t => t.INTERNET).HasColumnName("INTERNET");
            Property(t => t.KOMUTKODU).HasColumnName("KOMUTKODU");
            Property(t => t.KOMUT).HasColumnName("KOMUT");
            Property(t => t.KREDI).HasColumnName("KREDI");
            Property(t => t.ISLEMTARIH).HasColumnName("ISLEMTARIH");
            Property(t => t.KOMUTID).HasColumnName("KOMUTID");
            Property(t => t.ACIKLAMA).HasColumnName("ACIKLAMA");
            Property(t => t.VERSIYON).HasColumnName("VERSIYON");
            Property(t => t.OLUSTURAN).HasColumnName("OLUSTURAN");
            Property(t => t.OLUSTURMATARIH).HasColumnName("OLUSTURMATARIH").IsRequired();
            Property(t => t.GUNCELLEYEN).HasColumnName("GUNCELLEYEN");
            Property(t => t.GUNCELLEMETARIH).HasColumnName("GUNCELLEMETARIH");
            Property(t => t.SAYACSERINO).HasColumnName("SAYACSERINO");
            Property(t => t.DURUM).HasColumnName("DURUM");
            Property(t => t.MAKBUZTARIH).HasColumnName("MAKBUZTARIH");
            Property(t => t.MAKBUZNO).HasColumnName("MAKBUZNO");
            Property(t => t.ABONENO).HasColumnName("ABONENO");
            // Relationships
            //HasOptional(t => t.ENTHABERLESMEUNITESIKONSSERINOEf)
            //  .WithMany(t => t.KONSSERINOENTKREDIKOMUTTAKIPEfCollection)
            //  .HasForeignKey(d => d.KONSSERINO)
            //  .WillCascadeOnDelete(false);

        }
    }
}
