using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Mappings
{
    public class SYSKULLANICIMap : EntityTypeConfiguration<SYSKULLANICIEf>
    {
        public SYSKULLANICIMap()
        {

            // Table & Column Mappings
            ToTable("SYSKULLANICI");
            // Primary Key
            HasKey(t => t.KAYITNO);
            // Properties
            Property(t => t.KAYITNO).HasColumnName("KAYITNO").IsRequired();
            Property(t => t.KULLANICIAD).HasColumnName("KULLANICIAD").IsRequired();
            Property(t => t.SIFRE).HasColumnName("SIFRE").IsRequired();
            Property(t => t.VERSIYON).HasColumnName("VERSIYON").IsRequired();
            Property(t => t.OLUSTURAN).HasColumnName("OLUSTURAN").IsRequired();
            Property(t => t.OLUSTURMATARIH).HasColumnName("OLUSTURMATARIH").IsRequired();
            Property(t => t.GUNCELLEYEN).HasColumnName("GUNCELLEYEN");
            Property(t => t.GUNCELLEMETARIH).HasColumnName("GUNCELLEMETARIH");
            Property(t => t.AD).HasColumnName("AD");
            Property(t => t.SOYAD).HasColumnName("SOYAD");
           
            Property(t => t.DURUM).HasColumnName("DURUM").IsRequired();
           
            Property(t => t.DIL).HasColumnName("DIL").IsRequired();
           
            Property(t => t.KURUMKAYITNO).HasColumnName("KURUMKAYITNO");
            // Relationships
            //HasOptional(t => t.SYSCSTKULLANICITURKULLANICITUREf)
            //  .WithMany(t => t.KULLANICITURSYSKULLANICIEfCollection)
            //  .HasForeignKey(d => d.KULLANICITUR)
            //  .WillCascadeOnDelete(false);

            //HasRequired(t => t.CSTDURUMDURUMEf)
            //  .WithMany(t => t.DURUMSYSKULLANICIEfCollection)
            //  .HasForeignKey(d => d.DURUM)
            //  .WillCascadeOnDelete(false);

            //HasOptional(t => t.UstSYSKULLANICIEf)
            //  .WithMany(t => t.AltSYSKULLANICIEfCollection)
            //  .HasForeignKey(d => d.OLUSTURAN)
            //  .WillCascadeOnDelete(false);

            //HasOptional(t => t.UstSYSKULLANICIEf)
            //  .WithMany(t => t.AltSYSKULLANICIEfCollection)
            //  .HasForeignKey(d => d.GUNCELLEYEN)
            //  .WillCascadeOnDelete(false);

            HasRequired(t => t.ConKurumEf)
                .WithMany(t => t.SysKullaniciEfCollection)
                .HasForeignKey(d => d.KURUMKAYITNO)
                .WillCascadeOnDelete(false);

        }
    }
}
