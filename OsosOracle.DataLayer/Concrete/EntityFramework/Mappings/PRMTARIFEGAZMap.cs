using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Mappings
{
    public class PRMTARIFEGAZMap : EntityTypeConfiguration<PRMTARIFEGAZEf>
    {
        public PRMTARIFEGAZMap()
        {

            // Table & Column Mappings
            ToTable("PRMTARIFEGAZ");
            // Primary Key
            HasKey(t => t.KAYITNO);
            // Properties
            Property(t => t.KAYITNO).HasColumnName("KAYITNO").IsRequired();
            Property(t => t.VERSIYON).HasColumnName("VERSIYON").IsRequired();
            Property(t => t.OLUSTURMATARIH).HasColumnName("OLUSTURMATARIH").IsRequired();
            Property(t => t.OLUSTURAN).HasColumnName("OLUSTURAN").IsRequired();
            Property(t => t.GUNCELLEYEN).HasColumnName("GUNCELLEYEN");
            Property(t => t.GUNCELLEMETARIH).HasColumnName("GUNCELLEMETARIH");
            Property(t => t.TUKETIMLIMIT).HasColumnName("TUKETIMLIMIT").IsRequired();
            Property(t => t.SABAHSAAT).HasColumnName("SABAHSAAT").IsRequired();
            Property(t => t.AKSAMSAAT).HasColumnName("AKSAMSAAT").IsRequired();
            Property(t => t.PULSE).HasColumnName("PULSE").IsRequired();
            Property(t => t.BAYRAM1GUN).HasColumnName("BAYRAM1GUN");
            Property(t => t.BAYRAM1AY).HasColumnName("BAYRAM1AY");
            Property(t => t.BAYRAM1SURE).HasColumnName("BAYRAM1SURE");
            Property(t => t.BAYRAM2GUN).HasColumnName("BAYRAM2GUN");
            Property(t => t.BAYRAM2SURE).HasColumnName("BAYRAM2SURE");
            Property(t => t.BAYRAM2AY).HasColumnName("BAYRAM2AY");
            Property(t => t.KRITIKKREDI).HasColumnName("KRITIKKREDI").IsRequired();
            Property(t => t.SAYACTUR).HasColumnName("SAYACTUR").IsRequired();
            Property(t => t.SAYACCAP).HasColumnName("SAYACCAP").IsRequired();
            Property(t => t.AD).HasColumnName("AD").IsRequired();
            Property(t => t.FIYAT1).HasColumnName("FIYAT1").IsRequired();
            Property(t => t.YEDEKKREDI).HasColumnName("YEDEKKREDI");
            Property(t => t.KURUMKAYITNO).HasColumnName("KURUMKAYITNO");
            Property(t => t.BIRIMFIYAT).HasColumnName("BIRIMFIYAT");
            // Relationships
            HasRequired(t => t.ConKurumEf)
             .WithMany(t => t.PrmTarifeGazEfCollection)
             .HasForeignKey(d => d.KURUMKAYITNO)
             .WillCascadeOnDelete(false);

            //HasOptional(t => t.SYSKULLANICIGUNCELLEYENEf)
            //  .WithMany(t => t.GUNCELLEYENPRMTARIFEGAZEfCollection)
            //  .HasForeignKey(d => d.GUNCELLEYEN)
            //  .WillCascadeOnDelete(false);

        }
    }
}
