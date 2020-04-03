using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Mappings
{
    public class CONKURUMMap : EntityTypeConfiguration<CONKURUMEf>
    {
        public CONKURUMMap()
        {

            // Table & Column Mappings
            ToTable("CONKURUM");
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
           
            // Relationships
            //HasOptional(t => t.CONSAYACURUNAILESAYACURUNAILEEf)
            //  .WithMany(t => t.SAYACURUNAILECONKURUMEfCollection)
            //  .HasForeignKey(d => d.SAYACURUNAILE)
            //  .WillCascadeOnDelete(false);

            //HasOptional(t => t.SYSKULLANICIOLUSTURANEf)
            //  .WithMany(t => t.OLUSTURANCONKURUMEfCollection)
            //  .HasForeignKey(d => d.OLUSTURAN)
            //  .WillCascadeOnDelete(false);

            //HasOptional(t => t.SYSKULLANICIGUNCELLEYENEf)
            //  .WithMany(t => t.GUNCELLEYENCONKURUMEfCollection)
            //  .HasForeignKey(d => d.GUNCELLEYEN)
            //  .WillCascadeOnDelete(false);

        }
    }
}
