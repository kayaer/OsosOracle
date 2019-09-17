using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Mappings
{
    public class ENTABONESAYACMap : EntityTypeConfiguration<ENTABONESAYACEf>
    {
        public ENTABONESAYACMap()
        {

            // Table & Column Mappings
            ToTable("ENTABONESAYAC");
            // Primary Key
            HasKey(t => t.KAYITNO);
            // Properties
            Property(t => t.KAYITNO).HasColumnName("KAYITNO").IsRequired();
            Property(t => t.ABONEKAYITNO).HasColumnName("ABONEKAYITNO").IsRequired();
            Property(t => t.SAYACKAYITNO).HasColumnName("SAYACKAYITNO").IsRequired();
            Property(t => t.SOKULMENEDEN).HasColumnName("SOKULMENEDEN");
            Property(t => t.SONENDEKS).HasColumnName("SONENDEKS");
            Property(t => t.SOKULMETARIH).HasColumnName("SOKULMETARIH");
            Property(t => t.KARTNO).HasColumnName("KARTNO");
            Property(t => t.VERSIYON).HasColumnName("VERSIYON").IsRequired();
            Property(t => t.OLUSTURAN).HasColumnName("OLUSTURAN").IsRequired();
            Property(t => t.OLUSTURMATARIH).HasColumnName("OLUSTURMATARIH").IsRequired();
            Property(t => t.GUNCELLEYEN).HasColumnName("GUNCELLEYEN");
            Property(t => t.GUNCELLEMETARIH).HasColumnName("GUNCELLEMETARIH");
            Property(t => t.TARIFEKAYITNO).HasColumnName("TARIFEKAYITNO");
            Property(t => t.TARIFE).HasColumnName("TARIFE");
            Property(t => t.TAKILMATARIH).HasColumnName("TAKILMATARIH");
            Property(t => t.SONSATISKAYITNO).HasColumnName("SONSATISKAYITNO");
            Property(t => t.SONSATISTARIH).HasColumnName("SONSATISTARIH");
            // Relationships

            HasRequired(t => t.PrmTarifeSuEf)
            .WithMany(t => t.EntAboneSayacEfCollection)
            .HasForeignKey(d => d.TARIFEKAYITNO)
            .WillCascadeOnDelete(false);


            HasRequired(t => t.EntSayacEf)
            .WithMany(t => t.EntAboneSayacEfCollection)
            .HasForeignKey(d => d.SAYACKAYITNO)
            .WillCascadeOnDelete(false);

            HasRequired(t => t.EntSatisEf)
            .WithMany(t => t.EntAboneSayacEfCollection)
            .HasForeignKey(d => d.SONSATISKAYITNO)
            .WillCascadeOnDelete(false);

            HasRequired(t => t.EntAboneEf)
           .WithMany(t => t.AboneSayacEfCollection)
           .HasForeignKey(d => d.ABONEKAYITNO)
           .WillCascadeOnDelete(false);

            HasRequired(t => t.PrmTarifeElkEf)
           .WithMany(t => t.EntAboneSayacEfCollection)
           .HasForeignKey(d => d.TARIFEKAYITNO)
           .WillCascadeOnDelete(false);
        }
    }
}
