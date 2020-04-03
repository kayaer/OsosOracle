using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Mappings
{
    public class CSTSAYACMODELMap : EntityTypeConfiguration<CSTSAYACMODELEf>
    {
        public CSTSAYACMODELMap()
        {

            // Table & Column Mappings
            ToTable("CSTSAYACMODEL");
            // Primary Key
            HasKey(t => t.KAYITNO);
            // Properties
            Property(t => t.KAYITNO).HasColumnName("KAYITNO").IsRequired();
            Property(t => t.AD).HasColumnName("AD").IsRequired();
            Property(t => t.ACIKLAMA).HasColumnName("ACIKLAMA");
            Property(t => t.DURUM).HasColumnName("DURUM");
            Property(t => t.VERSIYON).HasColumnName("VERSIYON");
            Property(t => t.OLUSTURAN).HasColumnName("OLUSTURAN");
            Property(t => t.OLUSTURMATARIH).HasColumnName("OLUSTURMATARIH");
            Property(t => t.GUNCELLEYEN).HasColumnName("GUNCELLEYEN");
            Property(t => t.GUNCELLEMETARIH).HasColumnName("GUNCELLEMETARIH");
            Property(t => t.MARKAKAYITNO).HasColumnName("MARKAKAYITNO");
          
            Property(t => t.SayacTuruKayitNo).HasColumnName("SAYACTURUKAYITNO");

            // Relationships
            HasRequired(t => t.NesneDegerSayacTuruEf)
              .WithMany(t => t.CstSayacModelEfCollection)
              .HasForeignKey(d => d.SayacTuruKayitNo)
              .WillCascadeOnDelete(false);

            //HasOptional(t => t.CSTSAYACMARKAMARKAKAYITNOEf)
            //  .WithMany(t => t.MARKAKAYITNOCSTSAYACMODELEfCollection)
            //  .HasForeignKey(d => d.MARKAKAYITNO)
            //  .WillCascadeOnDelete(false);

            //HasOptional(t => t.SYSKULLANICIOLUSTURANEf)
            //  .WithMany(t => t.OLUSTURANCSTSAYACMODELEfCollection)
            //  .HasForeignKey(d => d.OLUSTURAN)
            //  .WillCascadeOnDelete(false);

            //HasOptional(t => t.SYSKULLANICIGUNCELLEYENEf)
            //  .WithMany(t => t.GUNCELLEYENCSTSAYACMODELEfCollection)
            //  .HasForeignKey(d => d.GUNCELLEYEN)
            //  .WillCascadeOnDelete(false);

        }
    }
}
