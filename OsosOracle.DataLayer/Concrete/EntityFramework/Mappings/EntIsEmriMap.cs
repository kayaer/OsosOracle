using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Mappings
{
    public class EntIsEmriMap : EntityTypeConfiguration<EntIsEmriEf>
    {
        public EntIsEmriMap()
        {

            // Table & Column Mappings
            ToTable("ENTISEMRI");
            // Primary Key
            HasKey(t => t.KAYITNO);
            // Properties
            Property(t => t.KAYITNO).HasColumnName("KAYITNO").IsRequired();
            Property(t => t.SayacKayitNo).HasColumnName("SAYACKAYITNO");
            Property(t => t.IsEmriKayitNo).HasColumnName("ISEMRIKAYITNO");
            Property(t => t.Parametre).HasColumnName("PARAMETRE");
            Property(t => t.IsEmriDurumKayitNo).HasColumnName("ISEMRIDURUMKAYITNO");
            Property(t => t.IsEmriCevap).HasColumnName("ISEMRICEVAP");
            Property(t => t.Cevap).HasColumnName("CEVAP");
            Property(t => t.OlusturmaTarih).HasColumnName("OLUSTURMATARIH");
            Property(t => t.GuncellemeTarih).HasColumnName("GUNCELLEMETARIH");

            HasRequired(t => t.EntSayacEf)
            .WithMany(t => t.EntIsEmriEfCollection)
            .HasForeignKey(d => d.SayacKayitNo)
            .WillCascadeOnDelete(false);

            HasRequired(t => t.NesneDegerIsEmriEf)
          .WithMany(t => t.EntIsEmriEfCollection)
          .HasForeignKey(d => d.IsEmriKayitNo)
          .WillCascadeOnDelete(false);
          
            HasRequired(t => t.NesneDegerIsEmriDurumEf)
         .WithMany(t => t.EntIsEmriDurumEfCollection)
         .HasForeignKey(d => d.IsEmriDurumKayitNo)
         .WillCascadeOnDelete(false);

        }
    }
}
