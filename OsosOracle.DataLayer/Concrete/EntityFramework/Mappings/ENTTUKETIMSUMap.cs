using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Mappings
{
    public class ENTTUKETIMSUMap : EntityTypeConfiguration<ENTTUKETIMSUEf>
    {
        public ENTTUKETIMSUMap()
        {

            // Table & Column Mappings
            ToTable("ENTTUKETIMSU");
            // Primary Key
            HasKey(t => t.KAYITNO);
            // Properties
            Property(t => t.KAYITNO).HasColumnName("KAYITNO").IsRequired();
            Property(t => t.SAYACID).HasColumnName("SAYACID").IsRequired();
            Property(t => t.TUKETIM).HasColumnName("TUKETIM");
            Property(t => t.TUKETIM1).HasColumnName("TUKETIM1");
            Property(t => t.TUKETIM2).HasColumnName("TUKETIM2");
            Property(t => t.TUKETIM3).HasColumnName("TUKETIM3");
            Property(t => t.TUKETIM4).HasColumnName("TUKETIM4");
            Property(t => t.SAYACTARIH).HasColumnName("SAYACTARIH");
            Property(t => t.OKUMATARIH).HasColumnName("OKUMATARIH");
            Property(t => t.DATA).HasColumnName("DATA");
            Property(t => t.HGUN).HasColumnName("HGUN");
            Property(t => t.HARCANANKREDI).HasColumnName("HARCANANKREDI");
            Property(t => t.KALANKREDI).HasColumnName("KALANKREDI");
            Property(t => t.OLUSTURAN).HasColumnName("OLUSTURAN");
            Property(t => t.OLUSTURMATARIH).HasColumnName("OLUSTURMATARIH").IsRequired();
            Property(t => t.GUNCELLEYEN).HasColumnName("GUNCELLEYEN");
            Property(t => t.GUNCELLEMETARIH).HasColumnName("GUNCELLEMETARIH");
            Property(t => t.VERSIYON).HasColumnName("VERSIYON");
            Property(t => t.HEADERNO).HasColumnName("HEADERNO");
            Property(t => t.FATURAMOD).HasColumnName("FATURAMOD");
            // Relationships
            //HasOptional(t => t.ENTHEADERPAKETHEADERNOEf)
            //  .WithMany(t => t.HEADERNOENTTUKETIMSUEfCollection)
            //  .HasForeignKey(d => d.HEADERNO)
            //  .WillCascadeOnDelete(false); 

            HasRequired(t => t.EntSayacDurumSuEf).WithRequiredPrincipal(t => t.EntTuketimSuEf);

        }
    }
}
