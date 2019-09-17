using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Mappings
{
    public class ENTABONEBILGIMap : EntityTypeConfiguration<ENTABONEBILGIEf>
    {
        public ENTABONEBILGIMap()
        {

            // Table & Column Mappings
            ToTable("ENTABONEBILGI");
            // Primary Key
            HasKey(t => t.KAYITNO);
            // Properties
            Property(t => t.BORC).HasColumnName("BORC");
            Property(t => t.TELEFON).HasColumnName("TELEFON");
            Property(t => t.IKIM3BILGI).HasColumnName("IKIM3BILGI");
            Property(t => t.BESM3BILGI).HasColumnName("BESM3BILGI");
            Property(t => t.KIMLIKNO).HasColumnName("KIMLIKNO");
            Property(t => t.BLOKE).HasColumnName("BLOKE");
            Property(t => t.BLOKEACIKLAMA).HasColumnName("BLOKEACIKLAMA");
            Property(t => t.KAYITNO).HasColumnName("KAYITNO").IsRequired();
            Property(t => t.ABONENO).HasColumnName("ABONENO").IsRequired();
            Property(t => t.ABONEKAYITNO).HasColumnName("ABONEKAYITNO").IsRequired();
            Property(t => t.EPOSTA).HasColumnName("EPOSTA");
            Property(t => t.GSM).HasColumnName("GSM");
            Property(t => t.ADRES).HasColumnName("ADRES");
            Property(t => t.VERSIYON).HasColumnName("VERSIYON").IsRequired();
            Property(t => t.OLUSTURAN).HasColumnName("OLUSTURAN").IsRequired();
            Property(t => t.OLUSTURMATARIH).HasColumnName("OLUSTURMATARIH").IsRequired();
            Property(t => t.GUNCELLEYEN).HasColumnName("GUNCELLEYEN");
            Property(t => t.GUNCELLEMETARIH).HasColumnName("GUNCELLEMETARIH");
            // Relationships


            HasRequired(t => t.EntAboneEf)
              .WithMany(t => t.AboneBilgiEfCollection)
              .HasForeignKey(d => d.ABONEKAYITNO)
              .WillCascadeOnDelete(false);



        }
    }
}
