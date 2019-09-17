using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Mappings
{
    public class SYSOPERASYONGOREVMap : EntityTypeConfiguration<SYSOPERASYONGOREVEf>
    {
        public SYSOPERASYONGOREVMap()
        {

            // Table & Column Mappings
            ToTable("SYSOPERASYONGOREV");
            // Primary Key
            HasKey(t => t.KAYITNO);
            // Properties
            Property(t => t.OPERASYONKAYITNO).HasColumnName("OPERASYONKAYITNO").IsRequired();
            Property(t => t.GOREVKAYITNO).HasColumnName("GOREVKAYITNO").IsRequired();
            Property(t => t.VERSIYON).HasColumnName("VERSIYON").IsRequired();
            Property(t => t.OLUSTURAN).HasColumnName("OLUSTURAN").IsRequired();
            Property(t => t.OLUSTURMATARIH).HasColumnName("OLUSTURMATARIH").IsRequired();
            Property(t => t.GUNCELLEYEN).HasColumnName("GUNCELLEYEN");
            Property(t => t.GUNCELLEMETARIH).HasColumnName("GUNCELLEMETARIH");
            Property(t => t.KAYITNO).HasColumnName("KAYITNO").IsRequired();
            // Relationships
            HasRequired(t => t.SysCstOperasyonEf)
              .WithMany(t => t.OperasyonSysOperasyonGorevEfCollection)
              .HasForeignKey(d => d.OPERASYONKAYITNO)
              .WillCascadeOnDelete(false);

            HasRequired(t => t.SysGorevEf)
              .WithMany(t => t.GOREVKAYITNOSYSOPERASYONGOREVEfCollection)
              .HasForeignKey(d => d.GOREVKAYITNO)
              .WillCascadeOnDelete(false);

        }
    }
}
