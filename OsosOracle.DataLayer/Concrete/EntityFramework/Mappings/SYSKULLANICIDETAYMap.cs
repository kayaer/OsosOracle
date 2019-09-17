using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Mappings
{
    public class SYSKULLANICIDETAYMap : EntityTypeConfiguration<SYSKULLANICIDETAYEf>
    {
        public SYSKULLANICIDETAYMap()
        {

            // Table & Column Mappings
            ToTable("SYSKULLANICIDETAY");
            // Primary Key
            HasKey(t => t.KAYITNO);
            // Properties
            Property(t => t.KAYITNO).HasColumnName("KAYITNO").IsRequired();
            Property(t => t.EPOSTA).HasColumnName("EPOSTA").IsRequired();
            Property(t => t.GSM).HasColumnName("GSM");
            Property(t => t.KULLANICIKAYITNO).HasColumnName("KULLANICIKAYITNO").IsRequired();
            Property(t => t.VERSIYON).HasColumnName("VERSIYON").IsRequired();
            Property(t => t.OLUSTURAN).HasColumnName("OLUSTURAN");
            Property(t => t.OLUSTURMATARIH).HasColumnName("OLUSTURMATARIH");
            Property(t => t.GUNCELLEYEN).HasColumnName("GUNCELLEYEN");
            Property(t => t.GUNCELLEMETARIH).HasColumnName("GUNCELLEMETARIH");
            // Relationships
            HasRequired(t => t.SYSKULLANICIEf)
              .WithMany(t => t.SysKullaniciDetayEfCollection)
              .HasForeignKey(d => d.KULLANICIKAYITNO)
              .WillCascadeOnDelete(false);

            //HasOptional(t => t.SYSKULLANICIOLUSTURANEf)
            //  .WithMany(t => t.OLUSTURANSYSKULLANICIDETAYEfCollection)
            //  .HasForeignKey(d => d.OLUSTURAN)
            //  .WillCascadeOnDelete(false);

            //HasOptional(t => t.SYSKULLANICIGUNCELLEYENEf)
            //  .WithMany(t => t.GUNCELLEYENSYSKULLANICIDETAYEfCollection)
            //  .HasForeignKey(d => d.GUNCELLEYEN)
            //  .WillCascadeOnDelete(false);

        }
    }
}
