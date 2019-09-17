using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Mappings
{
    public class ENTSATISMap : EntityTypeConfiguration<ENTSATISEf>
    {
        public ENTSATISMap()
        {

            // Table & Column Mappings
            ToTable("ENTSATIS");
            // Primary Key
            HasKey(t => t.KAYITNO);
            // Properties
            Property(t => t.KAYITNO).HasColumnName("KAYITNO").IsRequired();
            Property(t => t.ABONEKAYITNO).HasColumnName("ABONEKAYITNO").IsRequired();
            Property(t => t.SAYACKAYITNO).HasColumnName("SAYACKAYITNO").IsRequired();
            Property(t => t.FATURANO).HasColumnName("FATURANO");
            Property(t => t.ODEME).HasColumnName("ODEME").IsRequired();
            Property(t => t.VERSIYON).HasColumnName("VERSIYON").IsRequired();
            Property(t => t.OLUSTURAN).HasColumnName("OLUSTURAN").IsRequired();
            Property(t => t.OLUSTURMATARIH).HasColumnName("OLUSTURMATARIH").IsRequired();
            Property(t => t.GUNCELLEYEN).HasColumnName("GUNCELLEYEN");
            Property(t => t.GUNCELLEMETARIH).HasColumnName("GUNCELLEMETARIH");
            Property(t => t.IPTAL).HasColumnName("IPTAL");
            Property(t => t.KREDI).HasColumnName("KREDI");
            Property(t => t.YEDEKKREDI).HasColumnName("YEDEKKREDI");
            // Relationships
            HasRequired(t => t.EntAboneEf)
               .WithMany(t => t.SatisEfCollection)
               .HasForeignKey(d => d.ABONEKAYITNO)
               .WillCascadeOnDelete(false);

            HasRequired(t => t.EntSayacEf)
             .WithMany(t => t.EntSatisEfCollection)
             .HasForeignKey(d => d.SAYACKAYITNO)
             .WillCascadeOnDelete(false);

        }
    }
}
