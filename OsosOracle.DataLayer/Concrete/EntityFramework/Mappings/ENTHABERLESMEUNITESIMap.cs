using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Mappings
{
    public class ENTHABERLESMEUNITESIMap : EntityTypeConfiguration<ENTHABERLESMEUNITESIEf>
    {
        public ENTHABERLESMEUNITESIMap()
        {

            // Table & Column Mappings
            ToTable("ENTHABERLESMEUNITESI");
            // Primary Key
            HasKey(t => t.KAYITNO);
            // Properties
            Property(t => t.KAYITNO).HasColumnName("KAYITNO").IsRequired();
            Property(t => t.SERINO).HasColumnName("SERINO").IsRequired();
            Property(t => t.SIMTELNO).HasColumnName("SIMTELNO");
            Property(t => t.IP).HasColumnName("IP");
            Property(t => t.ACIKLAMA).HasColumnName("ACIKLAMA");
            Property(t => t.DURUM).HasColumnName("DURUM").IsRequired();
            Property(t => t.VERSIYON).HasColumnName("VERSIYON").IsRequired();
            Property(t => t.OLUSTURAN).HasColumnName("OLUSTURAN").IsRequired();
            Property(t => t.OLUSTURMATARIH).HasColumnName("OLUSTURMATARIH").IsRequired();
            Property(t => t.GUNCELLEYEN).HasColumnName("GUNCELLEYEN");
            Property(t => t.GUNCELLEMETARIH).HasColumnName("GUNCELLEMETARIH");
            Property(t => t.MARKA).HasColumnName("MARKA");
            Property(t => t.MODEL).HasColumnName("MODEL");
            Property(t => t.KURUMKAYITNO).HasColumnName("KURUMKAYITNO").IsRequired();
            // Relationships
            HasRequired(t => t.CstHuMarkaEf)
              .WithMany(t => t.EntHaberlesmeUnitesiEfCollection)
              .HasForeignKey(d => d.MARKA)
              .WillCascadeOnDelete(false);

            HasRequired(t => t.CstHuModelEf)
              .WithMany(t => t.EntHaberlesmeUnitesiEfCollection)
              .HasForeignKey(d => d.MODEL)
              .WillCascadeOnDelete(false);

            HasRequired(t => t.ConKurumEf)
             .WithMany(t => t.EntHaberlesmeUnitesiEfCollection)
             .HasForeignKey(d => d.KURUMKAYITNO)
             .WillCascadeOnDelete(false);
        }
    }
}
