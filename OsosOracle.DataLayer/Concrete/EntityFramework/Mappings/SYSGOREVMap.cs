using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Mappings
{
    public class SYSGOREVMap : EntityTypeConfiguration<SYSGOREVEf>
    {
        public SYSGOREVMap()
        {

            // Table & Column Mappings
            ToTable("SYSGOREV");
            // Primary Key
            HasKey(t => t.KAYITNO);
            // Properties
            Property(t => t.KAYITNO).HasColumnName("KAYITNO").IsRequired();
            Property(t => t.AD).HasColumnName("AD").IsRequired();
            Property(t => t.ACIKLAMA).HasColumnName("ACIKLAMA").IsRequired();
            Property(t => t.VERSIYON).HasColumnName("VERSIYON").IsRequired();
            Property(t => t.OLUSTURAN).HasColumnName("OLUSTURAN").IsRequired();
            Property(t => t.OLUSTURMATARIH).HasColumnName("OLUSTURMATARIH").IsRequired();
            Property(t => t.GUNCELLEYEN).HasColumnName("GUNCELLEYEN");
            Property(t => t.GUNCELLEMETARIH).HasColumnName("GUNCELLEMETARIH");
            Property(t => t.KURUMKAYITNO).HasColumnName("KURUMKAYITNO").IsRequired();
            // Relationships
            HasRequired(t => t.ConKurumEf)
              .WithMany(t => t.SysGorevEfCollection)
              .HasForeignKey(d => d.KURUMKAYITNO)
              .WillCascadeOnDelete(false);

            //HasOptional(t => t.SYSKULLANICIGUNCELLEYENEf)
            //  .WithMany(t => t.GUNCELLEYENSYSGOREVEfCollection)
            //  .HasForeignKey(d => d.GUNCELLEYEN)
            //  .WillCascadeOnDelete(false);

        }
    }
}
