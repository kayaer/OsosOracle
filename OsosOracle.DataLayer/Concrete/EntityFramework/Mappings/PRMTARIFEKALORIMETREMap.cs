using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Mappings
{
    public class PRMTARIFEKALORIMETREMap : EntityTypeConfiguration<PRMTARIFEKALORIMETREEf>
    {
        public PRMTARIFEKALORIMETREMap()
        {

            // Table & Column Mappings
            ToTable("PRMTARIFEKALORIMETRE");
            // Primary Key
            HasKey(t => t.KAYITNO);
            // Properties
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
            Property(t => t.LIMIT1).HasColumnName("LIMIT1").IsRequired();
            Property(t => t.LIMIT2).HasColumnName("LIMIT2").IsRequired();
            Property(t => t.Kdv).HasColumnName("KDV");
            Property(t => t.Ctv).HasColumnName("CTV");
            Property(t => t.SAYACTIP).HasColumnName("SAYACTIP");
            Property(t => t.TUKETIMKATSAYI).HasColumnName("TUKETIMKATSAYI");
            Property(t => t.KREDIKATSAYI).HasColumnName("KREDIKATSAYI");
            Property(t => t.BAYRAM1GUN).HasColumnName("BAYRAM1GUN");
            Property(t => t.BAYRAM1AY).HasColumnName("BAYRAM1AY");
            Property(t => t.BAYRAM1SURE).HasColumnName("BAYRAM1SURE");
            Property(t => t.BAYRAM2GUN).HasColumnName("BAYRAM2GUN");
            Property(t => t.BAYRAM2AY).HasColumnName("BAYRAM2AY");
            Property(t => t.BAYRAM2SURE).HasColumnName("BAYRAM2SURE");
            Property(t => t.KURUMKAYITNO).HasColumnName("KURUMKAYITNO");
            Property(t => t.AylikBakimBedeli).HasColumnName("AYLIKBAKIMBEDELI");
            Property(t => t.BirimFiyat).HasColumnName("BIRIMFIYAT");
            Property(t => t.RezervKredi).HasColumnName("REZERVKREDI");
            // Relationships
            HasRequired(t => t.ConKurumEf)
           .WithMany(t => t.PrmTarifeKALORIMETREEfCollection)
           .HasForeignKey(d => d.KURUMKAYITNO)
           .WillCascadeOnDelete(false);

            //HasRequired(t => t.SYSKULLANICIOLUSTURANEf)
            //  .WithMany(t => t.OLUSTURANPRMTARIFEKALORIMETREEfCollection)
            //  .HasForeignKey(d => d.OLUSTURAN)
            //  .WillCascadeOnDelete(false);

            //HasOptional(t => t.SYSKULLANICIGUNCELLEYENEf)
            //  .WithMany(t => t.GUNCELLEYENPRMTARIFEKALORIMETREEfCollection)
            //  .HasForeignKey(d => d.GUNCELLEYEN)
            //  .WillCascadeOnDelete(false);

        }
    }
}
