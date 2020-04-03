using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Mappings
{
    public class SYSCSTOPERASYONMap : EntityTypeConfiguration<SYSCSTOPERASYONEf>
    {
        public SYSCSTOPERASYONMap()
        {

            // Table & Column Mappings
            ToTable("SYSCSTOPERASYON");
            // Primary Key
            HasKey(t => t.KAYITNO);
            // Properties
            Property(t => t.KAYITNO).HasColumnName("KAYITNO").IsRequired();
            Property(t => t.AD).HasColumnName("AD").IsRequired();
            Property(t => t.ACIKLAMA).HasColumnName("ACIKLAMA").IsRequired();
            Property(t => t.VERSIYON).HasColumnName("VERSIYON").IsRequired();
            Property(t => t.OLUSTURAN).HasColumnName("OLUSTURAN");
            Property(t => t.OLUSTURMATARIH).HasColumnName("OLUSTURMATARIH");
            Property(t => t.GUNCELLEYEN).HasColumnName("GUNCELLEYEN");
            Property(t => t.GUNCELLEMETARIH).HasColumnName("GUNCELLEMETARIH");
            Property(t => t.MENUKAYITNO).HasColumnName("MENUKAYITNO");
            // Relationships

            //HasOptional(t => t.SysMenuEf)
            //    .WithMany(t => t.SysCstOperasyonEfCollection)
            //    .HasForeignKey(d => d.MENUKAYITNO)
            //    .WillCascadeOnDelete(false);
        }
    }
}
