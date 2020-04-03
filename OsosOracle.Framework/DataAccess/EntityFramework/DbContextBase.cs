using OsosOracle.Framework.Entities;
using OsosOracle.Framework.Infrastructure;
using OsosOracle.Framework.Utilities.Comparators;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OsosOracle.Framework.DataAccess.EntityFramework
{
    /// <summary>
    /// Base context 
    /// Loglama burada yapılır
    /// </summary>
    public abstract class DbContextBase : DbContext
    {
        protected DbContextBase(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            //queryi output a yazar
            Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
        }

        public DbSet<AuditLog> AuditLog { get; set; }
        public DbSet<AuditLogDetail> AuditLogDetail { get; set; }

        public override int SaveChanges()
        {

            var changes = ChangeTracker.Entries().Where(p => p.State == EntityState.Deleted || p.State == EntityState.Modified).ToList();
            var changeLogs = GenerateAuditLogs(changes);

            AuditLog.AddRange(changeLogs);


            //versiyonlama, ekleme ve değiştirme bilgileri ve soft silme işlemleri
            //BU BÖLÜMDE değiştirilen bilgiler loglanmayacaktır
            foreach (var dbEntityEntry in ChangeTracker.Entries().Where(x => x.State == EntityState.Added || x.State == EntityState.Modified || x.State == EntityState.Deleted))
            {
                IEntity entity = dbEntityEntry.Entity as IEntity;
                if (entity != null)
                {

                    if (dbEntityEntry.State == EntityState.Added)
                    {
                        entity.OLUSTURMATARIH = DateTime.Now;
                        //entity.OLUSTURAN = CurrenUser.Identity.UserId.HasValue ? CurrenUser.Identity.UserId.Value : -1;

                        //soft delete ise eklendiğinde silindisi false olmalı
                        IEntitySoftDelete entityD = dbEntityEntry.Entity as IEntitySoftDelete;
                        if (entityD != null)
                        {
                            entityD.DURUM = 1;
                        }



                    }
                    else if (dbEntityEntry.State == EntityState.Modified)
                    {
                        entity.GUNCELLEMETARIH = DateTime.Now;
                        //entity.GUNCELLEYEN = CurrenUser.Identity.UserId;
                    }

                    else if (dbEntityEntry.State == EntityState.Deleted) //soft delete işlemi
                    {
                        IEntitySoftDelete entityD = dbEntityEntry.Entity as IEntitySoftDelete;
                        if (entityD != null)
                        {
                            dbEntityEntry.State = EntityState.Modified;
                            dbEntityEntry.CurrentValues.SetValues(dbEntityEntry.OriginalValues);


                            entityD.DURUM = 2;
                            entity.GUNCELLEMETARIH = DateTime.Now;
                            //entity.GUNCELLEYEN = CurrenUser.Identity.UserId;
                            //dbEntityEntry.State = EntityState.Modified;
                        }

                    }

                    entity.VERSIYON = entity.VERSIYON + 1;

                }


            }

            return base.SaveChanges();
        }



        private IEnumerable<AuditLog> GenerateAuditLogs(IEnumerable<DbEntityEntry> entries)
        {
            foreach (var entity in entries)
            {

                INolog noLogEntity = entity.Entity as INolog;

                if (noLogEntity == null) // nolog interfacesinde olmayanlar loglanır
                {
                    var auditLog = GenerateAuditLog(entity);

                    if (auditLog != null)
                    {
                        yield return auditLog;
                    }

                }

            }
        }

        private AuditLog GenerateAuditLog(DbEntityEntry entity)
        {
            if (entity.State == EntityState.Detached)
            {
                // no need to log audit entries.
                return null;
            }

            string crud = null;
            if (entity.State == EntityState.Modified)
            {
                crud = "U";
            }
            else if (entity.State == EntityState.Deleted)
            {
                crud = "D";
            }

            var auditLog = new AuditLog
            {
                Islem = crud,
                TabloAdi = GetTableName(entity), // entity.Entity.GetType().Name.Split('_')[0],
                KayitId = GetPrimaryKeyValue(entity),
                EklemeTarihi = DateTime.Now,
                //KullaniciId = CurrenUser.Identity.UserId.HasValue ? CurrenUser.Identity.UserId.Value : -1,
                //Ip = CurrenUser.Identity.UserIp,
                AuditLogDetail = GenerateChangeLogDetails(entity)
            };


            return auditLog;
        }




        /// <summary>
        ///   GENEL KURAL her tabloda Id kolonu olacak , ara tablolarda dahil
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private int GetPrimaryKeyValue(DbEntityEntry entity)
        {



            if (entity.Property("KAYITNO") == null)
            {
                var type = entity.Entity.GetType();
                var name = type.Name;

                if (type.BaseType != null)
                {
                    //EF tipin  herzaman base entity si var
                    name = type.BaseType?.Name;
                }

                throw new MissingPrimaryKeyException(name + " table has no KAYITNO column");
            }

            //GENEL KURAL her tabloda Id kolonu olacak , ara tablolarda dahil
            return entity.Property("KAYITNO").CurrentValue.ToInt32();



            /*
            var type = entity.Entity.GetType();
            IEnumerable<System.Reflection.PropertyInfo> keys = GetPrimaryKeyProperties(type);
          

          
            int result =-1 ;
            foreach (var key in keys)
            {
                result = key.GetValue(entity.Entity).ToInt32();
            }

            return result;*/
        }
        private string GetTableName(DbEntityEntry entity)
        {
            var type = entity.Entity.GetType();
            if (type.BaseType != null)
            {
                //EF tipin  herzaman base entity si var
                return type.BaseType?.Name;
            }

            return type.Name;





        }



        private static List<AuditLogDetail> GenerateChangeLogDetails(DbEntityEntry entity)
        {
            var details = new List<AuditLogDetail>();


            var values = entity.State == EntityState.Deleted ? entity.OriginalValues : entity.CurrentValues;

            foreach (var propName in values.PropertyNames)
            {
                var detail = GenerateChangeLogDetail(entity, propName);

                if (detail != null)
                {
                    details.Add(detail);
                }
            }

            return details;
        }

        private static AuditLogDetail GenerateChangeLogDetail(DbEntityEntry entity, string propName)
        {
            object eskiVeri = null;
            if (entity.State == EntityState.Modified || entity.State == EntityState.Deleted)
            {
                eskiVeri = entity.OriginalValues.GetValue<object>(propName);
            }

            object yeniVeri;

            if (entity.State == EntityState.Deleted)
            {
                yeniVeri = null;
            }
            else
            {
                yeniVeri = entity.Property(propName).CurrentValue;
            }

            if (AreEqual(entity, propName, eskiVeri, yeniVeri))
            {
                return null;
            }

            return new AuditLogDetail
            {
                KolonAdi = propName,
                EskiVeri = eskiVeri?.ToString(),
                YeniVeri = yeniVeri?.ToString(),
            };

        }

        private static bool AreEqual(DbEntityEntry entity, string propName, object eskiVeri, object yeniVeri)
        {
            // Eski ve yeni veriler ayni olsa bile kaydediyoruz.
            // return false;

            var propertyType = entity.Entity.GetType().GetProperty(propName).PropertyType;
            var comparator = ComparatorFactory.GetComparator(propertyType);

            return comparator.AreEqual(eskiVeri, yeniVeri);

        }

        public override Task<int> SaveChangesAsync()
        {
            throw new NotSupportedException("Async is not supported in GTHBDbContext");
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            throw new NotSupportedException("Async is not supported in GTHBDbContext");
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AuditLogDetailMap());
            base.OnModelCreating(modelBuilder);
        }
    }
}
