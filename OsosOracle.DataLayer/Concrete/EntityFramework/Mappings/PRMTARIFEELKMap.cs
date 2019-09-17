using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Mappings
{
    public class PRMTARIFEELKMap : EntityTypeConfiguration<PRMTARIFEELKEf>
    {
        public PRMTARIFEELKMap()
        {

            // Table & Column Mappings
            ToTable("PRMTARIFEELK");
            // Primary Key
            HasKey(t => t.KAYITNO);
            // Properties
            Property(t => t.KREDIKATSAYI).HasColumnName("KREDIKATSAYI").IsRequired();
            Property(t => t.KAYITNO).HasColumnName("KAYITNO").IsRequired();
            Property(t => t.AD).HasColumnName("AD").IsRequired();
            Property(t => t.YEDEKKREDI).HasColumnName("YEDEKKREDI").IsRequired();
            Property(t => t.KRITIKKREDI).HasColumnName("KRITIKKREDI").IsRequired();
            Property(t => t.CARPAN).HasColumnName("CARPAN").IsRequired();
            Property(t => t.ACIKLAMA).HasColumnName("ACIKLAMA");
            Property(t => t.DURUM).HasColumnName("DURUM").IsRequired();
            Property(t => t.VERSIYON).HasColumnName("VERSIYON").IsRequired();
            Property(t => t.OLUSTURAN).HasColumnName("OLUSTURAN").IsRequired();
            Property(t => t.OLUSTURMATARIH).HasColumnName("OLUSTURMATARIH").IsRequired();
            Property(t => t.GUNCELLEYEN).HasColumnName("GUNCELLEYEN");
            Property(t => t.GUNCELLEMETARIH).HasColumnName("GUNCELLEMETARIH");
            Property(t => t.FIYAT1).HasColumnName("FIYAT1").IsRequired();
            Property(t => t.FIYAT2).HasColumnName("FIYAT2").IsRequired();
            Property(t => t.FIYAT3).HasColumnName("FIYAT3").IsRequired();
            Property(t => t.LIMIT1).HasColumnName("LIMIT1").IsRequired();
            Property(t => t.LIMIT2).HasColumnName("LIMIT2").IsRequired();
            Property(t => t.YUKLEMELIMIT).HasColumnName("YUKLEMELIMIT").IsRequired();
            Property(t => t.SABAHSAAT).HasColumnName("SABAHSAAT").IsRequired();
            Property(t => t.AKSAMSAAT).HasColumnName("AKSAMSAAT").IsRequired();
            Property(t => t.HAFTASONUAKSAM).HasColumnName("HAFTASONUAKSAM").IsRequired();
            Property(t => t.SABITUCRET).HasColumnName("SABITUCRET").IsRequired();
            Property(t => t.BAYRAM1GUN).HasColumnName("BAYRAM1GUN").IsRequired();
            Property(t => t.BAYRAM1AY).HasColumnName("BAYRAM1AY").IsRequired();
            Property(t => t.BAYRAM1SURE).HasColumnName("BAYRAM1SURE").IsRequired();
            Property(t => t.BAYRAM2GUN).HasColumnName("BAYRAM2GUN").IsRequired();
            Property(t => t.BAYRAM2AY).HasColumnName("BAYRAM2AY").IsRequired();
            Property(t => t.BAYRAM2SURE).HasColumnName("BAYRAM2SURE").IsRequired();
            Property(t => t.KURUMKAYITNO).HasColumnName("KURUMKAYITNO");
            // Relationships
            HasRequired(t => t.ConKurumEf)
             .WithMany(t => t.PrmTarifeElkEfCollection)
             .HasForeignKey(d => d.KURUMKAYITNO)
             .WillCascadeOnDelete(false);

            //HasOptional(t => t.SYSKULLANICIGUNCELLEYENEf)
            //  .WithMany(t => t.GUNCELLEYENPRMTARIFEELKEfCollection)
            //  .HasForeignKey(d => d.GUNCELLEYEN)
            //  .WillCascadeOnDelete(false);

        }
    }
}
