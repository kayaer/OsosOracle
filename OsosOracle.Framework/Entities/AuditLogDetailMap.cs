using System.Data.Entity.ModelConfiguration;

namespace OsosOracle.Framework.Entities
{
    internal class AuditLogDetailMap : EntityTypeConfiguration<AuditLogDetail>
    {
        public AuditLogDetailMap()
        {
            this.HasRequired(t => t.AuditLog)
               .WithMany(t => t.AuditLogDetail)
               .HasForeignKey(t => t.AuditLogId)
               .WillCascadeOnDelete(false);
        }

    }
}
