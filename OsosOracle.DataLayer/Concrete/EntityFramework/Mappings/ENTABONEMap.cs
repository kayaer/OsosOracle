using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Mappings
{
    public class ENTABONEMap : EntityTypeConfiguration<ENTABONEEf>
    {
        public ENTABONEMap()
        {

            // Table & Column Mappings
            ToTable("ENTABONE");
            // Primary Key
            HasKey(t => t.KAYITNO);
            // Properties
            Property(t => t.KAYITNO).HasColumnName("KAYITNO").IsRequired();
            Property(t => t.AD).HasColumnName("AD").IsRequired();
            Property(t => t.SOYAD).HasColumnName("SOYAD").IsRequired();
            Property(t => t.DURUM).HasColumnName("DURUM").IsRequired();
            Property(t => t.VERSIYON).HasColumnName("VERSIYON").IsRequired();
            Property(t => t.OLUSTURAN).HasColumnName("OLUSTURAN").IsRequired();
            Property(t => t.OLUSTURMATARIH).HasColumnName("OLUSTURMATARIH").IsRequired();
            Property(t => t.GUNCELLEYEN).HasColumnName("GUNCELLEYEN");
            Property(t => t.GUNCELLEMETARIH).HasColumnName("GUNCELLEMETARIH");
            Property(t => t.KURUMKAYITNO).HasColumnName("KURUMKAYITNO").IsRequired();
            Property(t => t.ABONENO).HasColumnName("ABONENO");
            Property(t => t.SonSatisTarih).HasColumnName("SONSATISTARIH");
            Property(t => t.KimlikNo).HasColumnName("KIMLIKNO");
            Property(t => t.Daire).HasColumnName("DAIRE");
            Property(t => t.Blok).HasColumnName("BLOK");
            Property(t => t.Adres).HasColumnName("ADRES");
            // Relationships


        }
    }
}
