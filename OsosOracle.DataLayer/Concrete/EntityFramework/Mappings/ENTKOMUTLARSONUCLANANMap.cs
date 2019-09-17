using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Mappings
{
    public class ENTKOMUTLARSONUCLANANMap : EntityTypeConfiguration<ENTKOMUTLARSONUCLANANEf>
    {
        public ENTKOMUTLARSONUCLANANMap()
        {

            // Table & Column Mappings
            ToTable("ENTKOMUTLARSONUCLANAN");
            // Primary Key
            HasKey(t => t.KAYITNO);
            // Properties
            Property(t => t.KAYITNO).HasColumnName("KAYITNO").IsRequired();
            Property(t => t.KONSSERINO).HasColumnName("KONSSERINO");
            Property(t => t.KOMUTKODU).HasColumnName("KOMUTKODU");
            Property(t => t.KOMUT).HasColumnName("KOMUT");
            Property(t => t.ISLEMTARIH).HasColumnName("ISLEMTARIH");
            Property(t => t.SONUC).HasColumnName("SONUC");
            Property(t => t.ISLEMSURESI).HasColumnName("ISLEMSURESI");
            Property(t => t.KOMUTID).HasColumnName("KOMUTID");
            Property(t => t.OLUSTURAN).HasColumnName("OLUSTURAN");
            Property(t => t.OLUSTURMATARIH).HasColumnName("OLUSTURMATARIH").IsRequired();
            Property(t => t.GUNCELLEYEN).HasColumnName("GUNCELLEYEN");
            Property(t => t.GUNCELLEMETARIH).HasColumnName("GUNCELLEMETARIH");
            Property(t => t.VERSIYON).HasColumnName("VERSIYON");
            Property(t => t.ACIKLAMA).HasColumnName("ACIKLAMA");
            Property(t => t.CEVAP).HasColumnName("CEVAP");
            // Relationships
            //HasOptional(t => t.CSTKOMUTLARKOMUTKODUEf)
            //  .WithMany(t => t.KOMUTKODUENTKOMUTLARSONUCLANANEfCollection)
            //  .HasForeignKey(d => d.KOMUTKODU)
            //  .WillCascadeOnDelete(false);

        }
    }
}
