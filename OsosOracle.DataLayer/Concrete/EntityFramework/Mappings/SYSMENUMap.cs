using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Mappings
{
    public class SYSMENUMap : EntityTypeConfiguration<SYSMENUEf>
    {
        public SYSMENUMap()
        {

            // Table & Column Mappings
            ToTable("SYSMENU");
            // Primary Key
            HasKey(t => t.KAYITNO);
            // Properties
            Property(t => t.KAYITNO).HasColumnName("KAYITNO").IsRequired();
            Property(t => t.TR).HasColumnName("TR").IsRequired();
            Property(t => t.PARENTKAYITNO).HasColumnName("PARENTKAYITNO");
            Property(t => t.MENUORDER).HasColumnName("MENUORDER").IsRequired();          
            Property(t => t.AREA).HasColumnName("AREA");
            Property(t => t.ACTION).HasColumnName("ACTION");
            Property(t => t.CONTROLLER).HasColumnName("CONTROLLER");         
            Property(t => t.DURUM).HasColumnName("DURUM");
            Property(t => t.VERSIYON).HasColumnName("VERSIYON").IsRequired();
            Property(t => t.OLUSTURAN).HasColumnName("OLUSTURAN");
            Property(t => t.OLUSTURMATARIH).HasColumnName("OLUSTURMATARIH");
            Property(t => t.GUNCELLEYEN).HasColumnName("GUNCELLEYEN");
            Property(t => t.GUNCELLEMETARIH).HasColumnName("GUNCELLEMETARIH");
            Property(t => t.ICON).HasColumnName("ICON");
            // Relationships
            //HasOptional(t => t.CSTDURUMDURUMEf)
            //  .WithMany(t => t.DURUMSYSMENUEfCollection)
            //  .HasForeignKey(d => d.DURUM)
            //  .WillCascadeOnDelete(false);

            //HasOptional(t => t.SYSKULLANICIOLUSTURANEf)
            //  .WithMany(t => t.OLUSTURANSYSMENUEfCollection)
            //  .HasForeignKey(d => d.OLUSTURAN)
            //  .WillCascadeOnDelete(false);

            //HasOptional(t => t.SYSKULLANICIGUNCELLEYENEf)
            //  .WithMany(t => t.GUNCELLEYENSYSMENUEfCollection)
            //  .HasForeignKey(d => d.GUNCELLEYEN)
            //  .WillCascadeOnDelete(false);

        }
    }
}
