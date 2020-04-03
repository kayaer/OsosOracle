using System.ComponentModel.DataAnnotations.Schema;
			using System.Data.Entity.ModelConfiguration;
			using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
			
			namespace OsosOracle.DataLayer.Concrete.EntityFramework.Mappings
			{
				public class CONDILMap : EntityTypeConfiguration<CONDILEf>
				{
					public CONDILMap()
					{
		
					// Table & Column Mappings
					ToTable("CONDIL");
					// Primary Key
					HasKey(t => t.KAYITNO);
					// Properties
			Property(t => t.KAYITNO).HasColumnName("KAYITNO").IsRequired();
Property(t => t.DIL).HasColumnName("DIL").IsRequired();
// Relationships
     }
  }
}
