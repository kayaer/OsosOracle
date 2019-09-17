using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Mappings
{
    public class SYSGOREVROLMap : EntityTypeConfiguration<SYSGOREVROLEf>
    {
        public SYSGOREVROLMap()
        {

            // Table & Column Mappings
            ToTable("SYSGOREVROL");
            // Primary Key
            HasKey(t => t.KAYITNO);
            // Properties
            Property(t => t.KAYITNO).HasColumnName("KAYITNO").IsRequired();
            Property(t => t.GOREVKAYITNO).HasColumnName("GOREVKAYITNO").IsRequired();
            Property(t => t.ROLKAYITNO).HasColumnName("ROLKAYITNO").IsRequired();
            Property(t => t.VERSIYON).HasColumnName("VERSIYON").IsRequired();
            Property(t => t.OLUSTURAN).HasColumnName("OLUSTURAN").IsRequired();
            Property(t => t.OLUSTURMATARIH).HasColumnName("OLUSTURMATARIH").IsRequired();
            Property(t => t.GUNCELLEYEN).HasColumnName("GUNCELLEYEN");
            Property(t => t.GUNCELLEMETARIH).HasColumnName("GUNCELLEMETARIH");
            // Relationships
            //HasRequired(t => t.SYSKULLANICIOLUSTURANEf)
            //  .WithMany(t => t.OLUSTURANSYSGOREVROLEfCollection)
            //  .HasForeignKey(d => d.OLUSTURAN)
            //  .WillCascadeOnDelete(false);

            //HasOptional(t => t.SYSKULLANICIGUNCELLEYENEf)
            //  .WithMany(t => t.GUNCELLEYENSYSGOREVROLEfCollection)
            //  .HasForeignKey(d => d.GUNCELLEYEN)
            //  .WillCascadeOnDelete(false);

            //HasRequired(t => t.SYSROLROLKAYITNOEf)
            //  .WithMany(t => t.ROLKAYITNOSYSGOREVROLEfCollection)
            //  .HasForeignKey(d => d.ROLKAYITNO)
            //  .WillCascadeOnDelete(false);

            HasRequired(t => t.SysGorevEf)
               .WithMany(t => t.GorevSysGorevRolEfCollection)
               .HasForeignKey(d => d.GOREVKAYITNO)
               .WillCascadeOnDelete(false);

            HasRequired(t => t.SysRolEf)
            .WithMany(t => t.RolSysGorevRolEfCollection)
            .HasForeignKey(d => d.ROLKAYITNO)
            .WillCascadeOnDelete(false);

        }
    }
}
