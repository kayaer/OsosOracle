using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Mappings
{
    public class CSTHUMODELMap : EntityTypeConfiguration<CSTHUMODELEf>
    {
        public CSTHUMODELMap()
        {

            // Table & Column Mappings
            ToTable("CSTHUMODEL");
            // Primary Key
            HasKey(t => t.KAYITNO);
            // Properties
            Property(t => t.ACIKLAMA).HasColumnName("ACIKLAMA");
            Property(t => t.DURUM).HasColumnName("DURUM");
            Property(t => t.VERSIYON).HasColumnName("VERSIYON");
            Property(t => t.OLUSTURAN).HasColumnName("OLUSTURAN");
            Property(t => t.OLUSTURMATARIH).HasColumnName("OLUSTURMATARIH");
            Property(t => t.GUNCELLEYEN).HasColumnName("GUNCELLEYEN");
            Property(t => t.GUNCELLEMETARIH).HasColumnName("GUNCELLEMETARIH");
            Property(t => t.MARKAKAYITNO).HasColumnName("MARKAKAYITNO");
            Property(t => t.YAZILIMVERSIYON).HasColumnName("YAZILIMVERSIYON");
            Property(t => t.CONTROLLER).HasColumnName("CONTROLLER");
            Property(t => t.KAYITNO).HasColumnName("KAYITNO").IsRequired();
            Property(t => t.AD).HasColumnName("AD").IsRequired();
            // Relationships


        }
    }
}
