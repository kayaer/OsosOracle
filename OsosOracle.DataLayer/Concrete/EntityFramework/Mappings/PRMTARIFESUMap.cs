using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Mappings
{
    public class PRMTARIFESUMap : EntityTypeConfiguration<PRMTARIFESUEf>
    {
        public PRMTARIFESUMap()
        {

            // Table & Column Mappings
            ToTable("PRMTARIFESU");
            // Primary Key
            HasKey(t => t.KAYITNO);
            // Properties
            Property(t => t.BORCYUZDE).HasColumnName("BORCYUZDE");
            Property(t => t.KAYITNO).HasColumnName("KAYITNO").IsRequired();
            Property(t => t.AD).HasColumnName("AD").IsRequired();
            Property(t => t.YEDEKKREDI).HasColumnName("YEDEKKREDI").IsRequired();
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
            Property(t => t.FIYAT4).HasColumnName("FIYAT4").IsRequired();
            Property(t => t.FIYAT5).HasColumnName("FIYAT5").IsRequired();
            Property(t => t.LIMIT1).HasColumnName("LIMIT1").IsRequired();
            Property(t => t.LIMIT2).HasColumnName("LIMIT2").IsRequired();
            Property(t => t.LIMIT3).HasColumnName("LIMIT3").IsRequired();
            Property(t => t.LIMIT4).HasColumnName("LIMIT4").IsRequired();
            Property(t => t.TUKETIMKATSAYI).HasColumnName("TUKETIMKATSAYI").IsRequired();
            Property(t => t.KREDIKATSAYI).HasColumnName("KREDIKATSAYI").IsRequired();
            Property(t => t.SABITUCRET).HasColumnName("SABITUCRET").IsRequired();
            Property(t => t.SAYACCAP).HasColumnName("SAYACCAP").IsRequired();
            Property(t => t.AVANSONAY).HasColumnName("AVANSONAY").IsRequired();
            Property(t => t.DONEMGUN).HasColumnName("DONEMGUN").IsRequired();
            Property(t => t.BAYRAM1GUN).HasColumnName("BAYRAM1GUN").IsRequired();
            Property(t => t.BAYRAM1AY).HasColumnName("BAYRAM1AY").IsRequired();
            Property(t => t.BAYRAM1SURE).HasColumnName("BAYRAM1SURE").IsRequired();
            Property(t => t.BAYRAM2GUN).HasColumnName("BAYRAM2GUN").IsRequired();
            Property(t => t.BAYRAM2AY).HasColumnName("BAYRAM2AY").IsRequired();
            Property(t => t.BAYRAM2SURE).HasColumnName("BAYRAM2SURE").IsRequired();
            Property(t => t.MAXDEBI).HasColumnName("MAXDEBI").IsRequired();
            Property(t => t.KRITIKKREDI).HasColumnName("KRITIKKREDI").IsRequired();
            Property(t => t.KURUMKAYITNO).HasColumnName("KURUMKAYITNO").IsRequired();
            Property(t => t.BAGLANTIPERIYOT).HasColumnName("BAGLANTIPERIYOT").IsRequired();
            Property(t => t.YANGINMODSURE).HasColumnName("YANGINMODSURE").IsRequired();
            Property(t => t.BIRIMFIYAT).HasColumnName("BIRIMFIYAT").IsRequired();
            // Relationships
            //HasRequired(t => t.CSTDURUMDURUMEf)
            //  .WithMany(t => t.DURUMPRMTARIFESUEfCollection)
            //  .HasForeignKey(d => d.DURUM)
            //  .WillCascadeOnDelete(false);

            //HasRequired(t => t.SYSKULLANICIOLUSTURANEf)
            //  .WithMany(t => t.OLUSTURANPRMTARIFESUEfCollection)
            //  .HasForeignKey(d => d.OLUSTURAN)
            //  .WillCascadeOnDelete(false);

            HasRequired(t => t.ConKurumEf)
              .WithMany(t => t.PrmTarifeSuEfCollection)
              .HasForeignKey(d => d.KURUMKAYITNO)
              .WillCascadeOnDelete(false);

        }
    }
}
