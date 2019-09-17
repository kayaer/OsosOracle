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
    [Table("AUDITLOG")]
    public class AuditLog
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }

        [Required, DisplayName("Kayıt Tipi"), StringLength(1, MinimumLength = 1)]
        [Column("ISLEM")]
        public string Islem { get; set; }

        [Required, DisplayName("Log Zamanı")]
        [Column("EKLEMETARIHI")]
        public System.DateTime EklemeTarihi { get; set; }

        [Required, DisplayName("Tablo Adı"), StringLength(50)]
        [Column("TABLOADI")]
        public string TabloAdi { get; set; }
        [Column("KAYITID")]
        public int KayitId { get; set; }


        [Required, DisplayName("Kullanıcı")]
        [Column("KULLANICIID")]
        public int KullaniciId { get; set; }

        [DisplayName("IP Adres"), StringLength(40)]
        [Column("IP")]
        public string Ip { get; set; }


        public virtual ICollection<AuditLogDetail> AuditLogDetail { get; set; }
    }
}
