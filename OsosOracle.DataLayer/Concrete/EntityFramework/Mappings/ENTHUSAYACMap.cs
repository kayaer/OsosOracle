using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Mappings
{
    public class ENTHUSAYACMap : EntityTypeConfiguration<ENTHUSAYACEf>
    {
        public ENTHUSAYACMap()
        {

            // Table & Column Mappings
            ToTable("ENTHUSAYAC");
            // Primary Key
            HasKey(t => t.KAYITNO);
            // Properties
            Property(t => t.KAYITNO).HasColumnName("KAYITNO").IsRequired();
            Property(t => t.HUKAYITNO).HasColumnName("HUKAYITNO").IsRequired();
            Property(t => t.SAYACKAYITNO).HasColumnName("SAYACKAYITNO").IsRequired();
            Property(t => t.VERSIYON).HasColumnName("VERSIYON").IsRequired();
            Property(t => t.OLUSTURAN).HasColumnName("OLUSTURAN").IsRequired();
            Property(t => t.OLUSTURMATARIH).HasColumnName("OLUSTURMATARIH").IsRequired();
            Property(t => t.GUNCELLEYEN).HasColumnName("GUNCELLEYEN");
            Property(t => t.GUNCELLEMETARIH).HasColumnName("GUNCELLEMETARIH");
            // Relationships
            HasRequired(t => t.ENTHABERLESMEUNITESIHUKAYITNOEf)
              .WithMany(t => t.HUKAYITNOENTHUSAYACEfCollection)
              .HasForeignKey(d => d.HUKAYITNO)
              .WillCascadeOnDelete(false);

            HasRequired(t => t.ENTSAYACSAYACKAYITNOEf)
              .WithMany(t => t.EntHuSayacEfCollection)
              .HasForeignKey(d => d.SAYACKAYITNO)
              .WillCascadeOnDelete(false);

        }
    }
}
