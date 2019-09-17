using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Mappings
{
    public class SYSROLKULLANICIMap : EntityTypeConfiguration<SYSROLKULLANICIEf>
    {
        public SYSROLKULLANICIMap()
        {

            // Table & Column Mappings
            ToTable("SYSROLKULLANICI");
            // Primary Key
            HasKey(t => t.KAYITNO);
            // Properties
            Property(t => t.KAYITNO).HasColumnName("KAYITNO").IsRequired();
            Property(t => t.KULLANICIKAYITNO).HasColumnName("KULLANICIKAYITNO").IsRequired();
            Property(t => t.ROLKAYITNO).HasColumnName("ROLKAYITNO").IsRequired();
            Property(t => t.VERSIYON).HasColumnName("VERSIYON").IsRequired();
            Property(t => t.OLUSTURAN).HasColumnName("OLUSTURAN").IsRequired();
            Property(t => t.OLUSTURMATARIH).HasColumnName("OLUSTURMATARIH").IsRequired();
            Property(t => t.GUNCELLEYEN).HasColumnName("GUNCELLEYEN");
            Property(t => t.GUNCELLEMETARIH).HasColumnName("GUNCELLEMETARIH");
            // Relationships
            HasRequired(t => t.SysKullaniciEf)
              .WithMany(t => t.SysRolKullaniciEfCollection)
              .HasForeignKey(d => d.KULLANICIKAYITNO)
              .WillCascadeOnDelete(false);

            //HasRequired(t => t.SYSKULLANICIOLUSTURANEf)
            //  .WithMany(t => t.OLUSTURANSYSROLKULLANICIEfCollection)
            //  .HasForeignKey(d => d.OLUSTURAN)
            //  .WillCascadeOnDelete(false);

            //HasOptional(t => t.SYSKULLANICIGUNCELLEYENEf)
            //  .WithMany(t => t.GUNCELLEYENSYSROLKULLANICIEfCollection)
            //  .HasForeignKey(d => d.GUNCELLEYEN)
            //  .WillCascadeOnDelete(false);

            HasRequired(t => t.SysRolEf)
              .WithMany(t => t.SysRolKullaniciEfCollection)
              .HasForeignKey(d => d.ROLKAYITNO)
              .WillCascadeOnDelete(false);

        }
    }
}
