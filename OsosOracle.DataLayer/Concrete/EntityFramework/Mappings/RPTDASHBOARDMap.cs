using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;

namespace OsosOracle.DataLayer.Concrete.EntityFramework.Mappings
{
    public class RPTDASHBOARDMap : EntityTypeConfiguration<RPTDASHBOARDEf>
    {
        public RPTDASHBOARDMap()
        {

            // Table & Column Mappings
            ToTable("RPTDASHBOARD");
            // Primary Key
            HasKey(t => t.KAYITNO);
            // Properties
            Property(t => t.TARIH).HasColumnName("TARIH").IsRequired();
            Property(t => t.ADET).HasColumnName("ADET").IsRequired();
            Property(t => t.KURUMKAYITNO).HasColumnName("KURUMKAYITNO").IsRequired();
            // Relationships
        }
    }
}
