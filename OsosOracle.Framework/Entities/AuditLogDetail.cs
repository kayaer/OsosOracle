using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsosOracle.Framework.Entities
{
    [Table("AUDITLOGDETAIL")]
    public class AuditLogDetail
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public long Id { get; set; }

        [Column("AUDITLOGID")]
        public long AuditLogId { get; set; }

        [ForeignKey("AuditLogId")]
        public virtual AuditLog AuditLog { get; set; }


        [DisplayName("Kolon Adı"), StringLength(50)]
        [Column("KOLONADI")]
        public string KolonAdi { get; set; }

        [DisplayName("Eski Veri")]
        [Column("ESKIVERI")]
        public string EskiVeri { get; set; }

        [DisplayName("Yeni Veri")]
        [Column("YENIVERI")]
        public string YeniVeri { get; set; }



    }
}
