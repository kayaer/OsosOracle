using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Mappings
{
    public class NESNEDEGERMap : EntityTypeConfiguration<NESNEDEGEREf>
    {
        public NESNEDEGERMap()
        {

            // Table & Column Mappings
            ToTable("NESNEDEGER");
            // Primary Key
            HasKey(t => t.KAYITNO);
            // Properties
            Property(t => t.KAYITNO).HasColumnName("KAYITNO").IsRequired();
            Property(t => t.NESNETIPKAYITNO).HasColumnName("NESNETIPKAYITNO").IsRequired();
            Property(t => t.AD).HasColumnName("AD").IsRequired();
            Property(t => t.DEGER).HasColumnName("DEGER");
            Property(t => t.BILGI).HasColumnName("BILGI");
            Property(t => t.SIRANO).HasColumnName("SIRANO").IsRequired();
            Property(t => t.OLUSTURAN).HasColumnName("OLUSTURAN").IsRequired();
            Property(t => t.OLUSTURMATARIH).HasColumnName("OLUSTURMATARIH").IsRequired();
            Property(t => t.GUNCELLEYEN).HasColumnName("GUNCELLEYEN");
            Property(t => t.GUNCELLEMETARIH).HasColumnName("GUNCELLEMETARIH");
            Property(t => t.VERSIYON).HasColumnName("VERSIYON").IsRequired();
            // Relationships
            HasRequired(t => t.NESNETIPNESNETIPKAYITNOEf)
              .WithMany(t => t.NESNETIPKAYITNONESNEDEGEREfCollection)
              .HasForeignKey(d => d.NESNETIPKAYITNO)
              .WillCascadeOnDelete(false);

        }
    }
}
