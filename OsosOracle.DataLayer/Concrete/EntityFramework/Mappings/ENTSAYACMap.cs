using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Mappings
{
    public class ENTSAYACMap : EntityTypeConfiguration<ENTSAYACEf>
    {
        public ENTSAYACMap()
        {

            // Table & Column Mappings
            ToTable("ENTSAYAC");
            // Primary Key
            HasKey(t => t.KAYITNO);
            // Properties
            Property(t => t.SAYACMONTAJTARIH).HasColumnName("SAYACMONTAJTARIH");
            Property(t => t.KURUMKAYITNO).HasColumnName("KURUMKAYITNO");
            Property(t => t.KAYITNO).HasColumnName("KAYITNO").IsRequired();
            Property(t => t.SERINO).HasColumnName("SERINO").IsRequired();
            Property(t => t.SayacModelKayitNo).HasColumnName("SAYACMODELKAYITNO").IsRequired();
            Property(t => t.ACIKLAMA).HasColumnName("ACIKLAMA");
            Property(t => t.DURUM).HasColumnName("DURUM").IsRequired();
            Property(t => t.VERSIYON).HasColumnName("VERSIYON").IsRequired();
            Property(t => t.OLUSTURAN).HasColumnName("OLUSTURAN").IsRequired();
            Property(t => t.OLUSTURMATARIH).HasColumnName("OLUSTURMATARIH").IsRequired();
            Property(t => t.GUNCELLEYEN).HasColumnName("GUNCELLEYEN");
            Property(t => t.GUNCELLEMETARIH).HasColumnName("GUNCELLEMETARIH");
            Property(t => t.KapakSeriNo).HasColumnName("KAPAKSERINO");
            // Relationships
           

            HasRequired(t => t.CstSayacModelEf)
                        .WithMany(t => t.EntSayacEfCollection)
                        .HasForeignKey(d => d.SayacModelKayitNo)
                        .WillCascadeOnDelete(false);

            HasRequired(t => t.ConKurumEf)
                       .WithMany(t => t.EntSayacEfCollection)
                       .HasForeignKey(d => d.KURUMKAYITNO)
                       .WillCascadeOnDelete(false);

        }
    }
}
