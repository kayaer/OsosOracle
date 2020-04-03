using System.ComponentModel.DataAnnotations.Schema;
			using System.Data.Entity.ModelConfiguration;
			using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
			
			namespace OsosOracle.DataLayer.Concrete.EntityFramework.Mappings
			{
				public class NESNETIPMap : EntityTypeConfiguration<NESNETIPEf>
				{
					public NESNETIPMap()
					{
		
					// Table & Column Mappings
					ToTable("NESNETIP");
					// Primary Key
					HasKey(t => t.KAYITNO);
					// Properties
			Property(t => t.KAYITNO).HasColumnName("KAYITNO").IsRequired();
Property(t => t.ADI).HasColumnName("ADI").IsRequired();
Property(t => t.OLUSTURAN).HasColumnName("OLUSTURAN").IsRequired();
Property(t => t.OLUSTURMATARIH).HasColumnName("OLUSTURMATARIH").IsRequired();
Property(t => t.GUNCELLEYEN).HasColumnName("GUNCELLEYEN");
Property(t => t.GUNCELLEMETARIH).HasColumnName("GUNCELLEMETARIH");
Property(t => t.VERSIYON).HasColumnName("VERSIYON").IsRequired();
// Relationships
     }
  }
}
